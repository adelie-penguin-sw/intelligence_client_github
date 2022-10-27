using System.Collections;
using System.Collections.Generic;
using InGame;
using UnityEngine;

namespace UserTab
{
    public class UserTabController : BaseTabController<UserTabApplication>
    {
        public override void Init(UserTabApplication app)
        {
            base.Init(app);

            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_TOP_RANK);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_MY_RANK);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_PERCENT_RANK);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_HIGHER_RANK);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.ONCLICK_LOWER_RANK);
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

            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_TOP_RANK);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_MY_RANK);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_PERCENT_RANK);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_HIGHER_RANK);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.ONCLICK_LOWER_RANK);
        }

        public override void LateAdvanceTime(float dt_sec)
        {
            base.LateAdvanceTime(dt_sec);
        }

        private void OnNotification(Notification noti)
        {
            switch (noti.msg)
            {
                case ENotiMessage.ONCLICK_TOP_RANK:
                    break;
                case ENotiMessage.ONCLICK_MY_RANK:
                    break;
                case ENotiMessage.ONCLICK_PERCENT_RANK:
                    PercentageSettingPopup percentageSettingPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "PercentageSettingPopup", PopupType.NORMAL)
                                .GetComponent<PercentageSettingPopup>();
                    if (percentageSettingPopup != null)
                    {
                        percentageSettingPopup.Init();
                    }
                    break;
                case ENotiMessage.ONCLICK_HIGHER_RANK:
                    break;
                case ENotiMessage.ONCLICK_LOWER_RANK:
                    break;
            }
        }
    }
}
