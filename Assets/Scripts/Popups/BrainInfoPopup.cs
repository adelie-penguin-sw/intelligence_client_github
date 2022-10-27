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
        [SerializeField] private Brain _brain;

        [SerializeField] private Button _decomposeBtn;
        [SerializeField] private Button _resetBtn;
        [SerializeField] private Button _upgradeMultiplierBtn;
        [SerializeField] private Button _upgradeLimitBtn;

        [SerializeField] private GameObject _intellectLimitArea;
        [SerializeField] private GameObject _multiplierArea;
        [SerializeField] private GameObject _storedNPArea;
        [SerializeField] private GameObject _distanceArea;
        [SerializeField] private GameObject _statusArea;
        [SerializeField] private GameObject _bulkUpgradeArea;

        [SerializeField] private TextMeshProUGUI _popupTitle;

        [SerializeField] private TextMeshProUGUI _idKey;
        [SerializeField] private TextMeshProUGUI _typeKey;
        [SerializeField] private TextMeshProUGUI _intellectKey;
        [SerializeField] private TextMeshProUGUI _intellectLimitKey;
        [SerializeField] private TextMeshProUGUI _multiplierKey;
        [SerializeField] private TextMeshProUGUI _npKey;
        [SerializeField] private TextMeshProUGUI _distanceKey;
        [SerializeField] private TextMeshProUGUI _statusKey;

        [SerializeField] private TextMeshProUGUI _idValue;
        [SerializeField] private TextMeshProUGUI _typeValue;
        [SerializeField] private TextMeshProUGUI _intellectValue;
        [SerializeField] private TextMeshProUGUI _intellectLimitValue;
        [SerializeField] private TextMeshProUGUI _multiplierValue;
        [SerializeField] private TextMeshProUGUI _npValue;
        [SerializeField] private TextMeshProUGUI _distanceValue;
        [SerializeField] private TextMeshProUGUI _statusValue;

        [SerializeField] private TextMeshProUGUI _resetButtonText;
        [SerializeField] private TextMeshProUGUI _upgradeMultiplierCost;
        [SerializeField] private TextMeshProUGUI _upgradeLimitCost;
        [SerializeField] private TextMeshProUGUI _decomposeReward;

        [SerializeField] private TextMeshProUGUI _bulkUpgradeCountText;

        private List<Brain> _deletableSenderList = new List<Brain>();

        private Dictionary<string, UpArrowNotation> _inputMap = new Dictionary<string, UpArrowNotation>();

        private int _bulkUpgradeCountIndex = 0;
        private int _bulkUpgradeCount = 1;
        private List<int> _bulkUpgradeCountList;

        public void Init(Brain brain, BrainNetwork brainNetwork)
        {
            base.Init();
            Set(brain, brainNetwork);
        }
        public void Set(Brain brain, BrainNetwork brainNetwork)
        {
            base.Set();

            _bulkUpgradeCountList = Managers.Definition.GetData<List<int>>(DefinitionKey.bulkUpgradeCountList);
            _bulkUpgradeCount = _bulkUpgradeCountList[_bulkUpgradeCountIndex];
            _bulkUpgradeCountText.text = Managers.Definition.GetTextFormatData(7, _bulkUpgradeCount.ToString("N0"));

            // 값 표시하는 부분 제외한 모든 텍스트 영역 표시
            _popupTitle.text = Managers.Definition.GetTextData(12001);
            _idKey.text = Managers.Definition.GetTextData(12002);
            _typeKey.text = Managers.Definition.GetTextData(12003);
            _intellectKey.text = Managers.Definition.GetTextData(12006);
            _intellectLimitKey.text = Managers.Definition.GetTextData(12007);
            _multiplierKey.text = Managers.Definition.GetTextData(12008);
            _npKey.text = Managers.Definition.GetTextData(12009);
            _distanceKey.text = Managers.Definition.GetTextData(12010);
            _statusKey.text = Managers.Definition.GetTextData(12011);
            _resetButtonText.text = Managers.Definition.GetTextData(12017);

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
            _intellectValue.text = _brain.BrainData.Intellect.ToString(ECurrencyType.INTELLECT);

            _inputMap.Clear();
            _inputMap.Add("intellect", _brain.BrainData.Intellect);
            _inputMap.Add("tpu008", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
            UpArrowNotation storedNP = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainDecomposingGainEquation));

            if (_brain.BrainData.brainType == EBrainType.NORMALBRAIN)
            {
                // stored NP
                _npValue.text = storedNP.ToString(ECurrencyType.NP) + " NP";

                UpArrowNotation totalSenderNP = new UpArrowNotation(0);
                foreach (Brain brain in _deletableSenderList)
                {
                    _inputMap.Clear();
                    _inputMap.Add("intellect", brain.Intellect);
                    _inputMap.Add("tpu008", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
                    totalSenderNP += Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainDecomposingGainEquation));
                }
                _decomposeReward.text = _deletableSenderList.Count == 0 ?
                    Managers.Definition.GetTextFormatData(5, storedNP.ToString(ECurrencyType.NP)) :
                    Managers.Definition.GetTextFormatData(6, storedNP.ToString(ECurrencyType.NP), totalSenderNP.ToString(ECurrencyType.NP));   // "총" 획득 NP량 계산해서 표시
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
            _idValue.text = _brain.BrainData.id.ToString();

            // brain type
            switch (_brain.BrainData.brainType)
            {
                case EBrainType.COREBRAIN:
                    _typeValue.text = Managers.Definition.GetTextData(12004);
                    break;
                case EBrainType.NORMALBRAIN:
                    _typeValue.text = Managers.Definition.GetTextData(12005);
                    break;
            }

            // current intellect limit
            _intellectLimitValue.text = _brain.CurrentIntellectLimit.ToString(ECurrencyType.INTELLECT);

            // current multiplier
            _multiplierValue.text = "x" + _brain.Multiplier.ToString(ECurrencyType.MULTIPLIER);

            // current distance
            _distanceValue.text = _brain.BrainData.distance.ToString();

            //TODO: 나중에 text 색 모두 rich text로 변경하셔야 해요.
            // current status
            if (_brain.ReceiverIdList.Count == 0)
            {
                _statusValue.text = Managers.Definition.GetTextData(12013);
                _statusValue.color = new Color32(255, 0, 0, 255);
            }
            else if (_brain.Intellect.Top3Layer[0] == 0f)
            {
                _statusValue.text = Managers.Definition.GetTextData(12014);
                _statusValue.color = new Color32(255, 255, 255, 255);
            }
            else if (_brain.IsLocked)
            {
                _statusValue.text = Managers.Definition.GetTextData(12016);
                _statusValue.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                _statusValue.text = Managers.Definition.GetTextData(12015);
                _statusValue.color = new Color32(255, 255, 255, 255);
            }

            // multiplier upgrade btn text
            string multiplierUpgradeText;
            if (_brain.BrainData.brainType == EBrainType.NORMALBRAIN)
            {
                if (UserData.TPUpgrades[0].UpgradeCount > 0)
                {
                    multiplierUpgradeText = _brain.SenderIdList.Count == 0 ?
                        Managers.Definition.GetTextData(12012) :
                        Managers.Definition.GetTextFormatData(2, (UserData.TPUpgrades[1].UpgradeCount + 2).ToString());
                }
                else
                {
                    multiplierUpgradeText = Managers.Definition.GetTextData(12012);
                }
                _upgradeMultiplierCost.text = multiplierUpgradeText + "\n" + Managers.Definition.GetTextFormatData(1, _brain.GetBrainUpgradeCost(_bulkUpgradeCount).ToString(ECurrencyType.NP));
            }
            else if (UserData.TPUpgrades[0].UpgradeCount > 0)       // 코어 브레인에서는 일괄업글 버튼 표시
            {
                multiplierUpgradeText = Managers.Definition.GetTextFormatData(3, (UserData.TPUpgrades[1].UpgradeCount + 2).ToString());
                _upgradeMultiplierCost.text = multiplierUpgradeText + "\n" + Managers.Definition.GetTextFormatData(1, GetTotalUpgradeCost().ToString(ECurrencyType.NP));
            }

            // limit upgrade btn text
            _inputMap.Clear();
            _inputMap.Add("distance", new UpArrowNotation(_brain.Distance));
            _inputMap.Add("upgradeCount", new UpArrowNotation(_brain.BrainData.limitUpgradeCount));
            _inputMap.Add("tpu002", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));
            UpArrowNotation limitUpgradeCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainLimitUpgradeCostEquation));

            string limitUpgradeText = Managers.Definition.GetTextFormatData(4, _brain.GetNextIntellectLimit().ToString(ECurrencyType.INTELLECT));
            _upgradeLimitCost.text = limitUpgradeText + "\n" + Managers.Definition.GetTextFormatData(1, limitUpgradeCost.ToString(ECurrencyType.NP));
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
                _sendData.Add(EDataParamKey.BULK_UPGRADE_COUNT, (long)_bulkUpgradeCount);
                Managers.Notification.PostNotification(ENotiMessage.ONCLICK_UPGRADE_BRAIN_MULTIPLIER, _sendData);
                UpdateInfo();
            }
            else
            {
                Debug.LogError("브레인 데이터 Null");
            }
        }

        public void OnClick_DecreaseUpgradeCount()
        {
            if (_bulkUpgradeCountIndex > 0)
            {
                _bulkUpgradeCountIndex--;
                _bulkUpgradeCount = _bulkUpgradeCountList[_bulkUpgradeCountIndex];
                _bulkUpgradeCountText.text = Managers.Definition.GetTextFormatData(7, _bulkUpgradeCount.ToString("N0"));
                UpdateInfo();
            }
        }

        public void OnClick_IncreaseUpgradeCount()
        {
            if (_bulkUpgradeCountIndex < _bulkUpgradeCountList.Count - 1)
            {
                _bulkUpgradeCountIndex++;
                _bulkUpgradeCount = _bulkUpgradeCountList[_bulkUpgradeCountIndex];
                _bulkUpgradeCountText.text = Managers.Definition.GetTextFormatData(7, _bulkUpgradeCount.ToString("N0"));
                UpdateInfo();
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

            // 코어 브레인은 업그레이드 및 분해 등의 동작이 필요하지 않으므로 버튼 비활성화 (일괄 업글할때 제외)
            _decomposeBtn.gameObject.SetActive(isNormalBrain);
            _upgradeMultiplierBtn.gameObject.SetActive(isNormalBrain || UserData.TPUpgrades[0].UpgradeCount > 0);
            _upgradeLimitBtn.gameObject.SetActive(isNormalBrain);
            _bulkUpgradeArea.gameObject.SetActive(isNormalBrain || UserData.TPUpgrades[0].UpgradeCount > 0);

            // 코어 브레인은 증폭계수, NP축적량, 거리, 상태 개념이 필요하지 않으므로 표시영역 모두 비활성화
            _intellectLimitArea.SetActive(isNormalBrain);
            _multiplierArea.SetActive(isNormalBrain && UserData.TPUpgrades[0].UpgradeCount > 0);    // 증폭계수 언락 전까지는 표시영역 비활성화
            _storedNPArea.SetActive(isNormalBrain);
            _distanceArea.SetActive(isNormalBrain && _brain.ReceiverIdList.Count > 0);              // 네트워크에 연결하기 전까지는 표시영역 비활성화
            _statusArea.SetActive(isNormalBrain);
        }

        private UpArrowNotation GetTotalUpgradeCost()
        {
            UpArrowNotation totalCost = new UpArrowNotation();
            foreach (Brain brain in _brain.BrainNetwork.Values)
            {
                if (brain.SenderIdList.Count > 0 && brain.ReceiverIdList.Count > 0)
                {
                    totalCost += brain.GetBrainUpgradeCost(_bulkUpgradeCount);
                }
            }
            return totalCost;
        }
    }
}
