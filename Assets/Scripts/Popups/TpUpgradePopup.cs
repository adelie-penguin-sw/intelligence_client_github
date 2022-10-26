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

        public void Init(TpUpgradeItem item)
        {
            base.Init();
            Set(item);
        }

        public void Set(TpUpgradeItem item)
        {
            base.Set();

            _item = item;

            _name.text = item.NameText;
            if (!item.Unlocked)
            {
                _currentLevel.text = "Locked";
            }
            else if (item.Maxed && item.CurrentLevel == 1)
            {
                _currentLevel.text = "Unlocked";
            }
            else
            {
                _currentLevel.text = $"Lv. {item.CurrentLevel}";
            }
            _description.text = item.EffectText;
            _cost.text = item.CostText;

            _buttonLocked.SetActive(!item.Unlocked);
            _buttonUnlocked.SetActive(item.Unlocked);

            if (!item.Unlocked)
            {
                if (item.UnlockRequirement.Count >= 3)
                {
                    _requirement3.SetActive(true);
                    _requirement3Image.sprite = LoadTPUIcon(item.UnlockRequirement[2][0]);
                    _requirement3Level.text = item.RequirementText[2];
                }
                else { _requirement3.SetActive(false); }
                if (item.UnlockRequirement.Count >= 2)
                {
                    _requirement2.SetActive(true);
                    _requirement2Image.sprite = LoadTPUIcon(item.UnlockRequirement[1][0]);
                    _requirement2Level.text = item.RequirementText[1];
                }
                else { _requirement2.SetActive(false); }
                _requirement1.SetActive(true);
                _requirement1Image.sprite = LoadTPUIcon(item.UnlockRequirement[0][0]);
                _requirement1Level.text = item.RequirementText[0];
            }
            else
            {
                _upgradeEffect.text = "Some Text...";        // TODO: csv에 TPU 수치적 효과 텍스트까지 추가되면 반영하기
                _upgradeText.text = item.Maxed ? "Max Level" : "Upgrade";
            }

            _index = item.Index;
            _unlocked = item.Unlocked;
            _maxed = item.Maxed;
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void Dispose()
        {
            base.Dispose();
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
    }
}
