using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class BehaviorController : BaseTabController<MainTabApplication>
    {
        private MainTabModel _model;
        private MainTabView _view;
        [SerializeField]
        private Brain _recentSelectBrain;
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            _model = app.MainTabModel;
            _view = app.MainTabView;

            InitHandlers();
            ChangeState(EBehaviorState.NONE);

            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.DRAG_END_CREATEBRAIN);

            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_DOWN_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_EXIT_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_UP_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_ENTER_BRAIN);
        }

        public override void Set()
        {
        }

        public override void AdvanceTime(float dt_sec)
        {
            //ZoomScreenMobile();

            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).AdvanceTime(dt_sec);
            }
        }

        public override void Dispose()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.DRAG_END_CREATEBRAIN);

            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_DOWN_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_EXIT_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_UP_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_ENTER_BRAIN);
        }

        private void OnNotification(Notification noti)
        {
            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).OnNotification(noti);
            }
        }

        #region StateHandler Function
        private Dictionary<EBehaviorState, IBehaviorStateModule> _handlers = new Dictionary<EBehaviorState, IBehaviorStateModule>();
        private EBehaviorState _currentState = EBehaviorState.UNKNOWN;
        private void InitHandlers()
        {
            _handlers.Clear();
            _handlers.Add(EBehaviorState.NONE, new StateHandlerNone());
            _handlers.Add(EBehaviorState.CREATE_BRAIN, new StateHandlerCreateBrain());
            _handlers.Add(EBehaviorState.CREATE_CHANNEL, new StateHandlerCreateChannel());

            foreach (EBehaviorState state in _handlers.Keys)
            {
                _handlers[state].Init(this);
            }
        }

        private void ChangeState(EBehaviorState nextState)
        {
            if (nextState != EBehaviorState.UNKNOWN && nextState != _currentState)
            {
                EBehaviorState prevState = _currentState;
                _currentState = nextState;
                IBehaviorStateModule leaveHandler = GetStateHandler(prevState);
                if (leaveHandler != null)
                {
                    leaveHandler.OnExit();
                }
                IBehaviorStateModule enterHandler = GetStateHandler(_currentState);
                if (enterHandler != null)
                {
                    enterHandler.OnEnter();
                }
            }
        }

        private IBehaviorStateModule GetStateHandler(EBehaviorState state)
        {
            if (_handlers.ContainsKey(state))
            {
                return _handlers[state];
            }
            return null;
        }
        #endregion

        #region StateHandler Class
        protected class StateHandlerNone : IBehaviorStateModule
        {
            private MainTabModel _model;
            private BehaviorController _controller;
            private bool _isBrainPointDown = false;
            private float _dtBrainPointDown = 0f;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
                _model = controller._model;
            }

            public void OnEnter()
            {
                _isBrainPointDown = false;
                _dtBrainPointDown = 0f;
            }

            public void AdvanceTime(float dt_sec)
            {
                MoveScreen();
                ZoomScreenPC();
                if(_isBrainPointDown)
                {
                    _dtBrainPointDown += dt_sec;
                    if(_dtBrainPointDown >= _model.WaitBrainClickTime)
                    {
                        _controller.ChangeState(EBehaviorState.CREATE_CHANNEL);
                    }
                }
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.DRAG_START_CREATEBRAIN:
                        _controller.ChangeState(EBehaviorState.CREATE_BRAIN);
                        break;
                    case ENotiMessage.MOUSE_DOWN_BRAIN:
                        _controller._recentSelectBrain = (Brain)noti.data[EDataParamKey.CLASS_BRAIN];
                        _isBrainPointDown = true;
                        break;
                    case ENotiMessage.MOUSE_UP_BRAIN:
                    case ENotiMessage.MOUSE_EXIT_BRAIN:
                        _dtBrainPointDown = 0;
                        _isBrainPointDown = false;
                        break;

                }
            }
            public void OnExit()
            {
            }

            public void Dispose()
            {
            }
            private void MoveScreen()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _model.PrevMousePos = Input.mousePosition;
                }

                if (Input.GetMouseButton(0))
                {
                    _model.CurMousePos = Input.mousePosition;

                    _model.MainCamera.transform.Translate(_model.CameraMoveDelta * _model.DragSpeed * _model.MainCamera.orthographicSize);

                    _model.PrevMousePos = _model.CurMousePos;
                }
            }

            private void ZoomScreenPC()
            {
                _model.CurCameraSize = Input.GetAxis("Mouse ScrollWheel");
            }

            private void ZoomScreenMobile()
            {
            }
        }

        protected class StateHandlerCreateBrain : IBehaviorStateModule
        {
            private BehaviorController _controller;
            private GameObject _goBrainTemp;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
                _goBrainTemp = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", controller._view.transform);
                _goBrainTemp.GetComponent<Brain>().Init(EBrainType.GUIDEBRAIN);
                _goBrainTemp.SetActive(false);
            }

            public void OnEnter()
            {
                Debug.Log("CreateBrain!");
                _goBrainTemp.SetActive(true);
            }

            private Vector2 _curPos;
            public void AdvanceTime(float dt_sec)
            {
                if (Input.GetMouseButton(0))
                {
                    _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _goBrainTemp.transform.position = _curPos;
                }
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.DRAG_END_CREATEBRAIN:
                        Hashtable sendData = new Hashtable();
                        sendData.Add(EDataParamKey.VECTOR2, (Vector2)_goBrainTemp.transform.position);
                        NotificationManager.Instance.PostNotification(ENotiMessage.CREATE_BRAIN, sendData);
                        _controller.ChangeState(EBehaviorState.NONE);
                        break;
                }
            }
            public void OnExit()
            {
                _goBrainTemp.SetActive(false);
            }

            public void Dispose()
            {
                PoolManager.Instance.DespawnObject(EPrefabsType.BRAIN, _goBrainTemp);
            }

        }

        protected class StateHandlerCreateChannel : IBehaviorStateModule
        {
            private BehaviorController _controller;
            private Channel _channel;
            private Brain _currentEnterBrain;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
            }

            public void OnEnter()
            {
                Debug.Log("CreateChannel!");
                CreateTempChannel();
            }

            private Vector2 _curPos;
            public void AdvanceTime(float dt_sec)
            {
                _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _channel.SetLineRenderToPos(_curPos);
                _channel.AdvanceTime(dt_sec);
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.MOUSE_UP_BRAIN:
                        CreateChannel();
                        _controller.ChangeState(EBehaviorState.NONE);
                        break;
                    case ENotiMessage.MOUSE_ENTER_BRAIN:
                        _currentEnterBrain = (Brain)noti.data[EDataParamKey.CLASS_BRAIN];
                        break;
                    case ENotiMessage.MOUSE_EXIT_BRAIN:
                        _currentEnterBrain = null;
                        break;

                }
            }
            public void OnExit()
            {
            }

            public void Dispose()
            {
            }

            private void CreateTempChannel()
            {
                _channel = PoolManager.Instance.GrabPrefabs(EPrefabsType.CHANNEL, "Channel", _controller._view.transform).GetComponent<Channel>();
                _channel.Set(CreateBrainSendData(-1, _controller._recentSelectBrain), CreateBrainSendData(-1, null));
            }

            private BrainSendData CreateBrainSendData(int id,Brain brain)
            {
                BrainSendData data;
                data.id = id;
                data.brain = brain;
                return data;
            }
            private void CreateChannel()
            {
                if (_currentEnterBrain != null && _channel.FromBrain != _currentEnterBrain)
                {
                    _channel.Set(CreateBrainSendData(-1, _channel.FromBrain), CreateBrainSendData(-1, _currentEnterBrain));
                    Hashtable sendData = new Hashtable();
                    sendData.Add(EDataParamKey.CLASS_CHANNEL, _channel);
                    NotificationManager.Instance.PostNotification(ENotiMessage.CREATE_CHANNEL, sendData);
                }
                else
                {
                    PoolManager.Instance.DespawnObject(EPrefabsType.CHANNEL, _channel.gameObject);
                }
            }
        }
        #endregion
    }

    public enum EBehaviorState
    {
        UNKNOWN,
        NONE,
        CREATE_BRAIN,
        CREATE_CHANNEL,
    }

    public interface IBehaviorStateModule
    {
        void Init(BehaviorController controller);
        void OnEnter();
        void AdvanceTime(float dt_sec);
        void OnNotification(Notification noti);
        void OnExit();
        void Dispose();
    }
}