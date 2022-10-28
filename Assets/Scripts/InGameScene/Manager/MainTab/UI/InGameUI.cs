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
        public void Init()
        {
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.UPDATE_NP);
            Managers.Notification.AddObserver(OnNotification, ENotiMessage.UPDATE_TP);
            foreach (var tab in _bottomTabs)
            {
                tab.Init();
                tab.OnClickTab = OnClick_Tab;
            }
            Set();
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

        public void OnClick_Tab(EGameState tab)
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
            string expGoalStr = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[UserData.ExperimentLevel].ToString();
            _txtCoreIntellect.text = string.Format("{0} / {1}", UserData.CoreIntellect.ToString(ECurrencyType.INTELLECT), expGoalStr);
        }

        /// <summary>
        /// 상단 NP Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="np">변경할 np</param>
        public void UpdateNPText()
        {
            _txtNP.text = string.Format("NP: {0}", UserData.NP.ToString(ECurrencyType.NP));
        }

        /// <summary>
        /// 상단 TP Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="tp">변경할 tp</param>
        public void UpdateTPText()
        {
            _txtTP.text = string.Format("TP: {0}", UserData.TP.ToString(ECurrencyType.TP));
        }

        public void SetUsernameText()
        {
            _txtUsername.text = UserData.username;
        }

        public void OnClick_DropDownMenu()
        {
            DropDownMenu dropDownMenu = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "DropDownMenu", PopupType.NORMAL)
                                .GetComponent<DropDownMenu>();
            if (dropDownMenu != null)
            {
                dropDownMenu.Init();
            }
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

        public void OnClick_UserInfo()
        {
            var popupObj = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "UserInfoPopup", PopupType.NORMAL);
            popupObj.GetComponent<UserInfoPopup>().Init();
        }
    }

}