using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UserTab
{
    public class PercentageSettingPopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _percentText;
        [SerializeField] private Slider _percentageSettingSlider;

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

        public void OnValueChange_Percentage()
        {
            float currentValue = _percentageSettingSlider.value;
            Debug.Log(currentValue);
            _percentText.text = currentValue.ToString("N2") + "%";
        }

        public void OnClick_OK()
        {
            Dispose();
        }
    }
}
