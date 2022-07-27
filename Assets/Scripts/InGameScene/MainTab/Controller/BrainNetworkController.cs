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
        private List<Channel> _reservationChannels;
        [SerializeField]
        private List<Brain> _removeBrains;
        [SerializeField]
        private List<Channel> _removeChannels;

        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ONCLICK_SELL_BRAIN);
        }
        public override void Set()
        {
            _dtElapse = 0f;
        }

        public override void AdvanceTime(float dt_sec)
        {
            RemoveNetwork();
            UpdateIntellect(dt_sec);
            Reservations(dt_sec);

            UpdateBrain(dt_sec);
            UpdateChannel(dt_sec);
        }

        public override void Dispose()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CREATE_CHANNEL);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_SELL_BRAIN);
        }

        private bool _isRemoveCheckTick = false;
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
                case ENotiMessage.ONCLICK_SELL_BRAIN:
                    Brain sellBrain = (Brain)noti.data[EDataParamKey.CLASS_BRAIN];
                    AddRemoveListNetwork(sellBrain);
                    _isRemoveCheckTick = true;
                    break;
            }
        }

        private void AddRemoveListNetwork(Brain brain)
        {
            Network removeNetwork = GetConnectNetwork(brain);
            _removeBrains = removeNetwork.brains;
            _removeChannels = removeNetwork.channels;

            foreach(var sellBrain in removeNetwork.brains)
            {
                _app.MainTabModel.NP += (int)sellBrain.Intellect;
            }
            _app.MainTabView.UI.SetNPText(_app.MainTabModel.NP);
        }

        /// <summary>
        /// Brain 생성 예약 메서드
        /// </summary>
        /// <param name="pos">생성될 브레인의 position</param>
        private void ReservationAddBrain(Vector2 pos)
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
        private void ReservationAddChannel(Channel channel)
        {
            _reservationChannels.Add(channel);
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
                    if (channel.ReceiverBrain != null && channel.SenderBrain != null)
                    {
                        channel.ReceiverBrain.StandByIntellect += channel.SenderBrain.Intellect;
                    }
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

            foreach (var reservationC in _reservationChannels)
            {
                _channels.Add(reservationC);
            }
            _reservationChannels.Clear();
        }

        private void RemoveNetwork()
        {
            if (_isRemoveCheckTick)
            {
                _isRemoveCheckTick = false;
                foreach (var brain in _removeBrains)
                {
                    _brains.Remove(brain);
                    brain.Dispose();
                }

                foreach (var channel in _channels)
                {
                    if (_brains.Find(brain => brain.ID != channel.SenderBrain.ID))
                    {
                        _removeChannels.Add(channel);
                    }
                }

                foreach (var channel in _removeChannels)
                {
                    _channels.Remove(channel);
                    channel.Dispose();
                }
                _removeChannels.Clear();
                _removeBrains.Clear();
            }
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

        private List<Channel> GetConnectChannelList(Brain brain)
        {
            List<Channel> channels = new List<Channel>();
            foreach (var channel in _channels)
            {
                if (channel.ReceiverBrain == brain)
                    channels.Add(channel);
            }
            return channels;
        }

        private Network GetConnectNetwork(Brain brain)
        {
            bool[] visitTemp = new bool[_brains.Count + 20000];
            Queue<Brain> channelQueue = new Queue<Brain>();

            Network resultList;
            resultList.brains = new List<Brain>();
            resultList.channels = new List<Channel>();

            channelQueue.Enqueue(brain);
            visitTemp[brain.ID] = true;

            while (channelQueue.Count > 0)
            {
                Brain curBrain = channelQueue.Dequeue();
                resultList.brains.Add(curBrain);
                foreach (var channel in GetConnectChannelList(curBrain))
                {
                    resultList.channels.Add(channel);
                    Brain SenderBrain = channel.SenderBrain;
                    if (!visitTemp[SenderBrain.ID])
                    {
                        visitTemp[SenderBrain.ID] = true;
                        channelQueue.Enqueue(SenderBrain);
                    }
                }
            }
            Debug.LogError("==============================================");
            foreach (var a in resultList.brains)
            {
                Debug.LogError(a.ID);
            }
            foreach (var a in resultList.channels)
            {
                Debug.LogError(a.name);
            }
            Debug.LogError("==============================================");
            return resultList;
        }
    }

    public struct Network
    {
        public List<Brain> brains;
        public List<Channel> channels;
    }
}

