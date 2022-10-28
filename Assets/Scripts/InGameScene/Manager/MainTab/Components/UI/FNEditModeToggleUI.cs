using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainTab
{
    public class FNEditModeToggleUI : MonoBehaviour
    {
        private bool _on = false;

        [SerializeField] private Image _buttonBackground;
        [SerializeField] private TextMeshProUGUI _text;

        public void OnClick_FNEditModeToggle()
        {
            _on = !_on;
            if (_on)
            {
                _buttonBackground.color = new Color32(255, 255, 255, 255);
                _text.color = new Color32(0, 0, 0, 255);
            }
            else
            {
                _buttonBackground.color = new Color32(30, 30, 30, 255);
                _text.color = new Color32(255, 255, 255, 255);
            }

            Managers.Notification.PostNotification(ENotiMessage.FN_EDIT_MODE_TOGGLE);
        }
    }
}
