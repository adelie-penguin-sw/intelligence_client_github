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

        private InGame.BrainInfoPopup _infoPopup;
        private InGame.LeaderboardPopup _leaderboardPopup;
        private InGame.NPCostPopup _npCostPopup;

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

        public InGame.LeaderboardPopup LeaderboardPopup
        {
            get
            {
                return _leaderboardPopup;
            }
            set
            {
                _leaderboardPopup = value;
            }
        }

        public InGame.NPCostPopup NPCostPopup
        {
            get
            {
                return _npCostPopup;
            }
            set
            {
                _npCostPopup = value;
            }
        }
    }
}