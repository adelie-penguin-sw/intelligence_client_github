using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
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

        private float _dtElapse = 0f;
        private const float _updateCycle = 1f;
        public override void AdvanceTime(float dt_sec)
        {
            _dtElapse += dt_sec;
            if(_dtElapse >= _updateCycle)
            {
                _dtElapse = 0f;
                foreach (var channel in _channels)
                {
                    channel.ToBrain.StandByIntellect += channel.FromBrain.Intellect;
                }
            }

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

            foreach (var brain in _brains)
            {
                brain.AdvanceTime(dt_sec);
            }

            foreach (var channel in _channels)
            {
                channel.AdvanceTime(dt_sec);
            }
        }

        public override void Dispose()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
        }

        public void ReservationAddBrain(Vector2 pos)
        {
            GameObject go = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _app.MainTabView.transform);
            go.transform.position = pos;
            Brain brain = go.GetComponent<Brain>();
            brain.Init(EBrainType.NORMALBRAIN);
            _reservationBrains.Add(brain);
        }
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
    }
}