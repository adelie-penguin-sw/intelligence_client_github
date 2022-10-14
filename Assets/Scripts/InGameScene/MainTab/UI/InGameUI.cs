using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

namespace InGame
{
    /// <summary>
    /// InGameScene에서 공통적으로 가지고 있는 UI Class
    /// </summary>
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private BottomTab[] _bottomTabs;
        [SerializeField] private TextMeshProUGUI _txtCoreIntellect;
        [SerializeField] private TextMeshProUGUI _txtNP;
        [SerializeField] private TextMeshProUGUI _txtTP;
        [SerializeField] private TextMeshProUGUI _txtUsername;

        public delegate void LogOutEvent();
        public event LogOutEvent LogOut;
        public void Init(InGameManager manager)
        {
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.UPDATE_NP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.UPDATE_TP);
            foreach (var tab in _bottomTabs)
            {
                tab.Init();
                tab.OnClickTab = OnClick_Tab;
            }

            Set();
            LogOut = manager.LogOut;
        }

        public void Set()
        {
            UpdateCoreIntellectText();
            UpdateNPText();
            UpdateTPText();
            SetUsernameText();
        }

        public void AdvanceTime(float dt_sec)
        {
            UpdateCoreIntellectText();
        }

        public void OnNotification(Notification noti)
        {
            switch(noti.msg)
            {
                case ENotiMessage.UPDATE_NP:
                    UpdateNPText();
                    break;
                case ENotiMessage.UPDATE_TP:
                    UpdateTPText();
                    break;
            }
        }

        public void Dispose()
        {
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.UPDATE_NP);
            Managers.Notification.RemoveObserver(OnNotification, ENotiMessage.UPDATE_TP);
        }

        private void OnClick_Tab(EGameState tab)
        {
            Hashtable sendData = new Hashtable();
            sendData.Add(EDataParamKey.EGAMESTATE, tab);
            Managers.Notification.PostNotification(ENotiMessage.ONCLICK_CHANGE_TAB, sendData);
        }

        /// <summary>
        /// 상단 Intellect Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="intellect">변경할 intellect</param>
        public void UpdateCoreIntellectText()
        {
            _txtCoreIntellect.text = UserData.CoreIntellect.ToString();
        }

        /// <summary>
        /// 상단 NP Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="np">변경할 np</param>
        public void UpdateNPText()
        {
            _txtNP.text = string.Format("NP: {0}", UserData.NP.ToString());
        }

        /// <summary>
        /// 상단 TP Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="tp">변경할 tp</param>
        public void UpdateTPText()
        {
            _txtTP.text = string.Format("TP: {0}", UserData.TP.ToString());
        }

        public void SetUsernameText()
        {
            _txtUsername.text = UserData.Username;
        }

        public void OnClick_LeaderBoard()
        {
            LeaderboardPopup leaderboardPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "LeaderboardPopup", PopupType.NORMAL)
                                .GetComponent<LeaderboardPopup>();
            if (leaderboardPopup != null)
            {
                leaderboardPopup.Init();
            }
        }

        public void OnClick_Logout()
        {
            LogOut();
        }

        public void OnClick_UserInfo()
        {
            Managers.Popup.CreatePopup(EPrefabsType.POPUP, "UserInfoPopup", PopupType.NORMAL);
        }
    }

}