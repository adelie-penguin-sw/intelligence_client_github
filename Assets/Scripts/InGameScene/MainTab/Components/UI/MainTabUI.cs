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
        [SerializeField] private TextMeshProUGUI _txtTP;

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
        public void SetNPText(PowerTowerNotation np)
        {
            _txtNP.text = string.Format("NP: {0}", np);
        }

        /// <summary>
        /// 상단 TP Text를 해당 인자로 변경한다.
        /// </summary>
        /// <param name="tp">변경할 tp</param>
        public void SetTPText(PowerTowerNotation tp)
        {
            _txtTP.text = string.Format("TP: {0}", tp);
        }

        public void Dispose()
        {
        }
    }
}