using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class DropDownMenu : PopupBase
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
        }

        public void OnClick_StatisticsButton()
        {

        }

        public void OnClick_OptionButton()
        {

        }

        public void OnClick_HelpButton()
        {
            HelpPopup dropDownMenu = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "HelpPopup", PopupType.NORMAL)
                                .GetComponent<HelpPopup>();
            if (dropDownMenu != null)
            {
                dropDownMenu.Init();
            }

            Dispose();
        }

        public void OnClick_AboutButton()
        {

        }
    }
}
