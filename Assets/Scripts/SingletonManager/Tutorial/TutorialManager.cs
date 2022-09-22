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
            "������ ����ǿ� ���� ���� ȯ���մϴ�! �극�� ��Ʈ��ũ�� ������� ������ �� ���� ���ɼ����� �����ϴ� ���� �� ������ ���� �⺻���̸鼭�� �ñ����� ��ǥ�Դϴ�."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(2, new TutorialTempClass(2,
            "���� ��� �տ��� �극�� ��Ʈ��ũ�� �߽��� �� \"�ھ� �극��\"�� �ֽ��ϴ�. �� �ھ� �극���� ���ɼ����� �� ����� ���� ������ �� ���Դϴ�."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(3, new TutorialTempClass(3,
            "�극�� ��Ʈ��ũ�� �����ϰ� �����ϱ� ���ؼ��� \"Networking Power(NP)\"��� Ư���� ������ �������� �ʿ��մϴ�. ������ �ּ����� NP�� �־��� �ֱ���."
            , TUTORIAL_BEHAVIOR.OPEN_UI_NP, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(4, new TutorialTempClass(4,
            "�ھ� �극���� ������ �ٸ� �극�ε��� ������ �޾� ������ ����ų �� �ֽ��ϴ�."
            , TUTORIAL_BEHAVIOR.NONE, TUTORIAL_QUEST.NONE, true, 3f));
        _tutorialDic.Add(5, new TutorialTempClass(5,
            "���� �ϴ��� ��ư���κ��� �巡���Ͽ� ���ϴ� ��ġ�� �극���� �����մϴ�. �� ���ۿ��� NP�� �Һ�˴ϴ�."
            , TUTORIAL_BEHAVIOR.OPEN_UI_CRATEBRAIN, TUTORIAL_QUEST.CREATE_BRAIN, false, 3f));
        _tutorialDic.Add(5, new TutorialTempClass(6,
            "���� �ϴ��� ��ư���κ��� �巡���Ͽ� ���ϴ� ��ġ�� �극���� �����մϴ�. �� ���ۿ��� NP�� �Һ�˴ϴ�."
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
    /// ����� �ൿ Enum
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
    /// ����� �ൿ State ������ interface<br />
    /// BehaviorController Inner Class�� ������ interface<br />
    /// </summary>
    public interface ITutorialStateModule
    {
        /// <summary>
        /// state ���� ������ ����
        /// </summary>
        /// <param name="controller"></param>
        void Init(TutorialManager parent);
        /// <summary>
        /// �ش� ���ۻ��� ���Խ� ���� 
        /// </summary>
        void OnEnter();
        /// <summary>
        /// �� ƽ���� �����Ͽ� ����
        /// </summary>
        /// <param name="dt_sec">DeltaTime</param>
        void AdvanceTime(float dt_sec);
        /// <summary>
        /// �� �������� ������ ���� ����
        /// </summary>
        /// <param name="dt_sec"></param>
        void OnNotification(Notification noti);
        /// <summary>
        /// �ش� ���ۻ��� Ż��� ����
        /// </summary>
        void OnExit();
        /// <summary>
        /// �޸� �ʱ�ȭ��
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
