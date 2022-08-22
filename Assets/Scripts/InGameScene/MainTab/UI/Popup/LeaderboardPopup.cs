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
        public override void Init()
        {
            base.Init();
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
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_BRAININFO_POPUP);
        }
    }
}
