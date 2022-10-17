using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MainTab;

namespace InGame
{
    /// <summary>
    /// 브레인의 정보를 나타내는 팝업 클래스<br />
    /// 브레인 판매 가능<br />
    /// </summary>
    public class BrainInfoPopup : PopupBase
    {
        [SerializeField] private Button _sellBtn;
        [SerializeField] private Button _resetBtn;
        [SerializeField] private Button _upgradeMultiplierBtn;
        [SerializeField] private Button _upgradeLimitBtn;

        [SerializeField] private GameObject _intellectLimitArea;
        [SerializeField] private GameObject _multiplierArea;
        [SerializeField] private GameObject _storedNPArea;
        [SerializeField] private GameObject _distanceArea;
        [SerializeField] private GameObject _statusArea;

        [SerializeField] private Brain _brain;

        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _intellectText;
        [SerializeField] private TextMeshProUGUI _intellectLimitText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _npText;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _statusText;
        [SerializeField] private TextMeshProUGUI _upgradeMultiplierCost;
        [SerializeField] private TextMeshProUGUI _upgradeLimitCost;
        [SerializeField] private TextMeshProUGUI _decomposeReward;

        private List<Brain> _deletableSenderList = new List<Brain>();

        private Dictionary<string, UpArrowNotation> _inputMap = new Dictionary<string, UpArrowNotation>();

        public void Init(Brain brain, BrainNetwork brainNetwork)
        {
            base.Init();
            Set(brain, brainNetwork);
        }
        public void Set(Brain brain, BrainNetwork brainNetwork)
        {
            base.Set();
            _brain = brain;
            _deletableSenderList.Clear();

            ItemActiveSetting(_brain.BrainData.brainType == EBrainType.NORMALBRAIN);

            // 해당 브레인을 삭제할 때 같이 삭제되는 모든 브레인들의 id리스트
            if (_brain.BrainData.brainType == EBrainType.NORMALBRAIN)
            {
                List<long> deletableSenderIdList = UserData.SingleNetworkWrapper.GetAllDeletableSenderIdListForID(_brain.BrainData.id);

                foreach (var deletableSenderID in deletableSenderIdList)
                {
                    _deletableSenderList.Add(brainNetwork.GetBrainForID(deletableSenderID));
                }
            }

            UpdateInfo();
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            // current intellect
            _inputMap.Clear();
            _inputMap.Add("intellect", _brain.BrainData.Intellect);
            _inputMap.Add("tpu008", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
            UpArrowNotation storedNP = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainDecomposingGainEquation));
            _intellectText.text = _brain.BrainData.Intellect.ToString(ECurrencyType.INTELLECT);

