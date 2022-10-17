using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class HelpPopup : PopupBase
    {
        [SerializeField] private GameObject _howToPlayTabScrollView;
        [SerializeField] private GameObject _termsTabScrollView;

        [SerializeField] private Image _howToPlayTabButton;
        [SerializeField] private Image _termsTabButton;

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

        public void OnClick_HowToPlayButton()
        {
            _howToPlayTabScrollView.SetActive(true);
            _termsTabScrollView.SetActive(false);

            _howToPlayTabButton.color = new Color32(94, 123, 125, 255);
            _termsTabButton.color = new Color32(38, 49, 51, 255);
        }

        public void OnClick_TermsButton()
        {
            _howToPlayTabScrollView.SetActive(false);
            _termsTabScrollView.SetActive(true);

            _howToPlayTabButton.color = new Color32(38, 49, 51, 255);
            _termsTabButton.color = new Color32(94, 123, 125, 255);
        }
    }
}
