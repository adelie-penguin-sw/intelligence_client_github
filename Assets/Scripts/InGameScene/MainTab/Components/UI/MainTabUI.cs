using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace MainTab
{
    /// <summary>
    /// 메인 탭 UI
    /// </summary>
    public class MainTabUI : MonoBehaviour, IGameBasicModule
    {
        [SerializeField] private TextMeshProUGUI _txtCoreIntellect;
        [SerializeField] private TextMeshProUGUI _txtNP;
        [SerializeField] private TextMeshProUGUI _txtTP;

        public void Init()
        {
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
            UpdateNPText();
            UpdateTPText();
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

        public void OnClick_Logout()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("LoginScene");
        }

        public void Dispose()
        {
        }
    }
}