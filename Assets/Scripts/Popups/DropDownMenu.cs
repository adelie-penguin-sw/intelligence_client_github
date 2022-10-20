using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace InGame
{
    public class DropDownMenu : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _statisticsText;
        [SerializeField] private TextMeshProUGUI _optionsText;
        [SerializeField] private TextMeshProUGUI _helpText;
        [SerializeField] private TextMeshProUGUI _aboutText;

        public override void Init()
        {
            base.Init();
            _statisticsText.text = Managers.Definition.GetUIText(UITextKey.dropDownMenuItemTextStatistics);
            _optionsText.text = Managers.Definition.GetUIText(UITextKey.dropDownMenuItemTextOptions);
            _helpText.text = Managers.Definition.GetUIText(UITextKey.dropDownMenuItemTextHelp);
            _aboutText.text = Managers.Definition.GetUIText(UITextKey.dropDownMenuItemTextAbout);
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

        public void OnClick_StatisticsButton()
        {
            StatisticsFullScreenPopup statisticsPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "StatisticsFullScreenPopup", PopupType.NORMAL)
                                .GetComponent<StatisticsFullScreenPopup>();
            if (statisticsPopup != null)
            {
                statisticsPopup.Init();
            }

            Dispose();
        }

        public void OnClick_OptionButton()
        {
            OptionsFullScreenPopup optionsPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "OptionsFullScreenPopup", PopupType.NORMAL)
                                .GetComponent<OptionsFullScreenPopup>();
            if (optionsPopup != null)
            {
                optionsPopup.Init();
            }

            Dispose();
        }

        public void OnClick_HelpButton()
        {
            HelpPopup helpPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "HelpPopup", PopupType.NORMAL)
                                .GetComponent<HelpPopup>();
            if (helpPopup != null)
            {
                helpPopup.Init();
            }

            Dispose();
        }

        public void OnClick_AboutButton()
        {
            AboutFullScreenPopup aboutPopup = Managers.Popup.CreatePopup(EPrefabsType.POPUP, "AboutFullScreenPopup", PopupType.NORMAL)
                                .GetComponent<AboutFullScreenPopup>();
            if (aboutPopup != null)
            {
                aboutPopup.Init();
            }

            Dispose();
        }
    }
}
