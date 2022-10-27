using System.Collections;
using System.Collections.Generic;
using MainTab;
using UnityEngine;

namespace TpTab
{
    public class TpBehaviorController : BaseTabController<TpTabApplication>
    {
        protected TpTabModel _model;
        protected TpTabView _view;

        public override void Init(TpTabApplication app)
        {
            base.Init(app);
            _model = app.TpTabModel;
            _view = app.TpTabView;
            InitHandlers();
            ChangeState(ETpBehaviorState.NONE);
            AddObservers();
        }

        public override void Set()
        {
            _view = _app.TpTabView;
        }

        public override void AdvanceTime(float dt_sec)
        {
            if (_currentState != ETpBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).AdvanceTime(dt_sec);
            }
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            if (_currentState != ETpBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).LateAdvanceTime(dt_sec);
            }
        }

        public override void Dispose()
        {
            RemoveObservers();

            foreach (ETpBehaviorState state in _handlers.Keys)
            {
                _handlers[state].Dispose();
            }
        }

        private void OnNotification(Notification noti)
        {
            if (_currentState != ETpBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).OnNotification(noti);
            }

            switch (noti.msg)
            {
                
            }
        }

        private async void TpUpgrade(long id)
        {
            
        }

        private void AddObservers()
        {
            // Managers.Notification.AddObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
        }

        private void RemoveObservers()
        {
            // Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.DRAG_START_CREATEBRAIN);
        }

        #region StateHandler Function
        private Dictionary<ETpBehaviorState, ITpBehaviorStateModule> _handlers = new Dictionary<ETpBehaviorState, ITpBehaviorStateModule>();
        private ETpBehaviorState _currentState = ETpBehaviorState.UNKNOWN;
        private void InitHandlers()
        {
            _handlers.Clear();
            _handlers.Add(ETpBehaviorState.NONE, new StateHandlerNone());
            _handlers.Add(ETpBehaviorState.VIEW_TPUPGRADE, new StateHandlerViewTpUpgrade());

            foreach (ETpBehaviorState state in _handlers.Keys)
            {
                _handlers[state].Init(this);
            }
        }

        private void ChangeState(ETpBehaviorState nextState)
        {
            if (nextState != ETpBehaviorState.UNKNOWN && nextState != _currentState)
            {
                ETpBehaviorState prevState = _currentState;
                _currentState = nextState;
                ITpBehaviorStateModule leaveHandler = GetStateHandler(prevState);
                if (leaveHandler != null)
                {
                    leaveHandler.OnExit();
                }
                ITpBehaviorStateModule enterHandler = GetStateHandler(_currentState);
                if (enterHandler != null)
                {
                    enterHandler.OnEnter();
                }
            }
        }

        private ITpBehaviorStateModule GetStateHandler(ETpBehaviorState state)
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
        protected class StateHandlerNone : ITpBehaviorStateModule
        {
            private TpTabModel _model;
            private TpBehaviorController _controller;
            private bool _isBrainPointDown = false;
            private bool _isTouchStartBrain = false;
            private float _dtBrainPointDown = 0f;
            private const float _limitTowTouch = 0.6f;

            public void Init(TpBehaviorController controller)
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
                if (UserData.IsCompleteExp || Managers.Popup.Count > 0)
                    return;

                if (_isTwoTouch)
                {
                    _dtCountTowTouch += dt_sec;
                    if (_dtCountTowTouch >= _limitTowTouch)
                    {
                        _isTwoTouch = false;
                        _dtCountTowTouch = 0;
                    }
                }
            }

            public void LateAdvanceTime(float dt_sec)
            {
                if (UserData.IsCompleteExp || Managers.Popup.Count > 0)
                    return;

                if (!_isTouchStartBrain)
                {
#if UNITY_EDITOR
                    BehaviorScreenPC();
#else
                    BehaviorScreenMobile();
#endif
                }
            }

            public void OnNotification(Notification noti)
            {
                switch (noti.msg)
                {
                    case ENotiMessage.ONCLICK_TPUPGRADE_ICON:
                        _controller.ChangeState(ETpBehaviorState.VIEW_TPUPGRADE);
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
                        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
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
        protected class StateHandlerViewTpUpgrade : ITpBehaviorStateModule
        {
            private TpBehaviorController _controller;

            public void Init(TpBehaviorController controller)
            {
                _controller = controller;
            }

            public void OnEnter()
            {
                
            }

            private Vector2 _curPos;
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
                    case ENotiMessage.ONCLICK_TPUPGRADE:
                        break;
                    case ENotiMessage.EXIT_TPUPGRADE_POPUP:
                        _controller.ChangeState(ETpBehaviorState.NONE);
                        break;
                }
            }
            public void OnExit()
            {
                
            }

            public void Dispose()
            {
                
            }
        }
    }

    #endregion

    /// <summary>
    /// 사용자 행동 Enum
    /// </summary>
    public enum ETpBehaviorState
    {
        UNKNOWN,
        NONE,
        VIEW_TPUPGRADE,
    }

    /// <summary>
    /// 사용자 행동 State 관리용 interface<br />
    /// TpBehaviorController Inner Class가 가지는 interface<br />
    /// </summary>
    public interface ITpBehaviorStateModule
    {
        /// <summary>
        /// state 최초 생성시 실행
        /// </summary>
        /// <param name="controller"></param>
        void Init(TpBehaviorController controller);
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