            if (_brain.BrainData.brainType == EBrainType.NORMALBRAIN)
            {
                // stored NP
                _npText.text = storedNP.ToString();

                UpArrowNotation totalSenderNP = new UpArrowNotation(0);
                foreach (Brain brain in _deletableSenderList)
                {
                    _inputMap.Clear();
                    _inputMap.Add("intellect", brain.Intellect);
                    _inputMap.Add("tpu008", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
                    totalSenderNP += Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainDecomposingGainEquation));
                }
                _decomposeReward.text = _deletableSenderList.Count == 0 ?
                    string.Format("Decompose\nfor {0} NP\n", storedNP.ToString(ECurrencyType.NP)) :
                    string.Format("Decompose\nfor {0} NP\n+ {1} NP", storedNP.ToString(ECurrencyType.NP), totalSenderNP.ToString(ECurrencyType.NP));   // "총" 획득 NP량 계산해서 표시
            }
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        // 실시간으로 업데이트할 필요 없는 정보들은 전부 여기다 넣고 Set()이랑 onclick()에서 호출
        private void UpdateInfo()
        {
            // id
            _idText.text = _brain.BrainData.id.ToString();

            // brain type
            switch (_brain.BrainData.brainType)
            {
                case EBrainType.COREBRAIN:
                    _typeText.text = "Core Brain";
                    break;
                case EBrainType.NORMALBRAIN:
                    _typeText.text = "Normal Brain";
                    break;
                default:
                    _typeText.text = "Unknown";
                    break;
            }

            // current intellect limit
            _intellectLimitText.text = _brain.CurrentIntellectLimit.ToString(ECurrencyType.INTELLECT);

            // current multiplier
            _multiplierText.text = "x" + _brain.Multiplier.ToString(ECurrencyType.MULTIPLIER);

            // current distance
            _distanceText.text = _brain.BrainData.distance.ToString();

            // current status
            if (_brain.ReceiverIdList.Count == 0)
            {
                _statusText.text = "Disconnected";
                _statusText.color = new Color32(255, 0, 0, 255);
            }
            else if (_brain.Intellect.Top3Layer[0] == 0f)
            {
                _statusText.text = "Idle";
                _statusText.color = new Color32(255, 255, 255, 255);
            }
            else if (_brain.IsLocked)
            {
                _statusText.text = "Locked At Limit";
                _statusText.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                _statusText.text = "Working";
                _statusText.color = new Color32(255, 255, 255, 255);
            }

            // multiplier upgrade btn text
            _inputMap.Clear();
            _inputMap.Add("upgradeCount", new UpArrowNotation(_brain.BrainData.multiplierUpgradeCount));
            _inputMap.Add("tpu012", new UpArrowNotation(UserData.TPUpgrades[12].UpgradeCount));
            _inputMap.Add("tpu022", new UpArrowNotation(UserData.TPUpgrades[22].UpgradeCount));
            UpArrowNotation multiplierUpgradeCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainMultiplierUpgradeCostEquation));
            string multiplierUpgradeText;
            if (UserData.TPUpgrades[0].UpgradeCount > 0)
            {
                multiplierUpgradeText = _brain.SenderIdList.Count == 0 ? "+1 Intellect" : $"x{UserData.TPUpgrades[1].UpgradeCount + 2} Multiplier";
            }
            else
            {
                multiplierUpgradeText = "+1 Intellect";
            }
            _upgradeMultiplierCost.text = string.Format(multiplierUpgradeText + "\nCost: {0} NP", multiplierUpgradeCost.ToString(ECurrencyType.NP));

            // limit upgrade btn text
            _inputMap.Clear();
            _inputMap.Add("distance", new UpArrowNotation(_brain.Distance));
            _inputMap.Add("upgradeCount", new UpArrowNotation(_brain.BrainData.limitUpgradeCount));
            _inputMap.Add("tpu002", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));
            UpArrowNotation limitUpgradeCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainLimitUpgradeCostEquation));
            string limitUpgradeText = $"Break Limit to {_brain.GetNextIntellectLimit().ToString(ECurrencyType.INTELLECT)}";
            _upgradeLimitCost.text = string.Format(limitUpgradeText + "\nCost: {0} NP", limitUpgradeCost.ToString(ECurrencyType.NP));
        }

        public void OnClick_DecomposeBrain()
        {
            Hashtable _sendData = new Hashtable();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, _brain.BrainData);
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_SELL_BRAIN, _sendData);
            Dispose();
        }

        public void OnClick_UpgradeBrainMultiplier()
        {
            if (_brain.BrainData != null)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.BRAIN_ID, _brain.BrainData.id);
                Managers.Notification.PostNotification(ENotiMessage.ONCLICK_UPGRADE_BRAIN_MULTIPLIER, _sendData);
                UpdateInfo();
            }
            else
            {
                Debug.LogError("브레인 데이터 Null");
            }
        }

        public void OnClick_UpgradeBrainLimit()
        {
            if (_brain.BrainData != null)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.BRAIN_ID, _brain.BrainData.id);
                Managers.Notification.PostNotification(ENotiMessage.ONCLICK_UPGRADE_BRAIN_LIMIT, _sendData);
                UpdateInfo();
            }
            else
            {
                Debug.LogError("브레인 데이터 Null");
            }
        }

        public void OnClick_Reset()
        {
            Managers.Notification.PostNotification(ENotiMessage.EXPERIMENT_COMPLETE);
        }

        private void ItemActiveSetting(bool isNormalBrain)
        {
            _resetBtn.gameObject.SetActive(!isNormalBrain);

            // 코어 브레인은 업그레이드 및 분해 등의 동작이 필요하지 않으므로 버튼 비활성화
            _sellBtn.gameObject.SetActive(isNormalBrain);
            _upgradeMultiplierBtn.gameObject.SetActive(isNormalBrain);
            _upgradeLimitBtn.gameObject.SetActive(isNormalBrain);

            // 코어 브레인은 증폭계수, NP축적량, 거리, 상태 개념이 필요하지 않으므로 표시영역 모두 비활성화
            _intellectLimitArea.SetActive(isNormalBrain);
            _multiplierArea.SetActive(isNormalBrain && UserData.TPUpgrades[0].UpgradeCount > 0);    // 증폭계수 언락 전까지는 표시영역 비활성화
            _storedNPArea.SetActive(isNormalBrain);
            _distanceArea.SetActive(isNormalBrain && _brain.ReceiverIdList.Count > 0);              // 네트워크에 연결하기 전까지는 표시영역 비활성화
            _statusArea.SetActive(isNormalBrain);
        }
    }
}
