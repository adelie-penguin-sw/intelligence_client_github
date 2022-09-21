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

            InitHandlers();
            ChangeState(EBehaviorState.NONE);
            AddObservers();
        }

        public override void Set()
        {
        }

        public override void AdvanceTime(float dt_sec)
        {
            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).AdvanceTime(dt_sec);
            }

        }

        public override void LateAdvanceTime(float dt_sec)
        {
            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).LateAdvanceTime(dt_sec);
            }
        }

        public override void Dispose()
        {
            RemoveObservers();
        }

        private void OnNotification(Notification noti)
        {
            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).OnNotification(noti);
            }

            switch (noti.msg)
            {
                case ENotiMessage.ONCLICK_LEADERBOARD:
                    _view.LeaderboardPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "LeaderboardPopup", PopupType.NORMAL)
                                .GetComponent<InGame.LeaderboardPopup>();
                    if (_view.LeaderboardPopup != null)
                    {
                        _view.LeaderboardPopup.Init();
                    }
                    ChangeState(EBehaviorState.SHOW_POPUP);
                    break;
            }
        }

        private void AddObservers()
        {
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.DRAG_END_CREATEBRAIN);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.MOUSE_DOWN_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.MOUSE_EXIT_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.MOUSE_UP_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.MOUSE_ENTER_BRAIN);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.CLOSE_BRAININFO_POPUP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.CLOSE_RESET_POPUP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.CLOSE_LEADERBOARD_POPUP);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_UPGRADE_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_LEADERBOARD);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_RESET_BUTTON);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.EXPERIMENT_COMPLETE);
        }
        private void RemoveObservers()
        {
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.DRAG_END_CREATEBRAIN);

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.MOUSE_DOWN_BRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.MOUSE_EXIT_BRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.MOUSE_UP_BRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.MOUSE_ENTER_BRAIN);

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.CLOSE_BRAININFO_POPUP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.CLOSE_RESET_POPUP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.CLOSE_LEADERBOARD_POPUP);

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_UPGRADE_BRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_LEADERBOARD);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_RESET_BUTTON);

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.EXPERIMENT_COMPLETE);
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
            private bool _isTouchStartBrain = false;
            private float _dtBrainPointDown = 0f;
            private const float _limitTowTouch = 0.6f;
            public void Init(BehaviorController controller)
            {
                _controller = controller;
                _model = controller._model;
            }

            public void OnEnter()
            {
                _isBrainPointDown = false;
                _isTouchStartBrain = false;
                _dtBrainPointDown = 0f;
                _isTwoTouch = false;
                _dtCountTowTouch = 0;
                _isMoveStart = false;
            }

            public void AdvanceTime(float dt_sec)
            {
                if (InGame.InGameManager.IsCompleteExp)
                    return;

                if (_isTwoTouch)
                {
                    _dtCountTowTouch += dt_sec;
                    if(_dtCountTowTouch >= _limitTowTouch)
                    {
                        _isTwoTouch = false;
                        _dtCountTowTouch = 0;
                    }
                }
            }

            public void LateAdvanceTime(float dt_sec)
            {

                if (InGame.InGameManager.IsCompleteExp)
                    return;


                if (!_isTouchStartBrain)
                {
#if UNITY_EDITOR
                    BehaviorScreenPC();
#else
                    BehaviorScreenMobile();
#endif
                }
                else if (_isBrainPointDown)
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
                switch (noti.msg)
                {
                    case ENotiMessage.DRAG_START_CREATEBRAIN:
                        _controller.ChangeState(EBehaviorState.CREATE_BRAIN);
                        break;
                    case ENotiMessage.MOUSE_DOWN_BRAIN:
                        _controller._recentSelectBrain = (Brain)noti.data[EDataParamKey.CLASS_BRAIN];
                        _isBrainPointDown = true;
                        _isTouchStartBrain = true;
                        break;
                    case ENotiMessage.MOUSE_UP_BRAIN:
                        if (_isBrainPointDown)
                        {
                            _controller._view.InfoPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "BrainInfoPopup", PopupType.NORMAL)
                                .GetComponent<InGame.BrainInfoPopup>();
                            _controller._view.InfoPopup.Init(_controller._recentSelectBrain, _model.SingleNetworkWrapper, _model.BrainNetwork);
                            _controller.ChangeState(EBehaviorState.SHOW_POPUP);
                        }
                        else
                        {
                            _dtBrainPointDown = 0;
                            _isBrainPointDown = false;
                        }
                        _isTouchStartBrain = false;
                        break;

                    case ENotiMessage.MOUSE_EXIT_BRAIN:
                        _dtBrainPointDown = 0;
                        _isBrainPointDown = false;
                        break;

                    case ENotiMessage.EXPERIMENT_COMPLETE:
                    case ENotiMessage.ONCLICK_RESET_BUTTON:
                        Managers.Popup.DeleteAll(PopupType.NORMAL);  // 현재 떠있는 모든 팝업 닫음
                        ResetPopup infoPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "ResetPopup", PopupType.NORMAL)
                            .GetComponent<ResetPopup>();
                        infoPopup.Init();
                        _controller.ChangeState(EBehaviorState.SHOW_POPUP);
                        break;
                }
            }

            public void OnExit()
            {
                _isBrainPointDown = false;
                _isTouchStartBrain = false; 
                _isTwoTouch = false;
                _dtBrainPointDown = 0f;
                _isTwoTouch = false;
                _dtCountTowTouch = 0;
                _isMoveStart = false;
            }

            public void Dispose()
            {
            }

            Vector2 clickPoint;
            float dragSpeed = 30.0f;


            private float Speed = 1f;
            private Vector2 nowPos, prePos;
            private Vector3 movePos;
            private bool _isTwoTouch = false;
            private float _dtCountTowTouch = 0;
            /// <summary>
            /// PC 스크린 조작
            /// </summary>
            private void BehaviorScreenPC()
            {
                //Debug.LogError("PC");
                //줌인 줌아웃
                _model.CurCameraSize = Input.GetAxis("Mouse ScrollWheel");
                
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

            private float orthoZoomSpeed = 0.01f;      //줌인,줌아웃할때 속도(OrthoGraphic모드 용)
            private bool _isMoveStart = false;
            /// <summary>
            /// 모바일 스크린 조작
            /// </summary>
            private void BehaviorScreenMobile()
            {
                //Debug.LogError("Mobile");

                if (Input.touchCount == 1)
                {
                    if (!_isTwoTouch)
                    {
                        Touch touch = Input.GetTouch(0);
                        Debug.Log(touch.phase);
                        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
                        {
                            prePos = touch.position - touch.deltaPosition;
                            _isMoveStart = true;
                        }
                        else if (touch.phase == TouchPhase.Moved)
                        {
                            if (!_isMoveStart)
                            {
                                prePos = touch.position - touch.deltaPosition;
                                _isMoveStart = true;
                            }
                            else
                            {
                                nowPos = touch.position - touch.deltaPosition;
                                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                                Camera.main.transform.Translate(movePos);
                                prePos = touch.position - touch.deltaPosition;
                            }
                        }
                        else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            _isMoveStart = false;
                        }
                    }
                    else
                    {
                        prePos = new Vector2(0, 0);
                        nowPos = new Vector2(0, 0);
                        movePos = new Vector2(0, 0);
                    }
                }
                else if (Input.touchCount == 2) //손가락 2개가 눌렸을 때
                {
                    _isTwoTouch = true;
                    _dtCountTowTouch = 0;
                    Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치를 저장
                    Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치를 저장

                    //터치에 대한 이전 위치값을 각각 저장함
                    //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    // 각 프레임에서 터치 사이의 벡터 거리 구함
                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    // 만약 카메라가 OrthoGraphic모드 라면
                    _model.MainCamera.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;
                    _model.MainCamera.orthographicSize = Mathf.Min(20f, Mathf.Max(_model.MainCamera.orthographicSize, 5f));
                }

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
                _tempBrain = Managers.Pool.GrabPrefabs(EPrefabsType.BRAIN, "Brain", controller._view.transform)
                            .GetComponent<Brain>();
                _tempBrain.Init(new BrainData(-1,EBrainType.GUIDEBRAIN));
            }

            public void OnEnter()
            {
                _tempBrain.gameObject.SetActive(true);
                _controller._view.NPCostPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "NPCostPopup", PopupType.NORMAL)
                        .GetComponent<InGame.NPCostPopup>();
                _controller._view.NPCostPopup.Init(ENPCostType.BRAIN_GEN);
            }

            private Vector2 _curPos;
            public void AdvanceTime(float dt_sec)
            {
                if (Input.GetMouseButton(0))
                {
                    _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _tempBrain.transform.position = _curPos;
                    _controller._view.NPCostPopup.SetCollisoinState(_tempBrain.IsCollision);
                }
            }

            public void LateAdvanceTime(float dt_sec)
            {
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
                _controller._view.NPCostPopup.Dispose();
            }

            public void Dispose()
            {
                _tempBrain.Dispose();
            }

            private async void CreateBrain()
            {
                if (!_tempBrain.IsCollision)
                {
                    CreateSingleNetworkBrainRequest req = new CreateSingleNetworkBrainRequest();
                    req.x = _tempBrain.transform.position.x;
                    req.y = _tempBrain.transform.position.y;

                    var res = await Managers.Network.API_CreateBrain(req);
                    if (res != null)
                    {
                        _controller._model.SingleNetworkWrapper.UpdateSingleNetworkData(req, res);
                        Managers.Notification.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);
                    }
                }
                else
                {
                    Debug.LogError("다른 브레인과 충돌");
                }
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

                _controller._view.NPCostPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "NPCostPopup", PopupType.NORMAL)
                        .GetComponent<InGame.NPCostPopup>();
                _controller._view.NPCostPopup.Init(ENPCostType.CHNNL_GEN);
                _controller._view.NPCostPopup.SetBrain(_currentSenderBrain, null);

                CreateTempChannel();
            }

            private Vector2 _curPos;
            public void AdvanceTime(float dt_sec)
            {
                _curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _channel.SetLineRenderToPos(_curPos);
            }

            public void LateAdvanceTime(float dt_sec)
            {
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
                        _controller._view.NPCostPopup.SetBrain(_currentSenderBrain, _currentEnterBrain);
                        break;
                    case ENotiMessage.MOUSE_EXIT_BRAIN:
                        _currentEnterBrain = null;
                        _controller._view.NPCostPopup.SetBrain(_currentSenderBrain, _currentEnterBrain);
                        break;

                }
            }
            public void OnExit()
            {
                _controller._view.NPCostPopup.Dispose();
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
                _channel = Managers.Pool.GrabPrefabs(EPrefabsType.CHANNEL, "Channel", _controller._view.transform).GetComponent<Channel>();
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
                    var res = await Managers.Network.API_CreateChannel(req);
                    if (res != null)
                    {
                        _controller._model.SingleNetworkWrapper.UpdateSingleNetworkData(req, res, () =>
                        {
                            Managers.Notification.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);
                        });
                    }
                }

                _controller.ChangeState(EBehaviorState.NONE);
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

            public void LateAdvanceTime(float dt_sec)
            {
            }
            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.CLOSE_BRAININFO_POPUP:
                    case ENotiMessage.CLOSE_RESET_POPUP:
                    case ENotiMessage.CLOSE_LEADERBOARD_POPUP:
                        _controller.ChangeState(EBehaviorState.NONE);
                        break;
                    case ENotiMessage.ONCLICK_UPGRADE_BRAIN:
                        long brainId = (long)noti.data[EDataParamKey.BRAIN_ID];
                        UpgradeBrain(brainId);
                        break;
                    case ENotiMessage.EXPERIMENT_COMPLETE:
                        Managers.Popup.DeleteAll(PopupType.NORMAL);  // 현재 떠있는 모든 팝업 닫음
                        ResetPopup infoPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "CompletePopup", PopupType.NORMAL)
                            .GetComponent<ResetPopup>();
                        infoPopup.Init();
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
                req.level = 1;
                CreateSingleNetworkBrainNumberResponse res = await Managers.Network.API_UpgradeBrain(req);

                if (res != null)
                {
                    // 브레인 업그레이드 상태가 바로 반영되도록 업데이트해주는 콜백 추가
                    _controller._model.SingleNetworkWrapper.UpdateSingleNetworkData(res, () =>
                    {
                        Managers.Notification.PostNotification(ENotiMessage.UPDATE_BRAIN_NETWORK);

                        SingleNetworkWrapper wrapper = _controller._model.SingleNetworkWrapper;
                        BrainNetwork network = _controller._model.BrainNetwork;
                        _controller._view.InfoPopup.Set(network.GetBrainForID(id), wrapper, network);
                    });
                }
                
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
        /// 매 프레임이 지나고 나서 실행
        /// </summary>
        /// <param name="dt_sec"></param>
        void LateAdvanceTime(float dt_sec);
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
