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
        public override void Init(MainTabApplication app)
        {
            base.Init(app);
            _dicQuestDef = Managers.Definition.GetDatas<Dictionary<int, TutorialQuestDefinition>>(EDefType.TUTORIAL_QUEST);
            _view = app.MainTabView;
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_CREATE_BRAIN);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_CREATE_CHANNEL);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_BRAIN_INTELLIGENCE_UPGRADE);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_RESET_NETWORK);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.QUEST_BRAIN_SELL);
        }

        public override void Set()
        {
            base.Set();
            _view.QuestUI.gameObject.SetActive(!UserData.IsTutorialClear);
            _dicQuest.Clear();
            _dicQuest.Add(1, new Quest(1,_dicQuestDef[1].text, EQuestType.CREATE_BRAIN, _dicQuestDef[1].requirement));
            _dicQuest.Add(2, new Quest(2,_dicQuestDef[2].text, EQuestType.CREATE_CHANNEL, _dicQuestDef[2].requirement));
            _dicQuest.Add(3, new Quest(3,_dicQuestDef[3].text, EQuestType.BRAIN_UPGRADE, _dicQuestDef[3].requirement));
            _dicQuest.Add(4, new Quest(4,_dicQuestDef[4].text, EQuestType.NETWORK_RESET, _dicQuestDef[4].requirement));
            _dicQuest.Add(5, new Quest(5,_dicQuestDef[5].text, EQuestType.BRAIN_SELL_FOR_NP, _dicQuestDef[5].requirement));

            if ((int)UserData.CurrentTutorialKey != -1)
            {
                SetQuestUI(_dicQuest[(int)UserData.CurrentTutorialKey]);
            }
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
            if (UserData.IsTutorialClear) return;
            int currentKey = (int)UserData.CurrentTutorialKey;
            switch (noti.msg)
            {
                case ENotiMessage.QUEST_CREATE_BRAIN:
                    if(_dicQuest[currentKey].type == EQuestType.CREATE_BRAIN)
                    {
                        QuestClearCheck();
                    }
                    break;
                case ENotiMessage.QUEST_CREATE_CHANNEL:
                    if (_dicQuest[currentKey].type == EQuestType.CREATE_CHANNEL)
                    {
                        QuestClearCheck();
                    }
                    break;
                case ENotiMessage.QUEST_BRAIN_INTELLIGENCE_UPGRADE:
                    if (_dicQuest[currentKey].type == EQuestType.BRAIN_UPGRADE)
                    {
                        QuestClearCheck();
                    }
                    break;
                case ENotiMessage.ONCLICK_RESET_NETWORK:
                    if (_dicQuest[currentKey].type == EQuestType.NETWORK_RESET)
                    {
                        QuestClearCheck();
                    }
                    break;
                case ENotiMessage.QUEST_BRAIN_SELL:
                    if (_dicQuest[currentKey].type == EQuestType.BRAIN_SELL_FOR_NP)
                    {
                        QuestClearCheck();
                    }
                    break;
            }
            _view.QuestUI.UpdateClearCount();
        }

        private async void QuestClearCheck()
        {
            if (UserData.CurrentTutorialKey != -1)
            {
                var req = new CompleteQuestRequest();
                req.questId = UserData.CurrentTutorialKey;
                await Managers.Network.API_QuestComplete(req);
                if (UserData.CurrentTutorialKey != -1)
                {
                    SetQuestUI(_dicQuest[(int)UserData.CurrentTutorialKey]);
                }
                _view.QuestUI.gameObject.SetActive(!UserData.IsTutorialClear);
            }
        }
    }

    public class Quest
    {
        public long id;
        public string text;
        public EQuestType type;
        public long goalCount;
        public bool isClear;

        public Quest(long id, string text, EQuestType type, long goalCount,bool isClear = false)
        {
            this.id = id;
            this.text = text;
            this.type = type;
            this.goalCount = goalCount;
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