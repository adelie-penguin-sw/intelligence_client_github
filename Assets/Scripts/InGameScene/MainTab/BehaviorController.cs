using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class BehaviorController : BaseTabController<MainTabApplication>
    {
        private MainTabModel _model;
        private MainTabView _view;
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            _model = app.MainTabModel;
            _view = app.MainTabView;

            InitHandlers();
            ChangeState(EBehaviorState.NONE);

            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ON_CLICK_CREATE_BRAIN_BTN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ON_CLICK_CREATE_CHANNEL_BTN);
        }

        public override void Set()
        {
        }

        public override void AdvanceTime(float dt_sec)
        {
            MoveScreen();
            ZoomScreenPC();
            //ZoomScreenMobile();

            if (_currentState != EBehaviorState.UNKNOWN)
            {
                GetStateHandler(_currentState).AdvanceTime(dt_sec);
            }
        }

        public override void Dispose()
        {
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.ON_CLICK_CREATE_BRAIN_BTN);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.ON_CLICK_CREATE_CHANNEL_BTN);
        }

        private void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.ON_CLICK_CREATE_BRAIN_BTN:
                    ChangeState(EBehaviorState.CREATE_BRAIN);
                    break;
                case ENotiMessage.ON_CLICK_CREATE_CHANNEL_BTN:
                    ChangeState(EBehaviorState.CREATE_CHANNEL);
                    break;
            }
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
            public void Init(BehaviorController controller)
            {
            }

            public void OnEnter()
            {
            }

            public void AdvanceTime(float dt_sec)
            {
            }

            public void OnExit()
            {
            }

            public void Dispose()
            {
            }

        }

        protected class StateHandlerCreateBrain : IBehaviorStateModule
        {
            private GameObject _goBrainTemp;
            private Camera _camera;
            public void Init(BehaviorController controller)
            {
                _camera = controller._model.MainCamera;
                _goBrainTemp = PoolManager.Instance.GrabPrefabs(EPrefabsType.Brain, "Brain", controller._view.transform);
                _goBrainTemp.SetActive(false);
            }

            public void OnEnter()
            {
                Debug.Log("CreateBrain!");
                _goBrainTemp.SetActive(true);
            }

            public void AdvanceTime(float dt_sec)
            {
                _goBrainTemp.transform.position = (Vector2)_camera.transform.position;
            }

            public void OnExit()
            {
                _goBrainTemp.SetActive(false);
            }

            public void Dispose()
            {
                PoolManager.Instance.DespawnObject(EPrefabsType.Brain, _goBrainTemp);
            }

        }

        protected class StateHandlerCreateChannel : IBehaviorStateModule
        {
            public void Init(BehaviorController controller)
            {
            }

            public void OnEnter()
            {
                Debug.Log("CreateChannel!");
            }

            public void AdvanceTime(float dt_sec)
            {
            }

            public void OnExit()
            {
            }

            public void Dispose()
            {
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
        void OnExit();
        void Dispose();
    }
}