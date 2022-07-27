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
        [SerializeField] private Brain _brain;
        [SerializeField] private TextMeshProUGUI _infoText;
        public void Init(Brain brain)
        {
            _brain = brain;

        }

        private Hashtable _sendData = new Hashtable();

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            _infoText.text =
                string.Format("Intelligence: {0}\n\nStore NP: {1}\n\nDistance: {2}"
                , _brain.Intellect, _brain.Intellect, _brain.Distance);
        }
        public override void Dispose()
        {
            base.Dispose();
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_BRAININFO_POPUP);
        }

        public void OnClick_SellBrain()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, _brain);
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_SELL_BRAIN, _sendData);
            Dispose();
        }

    }
}
