using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace MainTab
{
    /// <summary>
    /// 메인 탭 UI
    /// </summary>
    public class MainTabUI : MonoBehaviour, IGameBasicModule
    {
        [SerializeField] private TextMeshProUGUI _txtNP;

        public void Init()
        {
        }

        public void Set()
        {
        }

        public void AdvanceTime(float dt_sec)
        {
        }

        /// <summary>
        /// 상단 NP Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="np">변경할 np</param>
        public void SetNPText(int np)
        {
            _txtNP.text = string.Format("NP: {0}", np);
        }

        public void Dispose()
        {
        }
    }
}