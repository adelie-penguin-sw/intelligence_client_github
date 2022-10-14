﻿using System.Collections;
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

        [SerializeField] private Brain _brain;

        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _intellectText;
        [SerializeField] private TextMeshProUGUI _intellectLimitText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _npText;
        [SerializeField] private TextMeshProUGUI _distanceText;
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
            //bfs or dfs 사용해서 쭉쭉 파고들어가서 연결된 모든 브레인들의 braindata 가져온 후에 보여주면 되지 않을까..? 아마도,,?
            //근데 큰 문제는 senderidList가 지금 뭘 넣어주고있는게 없어서 가져올 수 있는게 없어요. --> 일단 대충 구현은 했슴당
            //복잡하게 구현하면 가져올 수 있을 것 같긴한데 금욜날 대면으로 회의하면서 어떻게 받아올지 논의 하면 좋을듯??
            //서버에서 reverse structure비슷하게 만들어서 보내주면 엄청 쉬워지긴한데 지금 서버 짜여져있는 코드 보고 결정해야할듯

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
            _intellectText.text = _brain.BrainData.Intellect.ToString();

            if (_brain.BrainData.brainType == EBrainType.NORMALBRAIN)
            {
                // stored NP
                _npText.text = storedNP.ToString();

                UpArrowNotation totalSenderNP = new UpArrowNotation(0);
                foreach (Brain brain in _deletableSenderList)
                {
                    //totalSenderNP += Exchange.GetNPRewardForBrainDecomposition(brain.Intellect);
                    _inputMap.Clear();
                    _inputMap.Add("intellect", brain.Intellect);
                    _inputMap.Add("tpu008", new UpArrowNotation(UserData.TPUpgrades[8].UpgradeCount));
                    totalSenderNP += Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainDecomposingGainEquation));
                }
                _decomposeReward.text = _brain.SenderIdList.Count == 0 ?
                    string.Format("Decompose\nfor {0} NP\n", storedNP) :
                    string.Format("Decompose\nfor {0} NP\n+ {1} NP", storedNP, totalSenderNP);   // "총" 획득 NP량 계산해서 표시
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
            _intellectLimitText.text = _brain.CurrentIntellectLimit.ToString();

            // current multiplier
            _multiplierText.text = "x" + _brain.Multiplier.ToString();

            // current distance
            _distanceText.text = _brain.BrainData.distance.ToString();

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
            _upgradeMultiplierCost.text = string.Format(multiplierUpgradeText + "\nCost: {0} NP", multiplierUpgradeCost);

            // limit upgrade btn text
            _inputMap.Clear();
            _inputMap.Add("distance", new UpArrowNotation(_brain.Distance));
            _inputMap.Add("upgradeCount", new UpArrowNotation(_brain.BrainData.limitUpgradeCount));
            _inputMap.Add("tpu002", new UpArrowNotation(UserData.TPUpgrades[2].UpgradeCount));
            UpArrowNotation limitUpgradeCost = Managers.Definition.CalcEquation(_inputMap, Managers.Definition.GetData<string>(DefinitionKey.brainLimitUpgradeCostEquation));
            string limitUpgradeText = $"Break Limit to {_brain.GetNextIntellectLimit()}";
            _upgradeLimitCost.text = string.Format(limitUpgradeText + "\nCost: {0} NP", limitUpgradeCost);
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

            // 코어 브레인은 증폭계수, NP축적량, 거리 개념이 필요하지 않으므로 표시영역 모두 비활성화
            _intellectLimitArea.SetActive(isNormalBrain);
            _multiplierArea.SetActive(isNormalBrain);
            _storedNPArea.SetActive(isNormalBrain);
            _distanceArea.SetActive(isNormalBrain);
        }
    }
}
