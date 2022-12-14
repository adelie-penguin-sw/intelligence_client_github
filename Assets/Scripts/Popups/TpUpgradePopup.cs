using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

namespace InGame
{
    public class TpUpgradePopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _cost;

        [SerializeField] private GameObject _buttonLocked;
        [SerializeField] private GameObject _buttonUnlocked;

        [SerializeField] private GameObject _requirement1;
        [SerializeField] private GameObject _requirement2;
        [SerializeField] private GameObject _requirement3;
        [SerializeField] private GameObject _hiddenText;

        [SerializeField] private Image _requirement1Image;
        [SerializeField] private Image _requirement2Image;
        [SerializeField] private Image _requirement3Image;

        [SerializeField] private TextMeshProUGUI _requirement1Level;
        [SerializeField] private TextMeshProUGUI _requirement2Level;
        [SerializeField] private TextMeshProUGUI _requirement3Level;

        [SerializeField] private TextMeshProUGUI _upgradeEffect;
        [SerializeField] private TextMeshProUGUI _upgradeText;

        private TpUpgradeItem _item;

        private int _index;
        private bool _unlocked;
        private bool _maxed;

        private bool _canView = false;

        private Dictionary<string, UpArrowNotation> _inputMap = new Dictionary<string, UpArrowNotation>();

        public void Init(TpUpgradeItem item)
        {
            base.Init();
            Set(item);
        }

        public void Set(TpUpgradeItem item)
        {
            base.Set();

            _item = item;

            _canView = _item.Unlocked || GetRequirementSum() > 0;

            _name.text = _canView ? _item.NameText : "???";
            if (!_item.Unlocked)
            {
                _currentLevel.text = "Locked";
            }
            else if (_item.Maxed && _item.CurrentLevel == 1)
            {
                _currentLevel.text = "Unlocked";
            }
            else
            {
                _currentLevel.text = $"Lv. {_item.CurrentLevel}";
            }
            _description.text = _canView ? _item.EffectText : "??????";
            _cost.text = _item.CostText;

            _buttonLocked.SetActive(!_item.Unlocked);
            _buttonUnlocked.SetActive(_item.Unlocked);

            _hiddenText.SetActive(!_canView);
            _upgradeEffect.gameObject.SetActive(_item.MaxLevel != 1);

            if (_canView)
            {
                if (!_item.Unlocked)
                {
                    if (_item.UnlockRequirement.Count >= 3)
                    {
                        _requirement3.SetActive(true);
                        _requirement3Image.sprite = LoadTPUIcon(_item.UnlockRequirement[2][0]);
                        _requirement3Level.text = _item.RequirementText[2];
                    }
                    else { _requirement3.SetActive(false); }
                    if (_item.UnlockRequirement.Count >= 2)
                    {
                        _requirement2.SetActive(true);
                        _requirement2Image.sprite = LoadTPUIcon(_item.UnlockRequirement[1][0]);
                        _requirement2Level.text = _item.RequirementText[1];
                    }
                    else { _requirement2.SetActive(false); }
                    _requirement1.SetActive(true);
                    _requirement1Image.sprite = LoadTPUIcon(_item.UnlockRequirement[0][0]);
                    _requirement1Level.text = _item.RequirementText[0];
                }
                else
                {
                    _upgradeEffect.text = GetEffectValueText();
                    _upgradeText.text = _item.Maxed ? _item.MaxLevel != 1 ? "Max Level" : "Unlocked" : "Upgrade";
                }
            }
            else
            {
                _requirement1.SetActive(false);
                _requirement2.SetActive(false);
                _requirement3.SetActive(false);
            }

            _index = _item.Index;
            _unlocked = _item.Unlocked;
            _maxed = _item.Maxed;
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void Dispose()
        {
            base.Dispose();
            Managers.Notification.PostNotification(ENotiMessage.EXIT_TPUPGRADE_POPUP);
        }

        private Sprite LoadTPUIcon(int index)
        {
            return Resources.Load<Sprite>($"Sprites/TPU{index.ToString("D3")}Icon");
        }

        public async void OnClick_TpUpgrade()
        {
            if (_unlocked && !_maxed)
            {
                TpUpgradeSingleNetworkRequest req = new TpUpgradeSingleNetworkRequest();
                req.upgrade = _index;
                req.upgradeCount = 1;
                await Managers.Network.API_TpUpgrade(req);

                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.CLASS_TPU_ITEM, _item);
                Managers.Notification.PostNotification(ENotiMessage.ONCLICK_TPUPGRADE, _sendData);
            }
        }

        private int GetRequirementSum()
        {
            int total = 0;
            foreach (string txt in _item.RequirementText)
            {
                string[] numList = txt.Split('/');
                total += int.Parse(numList[0]);
            }
            return total;
        }

        private string GetEffectValueText()
        {
            if (!string.IsNullOrEmpty(_item.EffectValueText))
            {
                UpArrowNotation currentEffect;
                UpArrowNotation nextEffect;

                if (_item.Index == 2)
                {
                    currentEffect = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.baseIntellectLimitList)[_item.CurrentLevel];
                    nextEffect = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.baseIntellectLimitList)[_item.CurrentLevel + 1];
                    return string.Format(!_item.Maxed ? _item.EffectValueText : GetCurrentEffectValueText(), currentEffect.ToString(), nextEffect.ToString());
                }

                _inputMap.Clear();
                _inputMap.Add("upgradeCount", new UpArrowNotation(_item.CurrentLevel));
                currentEffect = Managers.Definition.CalcEquationForStr(_inputMap, _item.EffectValueEquation);
                _inputMap["upgradeCount"] += 1;
                nextEffect = Managers.Definition.CalcEquationForStr(_inputMap, _item.EffectValueEquation);

                return string.Format(!_item.Maxed ? _item.EffectValueText : GetCurrentEffectValueText(), currentEffect.ToString(), nextEffect.ToString());
            }

            return "";
        }

        private string GetCurrentEffectValueText()
        {
            string newStr = string.Copy(_item.EffectValueText);
            newStr = newStr.Replace(" -> ", "|");
            newStr = newStr.Replace(" > ", "|");

            return newStr.Split('|')[0];
        }
    }
}
