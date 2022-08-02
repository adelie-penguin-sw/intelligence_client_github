using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class CompletePopup : PopupBase
    {
        public override void Init()
        {
            base.Init();
        }

        private Hashtable _sendData = new Hashtable();
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }
        public override void Dispose()
        {
            base.Dispose();
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_RESET_POPUP);
        }

        public void OnClick_Reset()
        {
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_RESET_NETWORK);
            Dispose();
        }

    }
}