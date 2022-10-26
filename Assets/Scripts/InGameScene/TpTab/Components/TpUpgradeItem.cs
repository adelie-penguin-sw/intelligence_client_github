using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TpUpgradeItem : MonoBehaviour
{
    private const int gridUnitSize = 200;
    private static Dictionary<int, List<int>> _itemPosDict = new Dictionary<int, List<int>>
    {
        {0, new List<int> { 0 * gridUnitSize, 0 * gridUnitSize } },
        {1, new List<int> { 0 * gridUnitSize, 1 * gridUnitSize } },
        {2, new List<int> { 0 * gridUnitSize, 2 * gridUnitSize } },
        {3, new List<int> { 3 * gridUnitSize, 2 * gridUnitSize } },
        {4, new List<int> { 0 * gridUnitSize, 3 * gridUnitSize } },
        {5, new List<int> { 1 * gridUnitSize, 3 * gridUnitSize } },
        {6, new List<int> { 2 * gridUnitSize, 3 * gridUnitSize } },
        {7, new List<int> { 3 * gridUnitSize, 3 * gridUnitSize } },
        {8, new List<int> { 0 * gridUnitSize, 4 * gridUnitSize } },
        {9, new List<int> { 1 * gridUnitSize, 4 * gridUnitSize } },
        {10, new List<int> { 3 * gridUnitSize, 4 * gridUnitSize } },
        {11, new List<int> { -3 * gridUnitSize, 5 * gridUnitSize } },
        {12, new List<int> { -2 * gridUnitSize, 5 * gridUnitSize } },
        {13, new List<int> { -1 * gridUnitSize, 5 * gridUnitSize } },
        {14, new List<int> { 0 * gridUnitSize, 5 * gridUnitSize } },
        {15, new List<int> { 3 * gridUnitSize, 5 * gridUnitSize } },
        {16, new List<int> { 0 * gridUnitSize, 6 * gridUnitSize } },
        {17, new List<int> { 0 * gridUnitSize, 0 * gridUnitSize } },    // ASN (제외된 항목)
        {18, new List<int> { 1 * gridUnitSize, 6 * gridUnitSize } },
        {19, new List<int> { 3 * gridUnitSize, 6 * gridUnitSize } },
        {20, new List<int> { 4 * gridUnitSize, 6 * gridUnitSize } },
        {21, new List<int> { -3 * gridUnitSize, 7 * gridUnitSize } },
        {22, new List<int> { -2 * gridUnitSize, 7 * gridUnitSize } },
        {23, new List<int> { -1 * gridUnitSize, 7 * gridUnitSize } },
        {24, new List<int> { 1 * gridUnitSize, 7 * gridUnitSize } },
        {25, new List<int> { 0 * gridUnitSize, 8 * gridUnitSize } },
        {26, new List<int> { 3 * gridUnitSize, 8 * gridUnitSize } },
        {27, new List<int> { 0 * gridUnitSize, 9 * gridUnitSize } },
        {28, new List<int> { 3 * gridUnitSize, 9 * gridUnitSize } },
        {29, new List<int> { 0 * gridUnitSize, 10 * gridUnitSize } },
    };

    [SerializeField] private GameObject _lockedIcon;
    [SerializeField] private Image _tpuIcon;

    private string _nameText;
    public string NameText { get { return _nameText; } }
    private string _effectText;
    public string EffectText { get { return _effectText; } }
    private string _costText;
    public string CostText { get { return _costText; } }
    //[SerializeField] private TextMeshProUGUI _upgradeLevelText;
    private List<string> _requirementText;
    public List<string> RequirementText { get { return _requirementText; } }

    private int _currentLevel;
    public int CurrentLevel { get { return _currentLevel; } }
    private int _maxLevel;
    public int MaxLevel { get { return _maxLevel; } }
    private List<List<int>> _unlockRequirement;
    public List<List<int>> UnlockRequirement { get { return _unlockRequirement; } }

    private int _index;
    public int Index { get { return _index; } }
    private string _costEquation;

    private bool _unlocked = false;
    public bool Unlocked { get { return _unlocked; } }
    private bool _maxed = false;
    public bool Maxed { get { return _maxed; } }

    private InGame.TpUpgradePopup _tpuPopup = null;

    public void SetImage(int index)
    {
        _tpuIcon.sprite = Resources.Load<Sprite>($"Sprites/TPU{index.ToString("D3")}Icon");
        _tpuIcon.color = new Color32(255, 255, 255, _unlocked ? (byte)255 : (byte)70);
        _lockedIcon.SetActive(!_unlocked);
        transform.position = new Vector3(_itemPosDict[index][0], _itemPosDict[index][1]);
    }

    public void SetNameText(string str)
    {
        _nameText = str;
    }
    public void SetNumText(string str)
    {
        //_numText.text = str;
    }
    public void SetEffectText(string str)
    {
        _effectText = str;
    }
    private Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();
    public void SetUpgradeLevelText()
    {
        if (!_unlocked || _currentLevel == 0)   // 잠김 상태
        {
            //_upgradeLevelText.text = "잠김";  // csv..??
        }
        else    // 해금 상태
        {
            switch (_maxLevel)
            {
                case 0:
                    //_upgradeLevelText.text = $"Lv. {_currentLevel}";
                    break;
                case 1:
                    //_upgradeLevelText.text = "잠금 해제됨";  // csv..??
                    break;
                default:
                    //_upgradeLevelText.text = $"Lv. {_currentLevel} / {_maxLevel}";
                    break;
            }
        }
    }
    public void SetRequirementText()
    {
        if (_currentLevel > 0)
        {
            return;
        }

        List<string> resultTextList = new List<string>();
        foreach (List<int> intPair in _unlockRequirement)
        {
            int upgradeNumber = intPair[0];
            int currentLevel = (int)UserData.TPUpgrades[upgradeNumber].UpgradeCount;
            int requirement = intPair[1];

            resultTextList.Add($"{currentLevel}/{requirement}");
        }
        _requirementText = resultTextList;
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
            _costText = "Cost: ??? TP";  // csv..??
        }
        else if (_currentLevel == 0)    // 해금 가능하지만 아직 안한 상태
        {
            inputMap.Clear();
            if (!UserData.TPUpgrades.ContainsKey(_index)) { UserData.TPUpgrades.Add(_index, new TPUpgrade(false, 0, false)); }
            inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[_index].UpgradeCount));
            _costText = $"Cost: {Managers.Definition.CalcEquationForStr(inputMap, _costEquation).ToString(ECurrencyType.TP)} TP";  // csv..??
        }
        else if (_maxed && _maxLevel > 1)
        {
            _costText = "";  // csv..??
        }
        else                // 해금 상태
        {
            if (_maxLevel == 1)
            {
                _costText = "";  // csv..??
            }
            else
            {
                inputMap.Clear();
                if (!UserData.TPUpgrades.ContainsKey(_index)) { UserData.TPUpgrades.Add(_index, new TPUpgrade(false, 0, false)); }
                inputMap.Add("upgradeCount", new UpArrowNotation(UserData.TPUpgrades[_index].UpgradeCount));
                _costText = $"Cost: {Managers.Definition.CalcEquationForStr(inputMap, _costEquation).ToString(ECurrencyType.TP)} TP";  // csv..??
            }
        }
    }

    public void OnClick_TpUpgradeOpen()
    {
        Hashtable _sendData = new Hashtable();
        _sendData.Add(EDataParamKey.CLASS_TPU_ITEM, this);
        Managers.Notification.PostNotification(ENotiMessage.ONCLICK_TPUPGRADE_ICON, _sendData);
    }
}
