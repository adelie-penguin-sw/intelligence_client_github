using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MainTab;

namespace InGame
{
    /// <summary>
    /// 리더보드 팝업 클래스
    /// </summary>
    public class LeaderboardPopup : PopupBase
    {
        [SerializeField] private Transform _leaderboardContent;

        private List<RankItem> _rankItemList = new List<RankItem>();

        public override async void Init()
        {
            base.Init();

            var res = await NetworkManager.Instance.API_Leaderboard();
            if (res != null)
            {
                res.allRank.Sort((r1, r2) => r1.rank.CompareTo(r2.rank));
                foreach (var item in res.allRank)
                {
                    RankItem rankItem = Managers.Pool.GrabPrefabs(EPrefabsType.RANK_ITEM, "RankItem", _leaderboardContent).GetComponent<RankItem>();
                    rankItem.Init(item, item.rank == res.selfRank);
                    _rankItemList.Add(rankItem);
                }
            }
        }

        public override void Set()
        {
            base.Set();
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (RankItem item in _rankItemList)
            {
                item.Dispose();
            }
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_LEADERBOARD_POPUP);
        }
    }
}
