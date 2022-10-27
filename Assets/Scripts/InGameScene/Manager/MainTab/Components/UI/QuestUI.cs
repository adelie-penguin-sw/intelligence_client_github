using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace MainTab {
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _txtQuest;
        private Quest _quest;
        public void Set(Quest quest)
        {
            _quest = quest;
            _txtQuest.text = string.Format(quest.text, UserData.DicQuest[quest.id].questLevel, quest.goalCount);
        }

        public void UpdateClearCount()
        {
            _txtQuest.text = string.Format(_quest.text, UserData.DicQuest[_quest.id].questLevel, _quest.goalCount);
        }
    }
}