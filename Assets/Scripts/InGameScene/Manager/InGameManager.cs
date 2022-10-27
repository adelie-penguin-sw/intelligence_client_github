using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace InGame
{
    /// <summary>
    /// Unity의 생명주기를 가지고 있다<br />
    /// InGameScene을 총괄하는 Manager<br />
    /// Application을 tab 별로 가지고 stateHandelr에 따라 조작시키고 실행시켜준다. <br />
    /// </summary>
    public class InGameManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _anchor;
        [SerializeField] private InGameUI _ui;
        [SerializeField] private List<TabApp> _tabAppList;

        void Awake()
        {
            
        }

        void Start()
        {
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.LOGOUT);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_CHANGE_TAB);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.LOGOUT);
            if (_ui != null)
            {
                _ui.Init();
            }
            InitHandlers();
            ChangeState(EGameState.MAIN_TAB);
        }

        void Update()
        {
            if (_ui != null)
            {
                _ui.AdvanceTime(Time.deltaTime);
            }
            if (_currentState != EGameState.UNKNOWN)
            {
                GetStateHandler(_currentState).AdvanceTime(Time.deltaTime);
            }
        }

        void LateUpdate()
        {
            if (_currentState != EGameState.UNKNOWN)
            {
                GetStateHandler(_currentState).LateAdvanceTime(Time.deltaTime);
            }
        }

        public void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.ONCLICK_CHANGE_TAB:

                    EGameState state = (EGameState)noti.data[EDataParamKey.EGAMESTATE];
                    ChangeState(state);
                    break;
                case ENotiMessage.LOGOUT:
                    Dispose();
                    break;
            }
        }

        public void Dispose()
        {
            _ui.Dispose();
            GC.SuppressFinalize(this);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_CHANGE_TAB);
            //Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.LOGOUT);
            this.Dispose(true);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;
            if (disposing)
            {
                foreach (EGameState state in _handlers.Keys)
                {
                    _handlers[state].Dispose();
                }
            }
            this._disposed = true;
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LoginScene");
        }

        [ShowInInspector] private Dictionary<EGameState, IGameStateBasicModule> _handlers = new Dictionary<EGameState, IGameStateBasicModule>();
        private EGameState _currentState = EGameState.UNKNOWN;
        private GameObject _goTemp;
        private void InitHandlers()
        {
            _handlers.Clear();
            foreach(var tab in _tabAppList)
            {
                _handlers.Add(tab.tabType, tab.tabApp);
            }

            foreach (EGameState state in _handlers.Keys)
            {
                _handlers[state].Init();
            }
        }

        private void ChangeState(EGameState nextState)
        {
            if (nextState != EGameState.UNKNOWN && nextState != _currentState)
            {
                EGameState prevState = _currentState;
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

        /// <summary>
        /// 해당하는 state가 현재 handler에 들어있는 경우 해당하는 handler의 인터페이스를 반환해준다.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>현재 핸들러에 없는 경우 null, 유효한 경우 해당 인터페이스 반환</returns>
        private IGameStateBasicModule GetStateHandler(EGameState state)
        {
            if (_handlers.ContainsKey(state))
            {
                return _handlers[state];
            }
            return null;
        }
    }

    /// <summary>
    /// Tab에 따른 State를 표현하기 위한 Enum
    /// </summary>
    public enum EGameState
    {
        UNKNOWN,
        /// <summary>
        /// 메인탭
        /// </summary>
        MAIN_TAB,
        /// <summary>
        /// TP 사용 업그레이드 탭
        /// </summary>
        TP_UPGRADE_TAB,
        /// <summary>
        /// 리더보드 및 각 유저의 정보를 볼 수 있는 탭
        /// </summary>
        USER_TAB,
        /// <summary>
        /// 광고 시청 및 유료 상점 탭
        /// </summary>
        SHOP_TAB,
    }

    [Serializable]
    struct TabApp
    {
        public EGameState tabType;
        public BaseTabApplication tabApp;
    }
}