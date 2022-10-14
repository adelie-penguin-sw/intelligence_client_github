using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    public class QuestController : BaseTabController<MainTabApplication>
    {
        private MainTabView _view;
        private Dictionary<int, Quest> _dicQuest = new Dictionary<int, Quest>();
        private Dictionary<int, TutorialQuestDefinition> _dicQuestDef;
        private int _currentQuestKey;
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            _dicQuestDef = Managers.Definition.GetDatas<Dictionary<int, TutorialQuestDefinition>>(EDefType.TUTORIAL_QUEST);
            _view = app.MainTabView;
            _currentQuestKey = 1;

            _dicQuest.Clear();
            _dicQuest.Add(1, new Quest(_dicQuestDef[1].text,EQuestType.CREATE_BRAIN,_dicQuestDef[1].requirement));
            _dicQuest.Add(2, new Quest(_dicQuestDef[2].text, EQuestType.CREATE_CHANNEL, _dicQuestDef[2].requirement));
            _dicQuest.Add(3, new Quest(_dicQuestDef[3].text, EQuestType.BRAIN_UPGRADE, _dicQuestDef[3].requirement));
            _dicQuest.Add(4, new Quest(_dicQuestDef[4].text, EQuestType.NETWORK_RESET, _dicQuestDef[4].requirement));
            _dicQuest.Add(5, new Quest(_dicQuestDef[5].text, EQuestType.BRAIN_SELL_FOR_NP, _dicQuestDef[5].requirement));

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_CREATE_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_CREATE_CHANNEL);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_BRAIN_INTELLIGENCE_UPGRADE);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_RESET_NETWORK);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_BRAIN_SELL);
            SetQuestUI(_dicQuest[_currentQuestKey]);
        }

        public override void Set()
        {
            base.Set();
        }
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        public override void Dispose()
        {
            base.Dispose();
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.QUEST_CREATE_BRAIN);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.QUEST_CREATE_CHANNEL);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.QUEST_BRAIN_INTELLIGENCE_UPGRADE);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_RESET_NETWORK);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.QUEST_BRAIN_SELL);
        }

        public void SetQuestUI(Quest quest)
        {
            _view.QuestUI.Set(quest);
        }
        private void OnNotification(Notification noti)
        {
            switch (noti.msg)
            {
                case ENotiMessage.QUEST_CREATE_BRAIN:
                    if(_dicQuest[_currentQuestKey].type == EQuestType.CREATE_BRAIN)
                    {
                        _dicQuest[_currentQuestKey].clearCount++;
                    }
                    break;
                case ENotiMessage.QUEST_CREATE_CHANNEL:
                    if (_dicQuest[_currentQuestKey].type == EQuestType.CREATE_CHANNEL)
                    {
                        _dicQuest[_currentQuestKey].clearCount++;
                    }
                    break;
                case ENotiMessage.QUEST_BRAIN_INTELLIGENCE_UPGRADE:
                    if (_dicQuest[_currentQuestKey].type == EQuestType.BRAIN_UPGRADE)
                    {
                        _dicQuest[_currentQuestKey].clearCount++;
                    }
                    break;
                case ENotiMessage.ONCLICK_RESET_NETWORK:
                    if (_dicQuest[_currentQuestKey].type == EQuestType.NETWORK_RESET)
                    {
                        _dicQuest[_currentQuestKey].clearCount++;
                    }
                    break;
                case ENotiMessage.QUEST_BRAIN_SELL:
                    if (_dicQuest[_currentQuestKey].type == EQuestType.BRAIN_SELL_FOR_NP)
                    {
                        _dicQuest[_currentQuestKey].clearCount++;
                    }
                    break;
            }
            _view.QuestUI.UpdateClearCount(_dicQuest[_currentQuestKey].clearCount);
            QuestClearCheck();
        }

        private async void QuestClearCheck()
        {
            var req = new CompleteQuestRequest();
            req.questId = _currentQuestKey;
            await Managers.Network.API_QuestComplete(req);
            if (_dicQuest[_currentQuestKey].clearCount>= _dicQuest[_currentQuestKey].goalCount)
            {
                if (_dicQuest.ContainsKey(_currentQuestKey+1))
                {
                    _currentQuestKey++;
                    SetQuestUI(_dicQuest[_currentQuestKey]);
                }
            }
        }
    }

    public class Quest
    {
        public string text;
        public EQuestType type;
        public long goalCount;
        public long clearCount;
        public bool isClear;

        public Quest(string text, EQuestType type, long goalCount,long clearCount = 0, bool isClear = false)
        {
            this.text = text;
            this.type = type;
            this.goalCount = goalCount;
            this.clearCount = clearCount;
            this.isClear = isClear;
        }
    }

    public enum EQuestType
    {
        CREATE_BRAIN,
        CREATE_CHANNEL,
        BRAIN_UPGRADE,
        BRAIN_SELL_FOR_NP,
        NETWORK_RESET,
    }
}