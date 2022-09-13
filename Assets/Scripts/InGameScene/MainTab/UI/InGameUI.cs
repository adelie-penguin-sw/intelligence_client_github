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

        public void Init()
        {
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.UPDATE_NP);
            NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.UPDATE_TP);
            foreach (var tab in _bottomTabs)
            {
                tab.Init();
                tab.OnClickTab = OnClick_Tab;
            }

            UpdateCoreIntellectText();
            UpdateNPText();
            UpdateTPText();
        }

        public void Set()
        {
            UpdateCoreIntellectText();
            UpdateNPText();
            UpdateTPText();
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
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.UPDATE_NP);
            NotificationManager.Instance.RemoveObserver(OnNotification, ENotiMessage.UPDATE_TP);
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

        public void OnClick_LeaderBoard()
        {
            NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_LEADERBOARD);
        }

        public void OnClick_Logout()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LoginScene");
        }
    }

}