using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InGame
{
    public class InGameUI : MonoBehaviour
    {
        private BottomTab[] _bottomTabs;
        public void Init()
        {
            foreach(var tab in _bottomTabs)
            {
                tab.Init();
                tab.OnClickTab = OnClick_Tab;
            }
        }

        private void OnClick_Tab(EInGameTab tab)
        {
            switch (tab)
            {
                case EInGameTab.MAIN_TAB:
                    break;
                case EInGameTab.TP_UPGRADE_TAB:
                    break;
            }
        }
    }

}