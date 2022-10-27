using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopTab
{
    public class ShopTabController : BaseTabController<ShopTabApplication>
    {
        public override void Init(ShopTabApplication app)
        {
            base.Init(app);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_AD_REWARD_DOUBLE_TP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_AD_REWARD_SKIP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_AD_REWARD_SPEED);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_REMOVE_AD);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_SKIP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP1);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP2);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP3);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP4);
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

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_AD_REWARD_DOUBLE_TP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_AD_REWARD_SKIP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_AD_REWARD_SPEED);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_REMOVE_AD);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_SKIP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP1);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP2);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP3);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP4);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        private void OnNotification(Notification noti)
        {
            switch (noti.msg)
            {
                case ENotiMessage.ONCLICK_AD_REWARD_DOUBLE_TP:
                    break;
                case ENotiMessage.ONCLICK_AD_REWARD_SKIP:
                    break;
                case ENotiMessage.ONCLICK_AD_REWARD_SPEED:
                    break;
                case ENotiMessage.ONCLICK_PAID_REWARD_REMOVE_AD:
                    break;
                case ENotiMessage.ONCLICK_PAID_REWARD_SKIP:
                    break;
                case ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP1:
                    break;
                case ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP2:
                    break;
                case ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP3:
                    break;
                case ENotiMessage.ONCLICK_PAID_REWARD_BUY_TP4:
                    break;
            }
        }
    }
}
