using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace InGame
{
    public class OptionsFullScreenPopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _currentSFXVolumeText;
        [SerializeField] private TextMeshProUGUI _currentBrainHoldingTimeText;

        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Slider _brainHoldingTimeSlider;

        [SerializeField] private Image _setKoreanButton;
        [SerializeField] private Image _setEnglishButton;

        public override void Init()
        {
            base.Init();

            LoadSettings();
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

        public void OnValueChange_SFX()
        {
            float currentValue = (float)_sfxVolumeSlider.value / 100f;
            UserData.SetFloat("SFXVolume", currentValue);
            _currentSFXVolumeText.text = _sfxVolumeSlider.value.ToString() + "%";
        }

        public void OnValueChange_BrainHoldingTime()
        {
            float currentValue = (float)_brainHoldingTimeSlider.value / 1000f;
            UserData.SetFloat("WaitBrainClickTime", currentValue);
            _currentBrainHoldingTimeText.text = currentValue.ToString("N1") + "s";
        }

        public void OnClick_SetKoreanButton()
        {
            _setKoreanButton.color = new Color32(120, 120, 120, 255);
            _setEnglishButton.color = new Color32(40, 40, 40, 255);
            UserData.SetInt("Language", (int)ETextLanguage.KOR);
        }

        public void OnClick_SetEnglishButton()
        {
            _setEnglishButton.color = new Color32(120, 120, 120, 255);
            _setKoreanButton.color = new Color32(40, 40, 40, 255);
            UserData.SetInt("Language", (int)ETextLanguage.ENG);
        }

        public void OnClick_LogoutButton()
        {
            Managers.Notification.PostNotification(ENotiMessage.LOGOUT);
            Dispose();
        }

        public void OnClick_DeleteAccountButton()
        {

        }

        private void LoadSettings()
        {
            _sfxVolumeSlider.SetValueWithoutNotify((int)(UserData.SFXVolume * 100f));
            _currentSFXVolumeText.text = _sfxVolumeSlider.value.ToString() + "%";

            _brainHoldingTimeSlider.SetValueWithoutNotify((int)(UserData.WaitBrainClickTime * 1000f));
            _currentBrainHoldingTimeText.text = UserData.WaitBrainClickTime.ToString("N1") + "s";

            switch (UserData.Lang)
            {
                case ETextLanguage.KOR:
                    _setKoreanButton.color = new Color32(120, 120, 120, 255);
                    _setEnglishButton.color = new Color32(40, 40, 40, 255);
                    break;
                case ETextLanguage.ENG:
                    _setEnglishButton.color = new Color32(120, 120, 120, 255);
                    _setKoreanButton.color = new Color32(40, 40, 40, 255);
                    break;
            }
        }
    }
}
