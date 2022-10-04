using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MainTab
{
    /// <summary>
    /// Controller에서 받은 데이터들을 화면에서 출력해주는 역할을 함
    /// </summary>
    public class MainTabView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Brain _tempBrain;
        [SerializeField] private ShowCostUI _showCostUI;
        [SerializeField] private QuestUI _questUI;
        private InGame.BrainInfoPopup _infoPopup;
        private InGame.LeaderboardPopup _leaderboardPopup;

        [SerializeField] public ShowCostUI ShowCostUI { get { return _showCostUI; } }
        [SerializeField] public QuestUI QuestUI { get { return _questUI; } }
        public InGame.BrainInfoPopup InfoPopup
        {
            get
            {
                return _infoPopup;
            }
            set
            {
                _infoPopup = value;
            }
        }

        public Brain TempBrain
        {
            get
            {
                return _tempBrain;
            }
        }
    }
}