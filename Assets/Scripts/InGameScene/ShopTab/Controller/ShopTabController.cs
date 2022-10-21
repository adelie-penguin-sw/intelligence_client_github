using System.Collections;
using System.Collections.Generic;
using TpTab;
using UnityEngine;

namespace ShopTab
{
    public class ShopTabController : BaseTabController<ShopTabApplication>
    {
        public override void Init(ShopTabApplication app)
        {
            base.Init(app);

            //Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE);
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
            //Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_TPUPGRADE);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        private void OnNotification(Notification noti)
        {
            switch (noti.msg)
            {
                
            }
        }
    }
}
