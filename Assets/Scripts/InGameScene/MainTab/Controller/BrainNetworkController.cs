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
        private static int _tempBrainID = 2; //임시 브레인 아이디 생성용
        private BrainNetwork _brainNetwork;
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ONCLICK_SELL_BRAIN);
        }
        public override void Set()
        {
            _brainNetwork = _app.MainTabModel.BrainNetwork;
            _brainNetwork.Init(_app.MainTabView.transform);
        }

        public override void AdvanceTime(float dt_sec)
        {
            if (_brainNetwork != null)
            {
                _brainNetwork.AdvanceTime(dt_sec);
            }
        }

        public override void Dispose()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_SELL_BRAIN);

            _brainNetwork.Dispose();
            _brainNetwork = null;
            _app = null;
        }

        private void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.CREATE_BRAIN:
                    Vector2 brainPos = (Vector2)noti.data[EDataParamKey.VECTOR2];
                    AddBrain(brainPos);
                    break;
                case ENotiMessage.CREATE_CHANNEL:
                    Channel channel = (Channel)noti.data[EDataParamKey.CLASS_CHANNEL];
                    //if (channel != null)
                    //{
                    //    ReservationAddChannel(channel);
                    //}
                    break;
                case ENotiMessage.ONCLICK_SELL_BRAIN:
                    Brain sellBrain = (Brain)noti.data[EDataParamKey.CLASS_BRAIN];
                    break;
            }
        }

        /// <summary>
        /// BrainNetwork에 좌표에 해당하는 브레인을 생성시킨 후 추가시켜줌
        /// </summary>
        /// <param name="pos">생성될 브레인의 position</param>
        private void AddBrain(Vector2 pos)
        {
            GameObject go = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _app.MainTabView.transform);
            go.transform.position = pos;

            Brain brain = go.GetComponent<Brain>();
            brain.Init(new BrainData(_tempBrainID++, EBrainType.NORMALBRAIN));

            _app.MainTabModel.BrainNetwork.AddBrain(brain);
        }

        /// <summary>
        /// Channel 생성 예약 메서드
        /// </summary>
        /// <param name="channel">생성될 채널의 class</param>
        private void ReservationAddChannel(Channel channel)
        {
            //_reservationChannels.Add(channel);
        }
    }

}

