using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TpUpgradeItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _numText;
    [SerializeField] private TextMeshProUGUI _effectText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private TextMeshProUGUI _upgradeLevelText;
    [SerializeField] private TextMeshProUGUI _requirementText;

    private int _currentLevel;
    private int _maxLevel;
    private List<List<int>> _unlockRequirement;

    private int _index;
    private string _costEquation;

    private bool _unlocked = false;
    private bool _maxed = false;

    public void SetNameText(string str)
    {
        _nameText.text = str;
    }
    public void SetNumText(string str)
    {
        _numText.text = str;
    }
    public void SetEffectText(string str)
    {
        _effectText.text = str;
    }
    private Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();
    public void SetUpgradeLevelText()
    {
        if (!_unlocked || _currentLevel == 0)   // 잠김 상태
        {
            _upgradeLevelText.text = "잠김";  // csv..??
        }
        else    // 해금 상태
        {
            switch (_maxLevel)
            {
                case 0:
                    _upgradeLevelText.text = $"Lv. {_currentLevel}";
                    break;
                case 1:
                    _upgradeLevelText.text = "잠금 해제됨";  // csv..??
                    break;
                default:
                    _upgradeLevelText.text = $"Lv. {_currentLevel} / {_maxLevel}";
                    break;
            }
        }
    }
    public void SetRequirementText()
    {
        if (_currentLevel > 0)
        {
            _requirementText.text = "";
            return;
        }

        List<string> resultTextList = new List<string>();
        foreach (List<int> intPair in _unlockRequirement)
        {
            int upgradeNumber = intPair[0];
            int currentLevel = (int)UserData.TPUpgrades[upgradeNumber].UpgradeCount;
            int requirement = intPair[1];

            resultTextList.Add($"{upgradeNumber}: {currentLevel} / {requirement}");
        }
        _requirementText.text = string.Join("\t", resultTextList);
    }

    public void UpdateAllData(int index, string costEquation, int currentLevel, int maxLevel, List<List<int>> unlockRequirement)
    {
        _index = index;
        _costEquation = costEquation;

        _currentLevel = currentLevel;
        _maxLevel = maxLevel;

        _unlockRequirement = unlockRequirement;

        _maxed = _maxLevel != 0 && _currentLevel >= _maxLevel;
        _unlocked = true;

        foreach (List<int> intPair in unlockRequirement)
        {
            if (UserData.TPUpgrades[intPair[0]].UpgradeCount < intPair[1])
            {
                _unlocked = false;
                return;
            }
        }
    }

    public void SetCostText()
    {
        if (!_unlocked)     // 잠김 상태
        {
            _costText.text = "잠금 해제 조건\n미충족";  // csv..??
        }
        else if (_currentLevel == 0)    // 해금 가능하지만 아직 안한 상태
        {
            inputMap.Clear();
            if (!UserData.TPUpgrades.ContainsKey(_index)) { UserData.TPUpgrades.Add(_index, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[_index].UpgradeCount));
            _costText.text = $"잠금 해제\n{Managers.Definition.CalcEquationToStringForStr(inputMap, _costEquation)} TP";  // csv..??
        }
        else if (_maxed && _maxLevel > 1)
        {
            _costText.text = "최고 레벨";  // csv..??
        }
        else                // 해금 상태
        {
            if (_maxLevel == 1)
            {
                _costText.text = "잠금 해제됨";  // csv..??
            }
            else
            {
                inputMap.Clear();
                if (!UserData.TPUpgrades.ContainsKey(_index)) { UserData.TPUpgrades.Add(_index, new TPUpgrade(false, 0, false)); }
                inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[_index].UpgradeCount));
                _costText.text = $"업그레이드\n{Managers.Definition.CalcEquationToStringForStr(inputMap, _costEquation)} TP";  // csv..??
            }
        }
    }

    public async void OnClick_TpUpgrade()
    {
        if (_unlocked && !_maxed)
        {
            TpUpgradeSingleNetworkRequest req = new TpUpgradeSingleNetworkRequest();
            req.upgrade = _index;
            req.upgradeCount = 1;
            await Managers.Network.API_TpUpgrade(req);

            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_TPUPGRADE);
        }
    }
}
