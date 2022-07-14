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

        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CREATE_BRAIN);
        }
        public override void Set()
        {

        }

        public override void AdvanceTime(float dt_sec)
        {
            foreach (var reservationB in _reservationBrains)
            {
                _brains.Add(reservationB);
            }
            _reservationBrains.Clear();

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
        }

        public void ReservationAddBrain(Vector2 pos)
        {
            GameObject go = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", _app.MainTabView.transform);
            go.transform.position = pos;
            Brain brain = go.GetComponent<Brain>();
            brain.Init(EBrainType.NORMALBRAIN);
            _reservationBrains.Add(brain);
        }

        private void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.CREATE_BRAIN:
                    Vector2 brainPos = (Vector2)noti.data[EDataParamKey.VECTOR2];
                    ReservationAddBrain(brainPos);
                    break;
            }
        }
    }
}