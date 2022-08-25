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
        [SerializeField] private Button _upgradeBtn;
        [SerializeField] private GameObject _multiplierArea;
        [SerializeField] private GameObject _storedNPArea;
        [SerializeField] private GameObject _distanceArea;
        [SerializeField] private Brain _brain;
        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _intellectText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _npText;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _upgradeCost;
        [SerializeField] private TextMeshProUGUI _decomposeReward;

        private List<Brain> _deletableSenderList = new List<Brain>();

        public void Init(Brain brain, SingleNetworkWrapper networkWrapper, BrainNetwork brainNetwork)
        {
            base.Init();
            Set(brain, networkWrapper, brainNetwork);
        }
        public void Set(Brain brain, SingleNetworkWrapper networkWrapper, BrainNetwork brainNetwork)
        {
            base.Set();
            _brain = brain;
            _deletableSenderList.Clear();

            bool isNormalBrain = _brain.BrainData.brainType == EBrainType.NORMALBRAIN;

            // 코어 브레인은 업그레이드 및 분해 등의 동작이 필요하지 않으므로 버튼 비활성화
            _sellBtn.gameObject.SetActive(isNormalBrain);
            _upgradeBtn.gameObject.SetActive(isNormalBrain);

            // 코어 브레인은 증폭계수, NP축적량, 거리 개념이 필요하지 않으므로 표시영역 모두 비활성화
            _multiplierArea.SetActive(isNormalBrain);
            _storedNPArea.SetActive(isNormalBrain);
            _distanceArea.SetActive(isNormalBrain);

            // 해당 브레인을 삭제할 때 같이 삭제되는 모든 브레인들의 id리스트
            if (isNormalBrain)
            {
                List<long> deletableSenderIdList = networkWrapper.GetAllDeletableSenderIdListForID(_brain.BrainData.id);

                foreach (var deletableSenderID in deletableSenderIdList)
                {
                    Debug.Log(deletableSenderID);
                    _deletableSenderList.Add(brainNetwork.GetBrainForID(deletableSenderID));
                }
            }
            //bfs or dfs 사용해서 쭉쭉 파고들어가서 연결된 모든 브레인들의 braindata 가져온 후에 보여주면 되지 않을까..? 아마도,,?
            //근데 큰 문제는 senderidList가 지금 뭘 넣어주고있는게 없어서 가져올 수 있는게 없어요. --> 일단 대충 구현은 했슴당
            //복잡하게 구현하면 가져올 수 있을 것 같긴한데 금욜날 대면으로 회의하면서 어떻게 받아올지 논의 하면 좋을듯??
            //서버에서 reverse structure비슷하게 만들어서 보내주면 엄청 쉬워지긴한데 지금 서버 짜여져있는 코드 보고 결정해야할듯
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            UpArrowNotation storedNP = Exchange.GetNPRewardForBrainDecomposition(_brain.BrainData.Intellect);
            _idText.text = _brain.BrainData.id.ToString();
            switch (_brain.BrainData.brainType)
            {
                case EBrainType.MAINBRAIN:
                    _typeText.text = "Core Brain";
                    break;
                case EBrainType.NORMALBRAIN:
                    _typeText.text = "Normal Brain";
                    break;
                default:
                    _typeText.text = "Unknown";
                    break;
            }
            _intellectText.text = _brain.BrainData.Intellect.ToString();

            if (_brain.BrainData.brainType == EBrainType.NORMALBRAIN)
            {
                _multiplierText.text = "x" + _brain.BrainData.multiplier.ToString();
                _npText.text = storedNP.ToString();
                _distanceText.text = _brain.BrainData.distance.ToString();

                UpArrowNotation upgradeCost = new UpArrowNotation(10);
                upgradeCost *= Mathf.Pow(2.5f, (float)UpArrowNotation.Log10Top3Layer(_brain.BrainData.multiplier));

                _upgradeCost.text = string.Format("Upgrade\nCost: {0} NP", upgradeCost);

                UpArrowNotation totalSenderNP = new UpArrowNotation(0);
                foreach (Brain brain in _deletableSenderList)
                {
                    totalSenderNP += Exchange.GetNPRewardForBrainDecomposition(brain.Intellect);
                }
                _decomposeReward.text = string.Format("Decompose\nfor {0} NP", storedNP + totalSenderNP);   // "총" 획득 NP량 계산해서 표시
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_BRAININFO_POPUP);
        }

        public void OnClick_SellBrain()
        {
            Hashtable _sendData = new Hashtable();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, _brain.BrainData);
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_SELL_BRAIN, _sendData);
            Dispose();
        }

        public void OnClick_UpgradeBrain()
        {
            if (_brain.BrainData != null)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.BRAIN_ID, _brain.BrainData.id);
                NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_UPGRADE_BRAIN, _sendData);
            }
            else
            {
                Debug.LogError("브레인 데이터 Null");
            }
        }
    }
}
