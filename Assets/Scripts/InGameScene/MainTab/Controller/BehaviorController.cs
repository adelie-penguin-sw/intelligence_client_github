using System.Collections;
using System.Collections.Generic;
using UltimateClean;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// 사용자 조작 관리 Controller
    /// </summary>
    public class BehaviorController : BaseTabController<MainTabApplication>
    {
        private MainTabModel _model;
        private MainTabView _view;
        [SerializeField] private Brain _recentSelectBrain;


        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            _model = app.MainTabModel;
            _view = app.MainTabView;

            _view.UI.Init();

            InitHandlers();
            ChangeState(EBehaviorState.NONE);
            AddObservers();
        }

        public override void Set()
        {
            if (_view != null)
            {
                if (_view.UI != null)
                {
                    _view.UI.Set();
                }
            }
        }

        public override void AdvanceTime(float dt_sec)
        {
            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).AdvanceTime(dt_sec);
            }

            if (_view != null)
            {
                if (_view.UI != null)
                {
                    _view.UI.AdvanceTime(dt_sec);
                }
            }
        }

        public override void Dispose()
        {
            RemoveObservers();
            _view.UI.Dispose();
        }

        private void OnNotification(Notification noti)
        {
            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).OnNotification(noti);
            }
        }

        private void AddObservers()
        {
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.DRAG_END_CREATEBRAIN);

            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_DOWN_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_EXIT_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_UP_BRAIN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.MOUSE_ENTER_BRAIN);

            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CLOSE_BRAININFO_POPUP);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.CLOSE_RESET_POPUP);

            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ONCLICK_UPGRADE_BRAIN);
        }
        private void RemoveObservers()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.DRAG_END_CREATEBRAIN);

            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_DOWN_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_EXIT_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_UP_BRAIN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.MOUSE_ENTER_BRAIN);

            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CLOSE_BRAININFO_POPUP);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.CLOSE_RESET_POPUP);

            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_UPGRADE_BRAIN);
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
            _handlers.Add(EBehaviorState.SHOW_POPUP, new StateHandlerShowPopup());

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
        /// <summary>
        /// 기본 State class
        /// </summary>
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

                if (InGame.InGameManager.IsCompleteExp)
                {
                    CompletePopup infoPopup = PopupManager.Instance.CreatePopup(EPrefabsType.POPUP, "CompletePopup")
                        .GetComponent<CompletePopup>();
                    infoPopup.Init();
                    _controller.ChangeState(EBehaviorState.SHOW_POPUP);
                }

                if (_isBrainPointDown)
                {
                    _dtBrainPointDown += dt_sec;
                    if (_dtBrainPointDown >= _model.WaitBrainClickTime)
                    {
                        _controller.ChangeState(EBehaviorState.CREATE_CHANNEL);
                    }
                }
            }

            public void OnNotification(Notification noti)
            {
                if (InGame.InGameManager.IsCompleteExp)
                    return;

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
                        if (_isBrainPointDown)
                        {
                            _controller._view.InfoPopup = PopupManager.Instance.CreatePopup(EPrefabsType.POPUP, "BrainInfoPopup")
                                .GetComponent<InGame.BrainInfoPopup>();
                            _controller._view.InfoPopup.Init(_controller._recentSelectBrain.BrainData, _model.SingleNetworkWrapper);
                            _controller.ChangeState(EBehaviorState.SHOW_POPUP);
                        }
                        else
                        {
                            _dtBrainPointDown = 0;
                            _isBrainPointDown = false;
                        }
                        break;
                    case ENotiMessage.MOUSE_EXIT_BRAIN:
                        _dtBrainPointDown = 0;
                        _isBrainPointDown = false;
                        break;

                }
            }

            public void OnExit()
            {
                _isBrainPointDown = false;
                _dtBrainPointDown = 0f;
            }

            public void Dispose()
            {
            }

            /// <summary>
            /// 스크린 좌우상하 이동 메서드
            /// </summary>
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

            /// <summary>
            /// 스크린 줌 메서드 
            /// </summary>
            private void ZoomScreenPC()
            {
                _model.CurCameraSize = Input.GetAxis("Mouse ScrollWheel");
            }

            private void ZoomScreenMobile()
            {
            }
        }

        /// <summary>
        /// 브레인 생성 state class
        /// </summary>
        protected class StateHandlerCreateBrain : IBehaviorStateModule
        {
            private BehaviorController _controller;
            private Brain _tempBrain;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
                _tempBrain = PoolManager.Instance.GrabPrefabs(EPrefabsType.BRAIN, "Brain", controller._view.transform)
                            .GetComponent<Brain>();
                _tempBrain.Init(new BrainData(-1,EBrainType.GUIDEBRAIN));
            }

            public void OnEnter()
            {
                _tempBrain.gameObject.SetActive(true);
            }

            private Vector2 _curPos;
            public void AdvanceTime(float dt_sec)
            {
                if (Input.GetMouseButton(0))
                {
                    _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _tempBrain.transform.position = _curPos;
                }
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.DRAG_END_CREATEBRAIN:
                        CreateBrain();
                        _controller.ChangeState(EBehaviorState.NONE);
                        break;
                }
            }
            public void OnExit()
            {
                _tempBrain.gameObject.SetActive(false);
            }

            public void Dispose()
            {
                _tempBrain.Dispose();
            }

            private async void CreateBrain()
            {
                CreateSingleNetworkBrainRequest req = new CreateSingleNetworkBrainRequest();
                req.x = _tempBrain.transform.position.x;
                req.y = _tempBrain.transform.position.y;

                var res = await NetworkManager.Instance.API_CreateBrain(req);

                _controller._model.SingleNetworkWrapper.UpdateSingleNetworkData(req, res);
                NotificationManager.Instance.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);
            }
        }

        /// <summary>
        /// 채널 생성 State class
        /// </summary>
        protected class StateHandlerCreateChannel : IBehaviorStateModule
        {
            private BehaviorController _controller;
            private Channel _channel;
            private Brain _currentEnterBrain;
            private Brain _currentSenderBrain;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
            }

            public void OnEnter()
            {
                Debug.Log("CreateChannel!");
                _currentSenderBrain = _controller._recentSelectBrain;
                CreateTempChannel();
            }

            private Vector2 _curPos;
            public void AdvanceTime(float dt_sec)
            {
                _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _channel.SetLineRenderToPos(_curPos);
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.MOUSE_UP_BRAIN:
                        CreateChannel();
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
                _channel = null;
                _currentEnterBrain = null;
                _currentSenderBrain = null;
            }

            /// <summary>
            /// 임시채널 보여주기용 생성
            /// </summary>
            private void CreateTempChannel()
            {
                _channel = PoolManager.Instance.GrabPrefabs(EPrefabsType.CHANNEL, "Channel", _controller._view.transform).GetComponent<Channel>();
                _channel.Init(EChannelType.TEMP, _currentSenderBrain.transform, _currentSenderBrain.transform);
            }

            /// <summary>
            /// 남아있는 데이터가 다른 브레인과 연결하는것으로 판별나면 임시로 만들었던 채널 오브젝트를 실제 생성시키기 위해 Noti를 날려주고
            /// 아니면 Despawn 시키는 메서드
            /// </summary>
            private async void CreateChannel()
            {
                _channel.Dispose();
                if (_currentEnterBrain == null || _currentSenderBrain.Type == EBrainType.MAINBRAIN)
                {
                    _controller.ChangeState(EBehaviorState.NONE);
                    return;
                }

                if (_currentSenderBrain.ID != _currentEnterBrain.ID)
                {
                    CreateSingleNetworkChannelRequest req = new CreateSingleNetworkChannelRequest();
                    req.from = _currentSenderBrain.ID;
                    req.to = _currentEnterBrain.ID;

                    var res = await NetworkManager.Instance.API_CreateChannel(req);

                    switch((StatusCode)res.statusCode)
                    {
                        case StatusCode.SUCCESS:
                            _controller._model.SingleNetworkWrapper.UpdateSingleNetworkData(req, res, () =>
                            {
                                NotificationManager.Instance.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);
                                _controller.ChangeState(EBehaviorState.NONE);
                            });
                            break;
                        default:
                            _controller.ChangeState(EBehaviorState.NONE);
                            break;
                    }
                }
                else
                {
                    _controller.ChangeState(EBehaviorState.NONE);
                }
            }
        }

        /// <summary>
        /// popup state class
        /// </summary>
        protected class StateHandlerShowPopup : IBehaviorStateModule
        {
            private BehaviorController _controller;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
            }

            public void OnEnter()
            {
            }

            public void AdvanceTime(float dt_sec)
            {
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.CLOSE_BRAININFO_POPUP:
                    case ENotiMessage.CLOSE_RESET_POPUP:
                        _controller.ChangeState(EBehaviorState.NONE);
                        break;
                    case ENotiMessage.ONCLICK_UPGRADE_BRAIN:
                        long brainId = (long)noti.data[EDataParamKey.BRAIN_ID];
                        UpgradeBrain(brainId);
                        break;
                }
            }

            public void OnExit()
            {
            }

            public void Dispose()
            {
            }

            private async void UpgradeBrain(long id)
            {
                var req = new CreateSingleNetworkBrainNumberRequest();
                req.brain = id;
                CreateSingleNetworkBrainNumberResponse res = await NetworkManager.Instance.API_UpgradeBrain(req);

                // 브레인 업그레이드 상태가 바로 반영되도록 업데이트해주는 콜백 추가
                _controller._model.SingleNetworkWrapper.UpdateSingleNetworkData(res, () =>
                {
                    NotificationManager.Instance.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);

                    /* 문제점 : 
                     업글 가능한 브레인이 여러 개일 때 업글창 한 개 띄워놓고 업글버튼을 누를 때마다 실제 업글되는 브레인이 계속 바뀌는 문제 발생
                     */
                    SingleNetworkWrapper wrapper = _controller._model.SingleNetworkWrapper;
                    _controller._view.InfoPopup.Set(wrapper.GetBrainDataForID(id), wrapper);
                });
            }
        }
        #endregion
    }

    /// <summary>
    /// 사용자 행동 Enum
    /// </summary>
    public enum EBehaviorState
    {
        UNKNOWN,
        NONE,
        CREATE_BRAIN,
        CREATE_CHANNEL,
        SHOW_POPUP,
    }

    /// <summary>
    /// 사용자 행동 State 관리용 interface<br />
    /// BehaviorController Inner Class가 가지는 interface<br />
    /// </summary>
    public interface IBehaviorStateModule
    {
        /// <summary>
        /// state 최초 생성시 실행
        /// </summary>
        /// <param name="controller"></param>
        void Init(BehaviorController controller);
        /// <summary>
        /// 해당 조작상태 진입시 실행 
        /// </summary>
        void OnEnter();
        /// <summary>
        /// 매 틱마다 지속하여 실행
        /// </summary>
        /// <param name="dt_sec">DeltaTime</param>
        void AdvanceTime(float dt_sec);
        /// <summary>
        /// 옵저버 패턴에 delegate로 등록되어 있는 메서드
        /// </summary>
        /// <param name="noti">받는 noti</param>
        void OnNotification(Notification noti);
        /// <summary>
        /// 해당 조작상태 탈출시 실행
        /// </summary>
        void OnExit();
        /// <summary>
        /// 메모리 초기화용
        /// </summary>
        void Dispose();
    }
}