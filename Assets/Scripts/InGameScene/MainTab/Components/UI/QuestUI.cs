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
            _txtQuest.text = string.Format(quest.text, quest.clearCount, quest.goalCount);
        }

        public void UpdateClearCount(long clearCount)
        {
            _quest.clearCount = clearCount;
            _txtQuest.text = string.Format(_quest.text, _quest.clearCount, _quest.goalCount);
        }
    }
}