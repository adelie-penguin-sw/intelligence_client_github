using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class HelpPopup : PopupBase
    {
        [SerializeField] private GameObject _howToPlayTabContent;
        [SerializeField] private GameObject _termsTabContent;
        [SerializeField] private GameObject _scrollBar;

        [SerializeField] private Image _howToPlayTabButton;
        [SerializeField] private Image _termsTabButton;

        public override void Init()
        {
            base.Init();
            _scrollBar.SetActive(true);
        }

        public override void Set()
        {
            base.Set();
            _scrollBar.SetActive(true);
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);
            _scrollBar.SetActive(true);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void OnClick_HowToPlayButton()
        {
            _howToPlayTabContent.SetActive(true);
            _termsTabContent.SetActive(false);

            _howToPlayTabButton.color = new Color32(94, 123, 125, 255);
            _termsTabButton.color = new Color32(38, 49, 51, 255);
        }

        public void OnClick_TermsButton()
        {
            _howToPlayTabContent.SetActive(false);
            _termsTabContent.SetActive(true);

            _howToPlayTabButton.color = new Color32(38, 49, 51, 255);
            _termsTabButton.color = new Color32(94, 123, 125, 255);
        }
    }
}
