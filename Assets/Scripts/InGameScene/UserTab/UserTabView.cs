using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserTab
{
    public class UserTabView : MonoBehaviour
    {
        public void OnClick_TopRank()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_TOP_RANK);
        }

        public void OnClick_MyRank()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_MY_RANK);
        }

        public void OnClick_PercentRank()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PERCENT_RANK);
        }

        public void OnClick_HigherRank()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_HIGHER_RANK);
        }

        public void OnClick_LowerRank()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_LOWER_RANK);
        }
    }
}
