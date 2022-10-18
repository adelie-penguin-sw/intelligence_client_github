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
            _bulkUpgradeCountText.text = Managers.Definition.GetUIText(UITextKey.brainInfoBulkUpgradeText, _bulkUpgradeCount.ToString("N0"));

            // 값 표시하는 부분 제외한 모든 텍스트 영역 표시
            _popupTitle.text = Managers.Definition.GetUIText(UITextKey.brainInfoPopupTitleText);
            _idKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoIDTitle);
            _typeKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoTypeTitle);
            _intellectKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoIntellectTitle);
            _intellectLimitKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoIntellectLimitTitle);
            _multiplierKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoMultiplierTitle);
            _npKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoStoredNPTitle);
            _distanceKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoDistanceTitle);
            _statusKey.text = Managers.Definition.GetUIText(UITextKey.brainInfoStatusTitle);
            _resetButtonText.text = Managers.Definition.GetUIText(UITextKey.brainInfoResetButtonText);

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
                    Managers.Definition.GetUIText(UITextKey.brainInfoDecomposeButtonTextNoSender, storedNP.ToString(ECurrencyType.NP)) :
                    Managers.Definition.GetUIText(UITextKey.brainInfoDecomposeButtonTextHasSender, storedNP.ToString(ECurrencyType.NP), totalSenderNP.ToString(ECurrencyType.NP));   // "총" 획득 NP량 계산해서 표시
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
                    _typeValue.text = Managers.Definition.GetUIText(UITextKey.brainInfoTypeCoreBrain);
                    break;
                case EBrainType.NORMALBRAIN:
                    _typeValue.text = Managers.Definition.GetUIText(UITextKey.brainInfoTypeNormalBrain);
                    break;
            }

            // current intellect limit
            _intellectLimitValue.text = _brain.CurrentIntellectLimit.ToString(ECurrencyType.INTELLECT);

            // current multiplier
            _multiplierValue.text = "x" + _brain.Multiplier.ToString(ECurrencyType.MULTIPLIER);

            // current distance
            _distanceValue.text = _brain.BrainData.distance.ToString();

            // current status
            if (_brain.ReceiverIdList.Count == 0)
            {
                _statusValue.text = Managers.Definition.GetUIText(UITextKey.brainInfoStatusNotConnected);
                _statusValue.color = new Color32(255, 0, 0, 255);
            }
            else if (_brain.Intellect.Top3Layer[0] == 0f)
            {
                _statusValue.text = Managers.Definition.GetUIText(UITextKey.brainInfoStatusIdle);
                _statusValue.color = new Color32(255, 255, 255, 255);
            }
            else if (_brain.IsLocked)
            {
                _statusValue.text = Managers.Definition.GetUIText(UITextKey.brainInfoStatusLimitExceeded);
                _statusValue.color = new Color32(255, 0, 0, 255);
            }
            else
            {
                _statusValue.text = Managers.Definition.GetUIText(UITextKey.brainInfoStatusWorking);
                _statusValue.color = new Color32(255, 255, 255, 255);
            }

            // multiplier upgrade btn text
            string multiplierUpgradeText;
            if (UserData.TPUpgrades[0].UpgradeCount > 0)
            {
                multiplierUpgradeText = _brain.SenderIdList.Count == 0 ?
                    Managers.Definition.GetUIText(UITextKey.brainInfoUpgradeIntellectButtonText) :
                    Managers.Definition.GetUIText(UITextKey.brainInfoUpgradeMultiplierButtonText, (UserData.TPUpgrades[1].UpgradeCount + 2).ToString());
            }
            else
            {
                multiplierUpgradeText = Managers.Definition.GetUIText(UITextKey.brainInfoUpgradeIntellectButtonText);
            }
            _upgradeMultiplierCost.text = multiplierUpgradeText + "\n" + Managers.Definition.GetUIText(UITextKey.costText, GetTotalUpgradeCost().ToString(ECurrencyType.NP));

            // limit upgrade btn text
            _inputMap.Clear();
            _inputMap.Add("distance", new UpArrowNotation(_brain.Distance));
            _inputMap.Add("upgradeCount", new UpArrowNotation(_brain.BrainData.limitUpgradeCount));
            _inputMap.Add("tpu002", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));
            UpArrowNotation limitUpgradeCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainLimitUpgradeCostEquation));

            string limitUpgradeText = Managers.Definition.GetUIText(UITextKey.brainInfoUpgradeLimitButtonText, _brain.GetNextIntellectLimit().ToString(ECurrencyType.INTELLECT));
            _upgradeLimitCost.text = limitUpgradeText + "\n" + Managers.Definition.GetUIText(UITextKey.costText, limitUpgradeCost.ToString(ECurrencyType.NP));
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
                _bulkUpgradeCountText.text = Managers.Definition.GetUIText(UITextKey.brainInfoBulkUpgradeText, _bulkUpgradeCount.ToString("N0"));
                UpdateInfo();
            }
        }

        public void OnClick_IncreaseUpgradeCount()
        {
            if (_bulkUpgradeCountIndex < _bulkUpgradeCountList.Count - 1)
            {
                _bulkUpgradeCountIndex++;
                _bulkUpgradeCount = _bulkUpgradeCountList[_bulkUpgradeCountIndex];
                _bulkUpgradeCountText.text = Managers.Definition.GetUIText(UITextKey.brainInfoBulkUpgradeText, _bulkUpgradeCount.ToString("N0"));
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

            // 코어 브레인은 업그레이드 및 분해 등의 동작이 필요하지 않으므로 버튼 비활성화
            _decomposeBtn.gameObject.SetActive(isNormalBrain);
            _upgradeMultiplierBtn.gameObject.SetActive(isNormalBrain);
            _upgradeLimitBtn.gameObject.SetActive(isNormalBrain);
            _bulkUpgradeArea.gameObject.SetActive(isNormalBrain);

            // 코어 브레인은 증폭계수, NP축적량, 거리, 상태 개념이 필요하지 않으므로 표시영역 모두 비활성화
            _intellectLimitArea.SetActive(isNormalBrain);
            _multiplierArea.SetActive(isNormalBrain && UserData.TPUpgrades[0].UpgradeCount > 0);    // 증폭계수 언락 전까지는 표시영역 비활성화
            _storedNPArea.SetActive(isNormalBrain);
            _distanceArea.SetActive(isNormalBrain && _brain.ReceiverIdList.Count > 0);              // 네트워크에 연결하기 전까지는 표시영역 비활성화
            _statusArea.SetActive(isNormalBrain);
        }

        private UpArrowNotation GetTotalUpgradeCost()
        {
            float growthRate = Managers.Definition.GetData<float>(DefinitionKey.multiplierUpgradeCostGrowthRate);

            _inputMap.Clear();
            _inputMap.Add("growthRate", new UpArrowNotation(growthRate));
            _inputMap.Add("upgradeCount", new UpArrowNotation(_brain.BrainData.multiplierUpgradeCount));
            _inputMap.Add("tpu012", new UpArrowNotation(UserData.TPUpgrades[12].UpgradeCount));
            _inputMap.Add("tpu022", new UpArrowNotation(UserData.TPUpgrades[22].UpgradeCount));
            UpArrowNotation multiplierUpgradeCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainMultiplierUpgradeCostEquation));

            if (_brain.SenderIdList.Count == 0 || UserData.TPUpgrades[0].UpgradeCount == 0)
            {
                return multiplierUpgradeCost * _bulkUpgradeCount;
            }
            else
            {
                return multiplierUpgradeCost * ((Mathf.Pow(growthRate, (float)_bulkUpgradeCount) - 1) / (growthRate - 1));
            }
        }
    }
}
