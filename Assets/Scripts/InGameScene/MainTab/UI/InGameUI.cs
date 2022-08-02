using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InGame
{
    /// <summary>
    /// InGameScene에서 공통적으로 가지고 있는 UI Class
    /// </summary>
    public class InGameUI : MonoBehaviour
    {
        [SerializeField]private BottomTab[] _bottomTabs;
        public void Init()
        {
            foreach(var tab in _bottomTabs)
            {
                tab.Init();
                tab.OnClickTab = OnClick_Tab;
            }
        }

        private void OnClick_Tab(EGameState tab)
        {
            switch (tab)
            {
                case EGameState.MAIN_TAB:
                    Debug.Log("Main_Tab Click");
                    break;
                case EGameState.TP_UPGRADE_TAB:
                    Debug.Log("TP_UPGRADE_TAB Click");
                    break;
            }
        }
    }

}