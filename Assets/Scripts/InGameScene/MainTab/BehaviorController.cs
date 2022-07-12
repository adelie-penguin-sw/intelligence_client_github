using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class BehaviorController : BaseTabController<MainTabApplication>
    {
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ON_CLICK_CREATE_BRAIN_BTN);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ON_CLICK_CREATE_CHANNEL_BTN);
            InitHandlers();
            ChangeState(EBehaviorState.NONE);
        }

        public override void Set()
        {
        }

        public override void AdvanceTime(float dt_sec)
        {
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

        private Dictionary<EBehaviorState, IGameStateBasicModule> _handlers = new Dictionary<EBehaviorState, IGameStateBasicModule>();
        private EBehaviorState _currentState = EBehaviorState.UNKNOWN;
        private void InitHandlers()
        {
            _handlers.Clear();
            _handlers.Add(EBehaviorState.NONE, new StateHandlerNone());

            foreach (EBehaviorState state in _handlers.Keys)
            {
                _handlers[state].Init();
            }
        }

        private void ChangeState(EBehaviorState nextState)
        {
            if (nextState != EBehaviorState.UNKNOWN && nextState != _currentState)
            {
                EBehaviorState prevState = _currentState;
                _currentState = nextState;
                IGameStateBasicModule leaveHandler = GetStateHandler(prevState);
                if (leaveHandler != null)
                {
                    leaveHandler.OnExit();
                }
                IGameStateBasicModule enterHandler = GetStateHandler(_currentState);
                if (enterHandler != null)
                {
                    enterHandler.OnEnter();
                }
            }
        }

        private IGameStateBasicModule GetStateHandler(EBehaviorState state)
        {
            if (_handlers.ContainsKey(state))
            {
                return _handlers[state];
            }
            return null;
        }

        #region StateHandler Class
        protected class StateHandlerNone : IGameStateBasicModule
        {
            public void Init()
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

        protected class StateHandlerCreateBrain : IGameStateBasicModule
        {
            public void Init()
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

        protected class StateHandlerCreateChannel : IGameStateBasicModule
        {
            public void Init()
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
        #endregion
    }

    public enum EBehaviorState
    {
        UNKNOWN,
        NONE,
        CREATE_BRAIN,
        CREATE_CHANNEL,
    }
}