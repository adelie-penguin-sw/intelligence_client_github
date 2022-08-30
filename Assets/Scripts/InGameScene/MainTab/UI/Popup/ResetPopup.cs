using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class ResetPopup : PopupBase
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

        public async void OnClick_Reset()
        {
            // UserData.TP += _brainNetwork.RemoveBrain(brain);
            var res = await NetworkManager.Instance.API_NetworkReset();
            if (res != null)
            {
                Hashtable sendData = new Hashtable();
                sendData.Add(EDataParamKey.SINGLE_NETWORK_WRAPPER, new SingleNetworkWrapper(res));
                NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_RESET_NETWORK, sendData);
                Dispose();
            }
        }

    }
}