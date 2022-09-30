using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// 브레인 네트워크를 관리해주는 Controller<br />
    /// 브레인 네트워크를 구성하는 brain과 channel을 모두 가지고 있다.<br />
    /// 브레인간의 숫자전달 / 브레인,채널 생성과 같이 브레인네트워크와 관련된 행동을 조작해준다.<br />
    /// </summary>
    public class BrainNetworkController : BaseTabController<MainTabApplication>
    {
        private BrainNetwork _brainNetwork;
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_SELL_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_RESET_NETWORK);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.UPDATE_BRAIN_NETWORK);
        }

        public override void Set()
        {
            if (_app != null)
            {
                if (_app.MainTabModel != null)
                {
                    _brainNetwork = _app.MainTabModel.BrainNetwork;
                    _brainNetwork.Init(_app.MainTabView.transform);
                    _brainNetwork.Set();
                }
            }
        }

        public override void AdvanceTime(float dt_sec)
        {
            if (_brainNetwork != null)
            {
                if (!InGame.InGameManager.IsCompleteExp)
                {
                    _brainNetwork.AdvanceTime(dt_sec);
                }
            }
        }

        public override void Dispose()
        {
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_SELL_BRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_RESET_NETWORK);

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.UPDATE_BRAIN_NETWORK);

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.UPDATE_TP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.UPDATE_NP);

            _brainNetwork.Dispose();
            _brainNetwork = null;
            _app = null;
        }

        private void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.ONCLICK_SELL_BRAIN:
                    BrainData sellBrain = (BrainData)noti.data[EDataParamKey.CLASS_BRAIN];
                    RemoveBrain(sellBrain);
                    break;
                case ENotiMessage.ONCLICK_RESET_NETWORK:
                    ResetBrainNetWork();
                    break;
                case ENotiMessage.UPDATE_BRAIN_NETWORK:
                    _brainNetwork.UpdateBrainNetwork();
                    break;
            }
        }

        private async void RemoveBrain(BrainData data)
        {
            if(data.brainType == EBrainType.NORMALBRAIN)
            {
                if (await Managers.Network.API_DeleteBrain(data.id))
                {
                    ResetBrainNetWork();
                }
            }
        }

        private void ResetBrainNetWork()
        {
            _app.MainTabModel.BrainNetwork.Dispose();
            _app.MainTabModel.BrainNetwork = new BrainNetwork();

            Set();

            InGame.InGameManager.IsCompleteExp = false;
        }
    }
}

