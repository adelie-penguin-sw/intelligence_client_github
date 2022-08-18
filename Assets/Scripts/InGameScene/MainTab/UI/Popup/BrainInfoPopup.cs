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
        [SerializeField] private BrainData _brainData;
        [SerializeField] private TextMeshProUGUI _idText;
        [SerializeField] private TextMeshProUGUI _typeText;
        [SerializeField] private TextMeshProUGUI _intellectText;
        [SerializeField] private TextMeshProUGUI _multiplierText;
        [SerializeField] private TextMeshProUGUI _npText;
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _upgradeCost;
        [SerializeField] private TextMeshProUGUI _decomposeReward;
        public void Init(BrainData brain)
        {
            base.Init();
            Set(brain);
        }
        public void Set(BrainData brain)
        {
            base.Set();
            _brainData = brain;

            // 코어 브레인은 업그레이드 및 분해 등의 동작이 필요하지 않으므로 버튼 비활성화
            _sellBtn.gameObject.SetActive(_brainData.brainType == EBrainType.NORMALBRAIN);
            _upgradeBtn.gameObject.SetActive(_brainData.brainType == EBrainType.NORMALBRAIN);
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
            _multiplierText.text = "x" + _brainData.multiplier.ToString();
            _npText.text = storedNP.ToString();
            _distanceText.text = _brainData.distance.ToString();
            
            _upgradeCost.text = string.Format("Upgrade\nCost: {0} NP", 1);              // 업그레이드 비용 계산해서 표시
            _decomposeReward.text = string.Format("Decompose\nfor {0} NP", storedNP);   // "총" 획득 NP량 계산해서 표시
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
