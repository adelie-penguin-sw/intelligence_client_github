using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopTab
{
    public class ShopTabView : MonoBehaviour
    {
        public void OnClick_AdRewardDoubleTP()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_AD_REWARD_DOUBLE_TP);
        }

        public void OnClick_AdRewardSkip()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_AD_REWARD_SKIP);
        }

        public void OnClick_AdRewardSpeed()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_AD_REWARD_SPEED);
        }

        public void OnClick_PaidRewardRemoveAd()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PAID_REWARD_REMOVE_AD);
        }

        public void OnClick_PaidRewardSkip()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PAID_REWARD_SKIP);
        }

        public void OnClick_PaidRewardBuyTP1()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP1);
        }

        public void OnClick_PaidRewardBuyTP2()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP2);
        }

        public void OnClick_PaidRewardBuyTP3()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP3);
        }

        public void OnClick_PaidRewardBuyTP4()
        {
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP4);
        }
    }
}

