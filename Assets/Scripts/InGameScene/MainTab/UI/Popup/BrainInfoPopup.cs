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
        [SerializeField] private BrainData _brainData;
        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _intellectText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _npText;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _upgradeCost;
        [SerializeField] private TextMeshProUGUI _decomposeReward;
        public void Init(BrainData brain, SingleNetworkWrapper networkWrapper)
        {
            base.Init();
            Set(brain, networkWrapper);
            foreach (var id in _brainData._senderIdList)
            {
                BrainData data = networkWrapper.GetBrainDataForID(id);
                Debug.LogError(id);
            }
        }
        public void Set(BrainData brain, SingleNetworkWrapper networkWrapper)
        {
            base.Set();
            _brainData = brain;

            bool isCoreBrain = _brainData.brainType == EBrainType.NORMALBRAIN;

            // 코어 브레인은 업그레이드 및 분해 등의 동작이 필요하지 않으므로 버튼 비활성화
            _sellBtn.gameObject.SetActive(isCoreBrain);
            _upgradeBtn.gameObject.SetActive(isCoreBrain);

            // 코어 브레인은 증폭계수, NP축적량, 거리 개념이 필요하지 않으므로 표시영역 모두 비활성화
            _multiplierArea.SetActive(isCoreBrain);
            _storedNPArea.SetActive(isCoreBrain);
            _distanceArea.SetActive(isCoreBrain);

            //해당 브레인에게 보내고있는 senderIdList들
            // brain._senderIdList
            foreach (var id in _brainData._senderIdList)
            {
                BrainData data = networkWrapper.GetBrainDataForID(id);
                Debug.LogError(id);
            }
            //foreach (var id in _brainData._senderIdList)
            //{
            //    networkWrapper.GetBrainDataForID(id);
            //}
            //bfs or dfs 사용해서 쭉쭉 파고들어가서 연결된 모든 브레인들의 braindata 가져온 후에 보여주면 되지 않을까..? 아마도,,?
            //근데 큰 문제는 senderidList가 지금 뭘 넣어주고있는게 없어서 가져올 수 있는게 없어요.
            //복잡하게 구현하면 가져올 수 있을 것 같긴한데 금욜날 대면으로 회의하면서 어떻게 받아올지 논의 하면 좋을듯??
            //서버에서 reverse structure비슷하게 만들어서 보내주면 엄청 쉬워지긴한데 지금 서버 짜여져있는 코드 보고 결정해야할듯
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            UpArrowNotation storedNP = Exchange.GetNPRewardForBrainDecomposition(_brainData.Intellect);
            _idText.text = _brainData.id.ToString();
            switch (_brainData.brainType)
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
            _intellectText.text = _brainData.Intellect.ToString();

            if (_brainData.brainType == EBrainType.NORMALBRAIN)
            {
                _multiplierText.text = "x" + _brainData.multiplier.ToString();
                _npText.text = storedNP.ToString();
                _distanceText.text = _brainData.distance.ToString();

                UpArrowNotation upgradeCost = new UpArrowNotation(10);
                upgradeCost *= Mathf.Pow(2.5f, (float)UpArrowNotation.Log10Top3Layer(_brainData.multiplier));

                _upgradeCost.text = string.Format("Upgrade\nCost: {0} NP", upgradeCost);
                _decomposeReward.text = string.Format("Decompose\nfor {0} NP", storedNP);   // "총" 획득 NP량 계산해서 표시
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
            _sendData.Add(EDataParamKey.CLASS_BRAIN, _brainData);
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_SELL_BRAIN, _sendData);
            Dispose();
        }

        public void OnClick_UpgradeBrain()
        {
            if (_brainData != null)
            {
                Hashtable _sendData = new Hashtable();
                _sendData.Add(EDataParamKey.BRAIN_ID, _brainData.id);
                NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_UPGRADE_BRAIN, _sendData);
            }
            else
            {
                Debug.LogError("브레인 데이터 Null");
            }
        }

    }
}
