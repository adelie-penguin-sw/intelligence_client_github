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
        [SerializeField]
        private List<Brain> _brains;
        [SerializeField]
        private List<Channel> _channels;
        [SerializeField]
        private List<Brain> _reservationBrains;
        [SerializeField]
        private List<Channel> _reservationChannel;

        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
        }
        public override void Set()
        {
            _dtElapse = 0f;
        }

        public override void AdvanceTime(float dt_sec)
        {
            UpdateIntellect(dt_sec);
            Reservations(dt_sec);

            UpdateBrain(dt_sec);
            UpdateChannel(dt_sec);
        }

        public override void Dispose()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
        }

        /// <summary>
        /// Brain 생성 예약 메서드
        /// </summary>
        /// <param name="pos">생성될 브레인의 position</param>
        public void ReservationAddBrain(Vector2 pos)
        {
            GameObject go = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _app.MainTabView.transform);
            go.transform.position = pos;
            Brain brain = go.GetComponent<Brain>();
            brain.Init(EBrainType.NORMALBRAIN);
            _reservationBrains.Add(brain);
        }

        /// <summary>
        /// Channel 생성 예약 메서드
        /// </summary>
        /// <param name="channel">생성될 채널의 class</param>
        public void ReservationAddChannel(Channel channel)
        {
            _reservationChannel.Add(channel);
        }

        private void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.CREATE_BRAIN:
                    Vector2 brainPos = (Vector2)noti.data[EDataParamKey.VECTOR2];
                    ReservationAddBrain(brainPos);
                    break;
                case ENotiMessage.CREATE_CHANNEL:
                    Channel channel = (Channel)noti.data[EDataParamKey.CLASS_CHANNEL];
                    if (channel != null)
                    {
                        ReservationAddChannel(channel);
                    }
                    break;
            }
        }

        private float _dtElapse = 0f;
        private const float _updateCycle = 1f;
        private void UpdateIntellect(float dt_sec)
        {
            _dtElapse += dt_sec;
            if (_dtElapse >= _updateCycle)
            {
                _dtElapse = 0f;
                foreach (var channel in _channels)
                {
                    channel.ToBrain.StandByIntellect += channel.FromBrain.Intellect;
                }
            }
        }

        private void Reservations(float dt_sec)
        {

            foreach (var reservationB in _reservationBrains)
            {
                _brains.Add(reservationB);
            }
            _reservationBrains.Clear();

            foreach (var reservationC in _reservationChannel)
            {
                _channels.Add(reservationC);
            }
            _reservationChannel.Clear();
        }

        private void UpdateBrain(float dt_sec)
        {
            foreach (var brain in _brains)
            {
                brain.AdvanceTime(dt_sec);
            }
        }

        private void UpdateChannel(float dt_sec)
        {
            foreach (var channel in _channels)
            {
                channel.AdvanceTime(dt_sec);
            }
        }
    }
}