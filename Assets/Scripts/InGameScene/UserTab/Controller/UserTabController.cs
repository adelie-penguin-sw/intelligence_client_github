using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserTab
{
    public class UserTabController : BaseTabController<UserTabApplication>
    {
        public override void Init(UserTabApplication app)
        {
            base.Init(app);
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
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        private void OnNotification(Notification noti)
        {
            
        }
    }
}
