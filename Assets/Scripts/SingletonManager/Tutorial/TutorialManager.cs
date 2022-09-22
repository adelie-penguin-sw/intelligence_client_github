using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private Canvas _canvas = null;
    Dictionary<int, TutorialTempClass> _tutorialDic = new Dictionary<int, TutorialTempClass>();

    public void Init()
    {
        _tutorialDic.Add(1, new TutorialTempClass(1,
            "초지능 실험실에 오신 것을 환영합니다! 브레인 네트워크를 성장시켜 가능한 한 높은 지능수준을 구현하는 것이 이 실험의 가장 기본적이면서도 궁극적인 목표입니다."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(2, new TutorialTempClass(2,
            "지금 당신 앞에는 브레인 네트워크의 중심이 될 \"코어 브레인\"이 있습니다. 이 코어 브레인의 지능수준이 곧 당신의 실험 성과가 될 것입니다."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(3, new TutorialTempClass(3,
            "브레인 네트워크가 성장하고 구동하기 위해서는 \"Networking Power(NP)\"라는 특별한 형태의 에너지가 필요합니다. 다행히 최소한의 NP는 주어져 있군요."
            , TUTORIAL_BEHAVIOR.OPEN_UI_NP, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(4, new TutorialTempClass(4,
            "코어 브레인은 주위의 다른 브레인들의 도움을 받아 지능을 향상시킬 수 있습니다."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(5, new TutorialTempClass(5,
            "우측 하단의 버튼으로부터 드래그하여 원하는 위치에 브레인을 생성합니다. 이 동작에는 NP가 소비됩니다."
            , TUTORIAL_BEHAVIOR.OPEN_UI_CRATEBRAIN, TUTORIAL_QUEST.CREATE_BRAIN, false, 3f));
        _tutorialDic.Add(5, new TutorialTempClass(6,
            "우측 하단의 버튼으로부터 드래그하여 원하는 위치에 브레인을 생성합니다. 이 동작에는 NP가 소비됩니다."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.CREATE_CHANNEL, false, 3f));
    }

    #region StateHandler Function
    private Dictionary<ETutorialState, ITutorialStateModule> _handlers = new Dictionary<ETutorialState, ITutorialStateModule>();
    private ETutorialState _currentState = ETutorialState.UNKNOWN;
    private void InitHandlers()
    {
        _handlers.Clear();
        //_handlers.Add(ETutorialState.NONE, new StateHandlerNone());
        //_handlers.Add(ETutorialState.CREATE_BRAIN, new StateHandlerCreateBrain());
        //_handlers.Add(ETutorialState.CREATE_CHANNEL, new StateHandlerCreateChannel());
        //_handlers.Add(ETutorialState.SHOW_POPUP, new StateHandlerShowPopup());

        foreach (ETutorialState state in _handlers.Keys)
        {
            _handlers[state].Init(this);
        }
    }

    private void ChangeState(ETutorialState nextState)
    {
        if (nextState != ETutorialState.UNKNOWN && nextState != _currentState)
        {
            ETutorialState prevState = _currentState;
            _currentState = nextState;
            ITutorialStateModule leaveHandler = GetStateHandler(prevState);
            if (leaveHandler != null)
            {
                leaveHandler.OnExit();
            }
            ITutorialStateModule enterHandler = GetStateHandler(_currentState);
            if (enterHandler != null)
            {
                enterHandler.OnEnter();
            }
        }
    }

    private ITutorialStateModule GetStateHandler(ETutorialState state)
    {
        if (_handlers.ContainsKey(state))
        {
            return _handlers[state];
        }
        return null;
    }
    #endregion

    private void CreateCanvas()
    {
        if (_canvas == null)
        {
            GameObject go = GameObject.Find("TutorialCanvas");
            if (go == null)
                go = Managers.Pool.GrabPrefabs(EPrefabsType.POPUP, "PopupCanvas", Managers.ManagerObj.transform);

            if (go.TryGetComponent(out Canvas canvas))
            {
                _canvas = canvas;
            }
            else
            {
                _canvas = go.AddComponent<Canvas>();
                Debug.LogError("not canvas");
            }
        }
    }



    /// <summary>
    /// 사용자 행동 Enum
    /// </summary>
    public enum ETutorialState
    {
        UNKNOWN,
        NONE,
        SHOW_SCRIPT,
        QUEST_CHECK,
        END,
    }

    /// <summary>
    /// 사용자 행동 State 관리용 interface<br />
    /// BehaviorController Inner Class가 가지는 interface<br />
    /// </summary>
    public interface ITutorialStateModule
    {
        /// <summary>
        /// state 최초 생성시 실행
        /// </summary>
        /// <param name="controller"></param>
        void Init(TutorialManager parent);
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

public enum TUTORIAL_BEHAVIOR
{
    NONE,
    OPEN_UI_NP,
    OPEN_UI_TP,
    OPEN_UI_CRATEBRAIN,
}

public enum TUTORIAL_QUEST
{
    NONE,
    CREATE_BRAIN,
    CREATE_CHANNEL,
}

public class TutorialTempClass
{
    public int key;
    public string text;
    public TUTORIAL_BEHAVIOR behavior;
    public TUTORIAL_QUEST quest;
    public bool isAuto;
    public float autoTime;

    public TutorialTempClass(int key, string text, TUTORIAL_BEHAVIOR behavior, TUTORIAL_QUEST quest, bool isAuto, float autoTime)
    {
        this.key = key;
        this.text = text;
        this.behavior = behavior;
        this.quest = quest;
        this.isAuto = isAuto;
        this.autoTime = autoTime;
    }
}
