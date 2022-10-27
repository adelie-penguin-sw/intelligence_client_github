using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InGame
{
    public class HelpPopup : PopupBase
    {
        [SerializeField] private GameObject _howToPlayTabScrollView;
        [SerializeField] private GameObject _termsTabScrollView;

        [SerializeField] private Image _howToPlayTabButton;
        [SerializeField] private Image _termsTabButton;

        // 제목 텍스트
        [SerializeField] private TextMeshProUGUI _helpPopupTitle;

        // 탭 버튼 텍스트
        [SerializeField] private TextMeshProUGUI _howToPlayTabButtonText;
        [SerializeField] private TextMeshProUGUI _termsTabButtonText;

        // 기본 조작법 텍스트
        [SerializeField] private TextMeshProUGUI _howToExploreNetworkTitle;

        // 용어정의 텍스트
        [SerializeField] private TextMeshProUGUI _termExperiment;

        public override void Init()
        {
            base.Init();

            // 제목 텍스트
            _helpPopupTitle.text = Managers.Definition.GetTextData(10001);

            // 탭 버튼 텍스트
            _howToPlayTabButtonText.text = Managers.Definition.GetTextData(10002);
            _termsTabButtonText.text = Managers.Definition.GetTextData(10003);

            // 기본 조작법 텍스트
            _howToExploreNetworkTitle.text = Managers.Definition.GetTextData(10004);
            //_howToExploreNetworkDescription.text = Managers.Definition.GetUIText(howToExploreNetworkDescription);
            //_howToCreateBrainTitle.text = Managers.Definition.GetUIText(howToCreateBrainTitle);
            //_howToCreateBrainDescription.text = Managers.Definition.GetUIText(howToCreateBrainDescription);
            //_howToCreateChannelTitle.text = Managers.Definition.GetUIText(howToCreateChannelTitle);
            //_howToCreateChannelDescription.text = Managers.Definition.GetUIText(howToCreateChannelDescription);
            //_howToViewBrainInfoTitle.text = Managers.Definition.GetUIText(howToViewBrainInfoTitle);
            //_howToViewBrainInfoDescription.text = Managers.Definition.GetUIText(howToViewBrainInfoDescription);

            // 용어정의 텍스트
            _termExperiment.text = Managers.Definition.GetTextData(10005);
            //_descriptionExperiment.text = Managers.Definition.GetUIText(experimentDescription);
            //_termCoreBrain.text = Managers.Definition.GetUIText(coreBrainTerm);
            //_descriptionCoreBrain.text = Managers.Definition.GetUIText(coreBrainDescription);
            //_termNormalBrain.text = Managers.Definition.GetUIText(normalBrainTerm);
            //_descriptionNormalBrain.text = Managers.Definition.GetUIText(normalBrainDescription);
            //_termIntellect.text = Managers.Definition.GetUIText(intellectTerm);
            //_descriptionIntellect.text = Managers.Definition.GetUIText(intellectDescription);
            //_termChannel.text = Managers.Definition.GetUIText(channelTerm);
            //_descriptionChannel.text = Managers.Definition.GetUIText(channelDescription);
            //_termBrainNetwork.text = Managers.Definition.GetUIText(brainNetworkTerm);
            //_descriptionBrainNetwork.text = Managers.Definition.GetUIText(brainNetworkDescription);
            //_termDecomposition.text = Managers.Definition.GetUIText(decompositionTerm);
            //_descriptionDecomposition.text = Managers.Definition.GetUIText(decompositionDescription);
            //_termIntellectLimit.text = Managers.Definition.GetUIText(intellectLimitTerm);
            //_descriptionIntellectLimit.text = Managers.Definition.GetUIText(intellectLimitDescription);
            //_termMultiplier.text = Managers.Definition.GetUIText(multiplierTerm);
            //_descriptionMultiplier.text = Managers.Definition.GetUIText(multiplierDescription);
            //_termDepth.text = Managers.Definition.GetUIText(depthTerm);
            //_descriptionDepth.text = Managers.Definition.GetUIText(depthDescription);
            //_termExperimentGoal.text = Managers.Definition.GetUIText(experimentGoalTerm);
            //_descriptionExperimentGoal.text = Managers.Definition.GetUIText(experimentGoalDescription);
            //_termReset.text = Managers.Definition.GetUIText(resetTerm);
            //_descriptionReset.text = Managers.Definition.GetUIText(resetDescription);
            //_termNP.text = Managers.Definition.GetUIText(npTerm);
            //_descriptionNP.text = Managers.Definition.GetUIText(npDescription);
            //_termTP.text = Managers.Definition.GetUIText(tpTerm);
            //_descriptionTP.text = Managers.Definition.GetUIText(tpDescription);
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
