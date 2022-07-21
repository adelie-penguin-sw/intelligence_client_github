using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainTab;
namespace InGame
{
    /// <summary>
    /// �극���� ������ ��Ÿ���� �˾� Ŭ����<br />
    /// �극�� �Ǹ� ����<br />
    /// </summary>
    public class BrainInfoPopup : PopupBase
    {
        [SerializeField] private Button _sellBtn;
        [SerializeField] private Brain _brain;
        public void Init(Brain brain)
        {
            _brain = brain;
        }

        private Hashtable _sendData = new Hashtable();
        public void OnClick_SellBrain()
        {
            _sendData.Clear();
            _sendData.Add(EDataParamKey.CLASS_BRAIN, _brain);
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_SELL_BRAIN, _sendData);
        }
    }
}
