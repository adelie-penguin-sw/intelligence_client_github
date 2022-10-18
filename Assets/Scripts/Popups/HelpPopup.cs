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
        [SerializeField] private TextMeshProUGUI _howToExploreNetworkDescription;
        [SerializeField] private TextMeshProUGUI _howToCreateBrainTitle;
        [SerializeField] private TextMeshProUGUI _howToCreateBrainDescription;
        [SerializeField] private TextMeshProUGUI _howToCreateChannelTitle;
        [SerializeField] private TextMeshProUGUI _howToCreateChannelDescription;
        [SerializeField] private TextMeshProUGUI _howToViewBrainInfoTitle;
        [SerializeField] private TextMeshProUGUI _howToViewBrainInfoDescription;

        // 용어정의 텍스트
        [SerializeField] private TextMeshProUGUI _termExperiment;
        [SerializeField] private TextMeshProUGUI _descriptionExperiment;
        [SerializeField] private TextMeshProUGUI _termCoreBrain;
        [SerializeField] private TextMeshProUGUI _descriptionCoreBrain;
        [SerializeField] private TextMeshProUGUI _termNormalBrain;
        [SerializeField] private TextMeshProUGUI _descriptionNormalBrain;
        [SerializeField] private TextMeshProUGUI _termIntellect;
        [SerializeField] private TextMeshProUGUI _descriptionIntellect;
        [SerializeField] private TextMeshProUGUI _termChannel;
        [SerializeField] private TextMeshProUGUI _descriptionChannel;
        [SerializeField] private TextMeshProUGUI _termBrainNetwork;
        [SerializeField] private TextMeshProUGUI _descriptionBrainNetwork;
        [SerializeField] private TextMeshProUGUI _termDecomposition;
        [SerializeField] private TextMeshProUGUI _descriptionDecomposition;
        [SerializeField] private TextMeshProUGUI _termIntellectLimit;
        [SerializeField] private TextMeshProUGUI _descriptionIntellectLimit;
        [SerializeField] private TextMeshProUGUI _termMultiplier;
        [SerializeField] private TextMeshProUGUI _descriptionMultiplier;
        [SerializeField] private TextMeshProUGUI _termDepth;
        [SerializeField] private TextMeshProUGUI _descriptionDepth;
        [SerializeField] private TextMeshProUGUI _termExperimentGoal;
        [SerializeField] private TextMeshProUGUI _descriptionExperimentGoal;
        [SerializeField] private TextMeshProUGUI _termReset;
        [SerializeField] private TextMeshProUGUI _descriptionReset;
        [SerializeField] private TextMeshProUGUI _termNP;
        [SerializeField] private TextMeshProUGUI _descriptionNP;
        [SerializeField] private TextMeshProUGUI _termTP;
        [SerializeField] private TextMeshProUGUI _descriptionTP;

        public override void Init()
        {
            base.Init();

            // 제목 텍스트
            _helpPopupTitle.text = Managers.Definition.GetUIText(UITextKey.helpPopupTitleText);

            // 탭 버튼 텍스트
            _howToPlayTabButtonText.text = Managers.Definition.GetUIText(UITextKey.helpPopupTabButtonTextHowToPlay);
            _termsTabButtonText.text = Managers.Definition.GetUIText(UITextKey.helpPopupTabButtonTextTerms);

            // 기본 조작법 텍스트
            _howToExploreNetworkTitle.text = Managers.Definition.GetUIText(UITextKey.howToExploreNetworkTitle);
            _howToExploreNetworkDescription.text = Managers.Definition.GetUIText(UITextKey.howToExploreNetworkDescription);
            _howToCreateBrainTitle.text = Managers.Definition.GetUIText(UITextKey.howToCreateBrainTitle);
            _howToCreateBrainDescription.text = Managers.Definition.GetUIText(UITextKey.howToCreateBrainDescription);
            _howToCreateChannelTitle.text = Managers.Definition.GetUIText(UITextKey.howToCreateChannelTitle);
            _howToCreateChannelDescription.text = Managers.Definition.GetUIText(UITextKey.howToCreateChannelDescription);
            _howToViewBrainInfoTitle.text = Managers.Definition.GetUIText(UITextKey.howToViewBrainInfoTitle);
            _howToViewBrainInfoDescription.text = Managers.Definition.GetUIText(UITextKey.howToViewBrainInfoDescription);

            // 용어정의 텍스트
            _termExperiment.text = Managers.Definition.GetUIText(UITextKey.experimentTerm);
            _descriptionExperiment.text = Managers.Definition.GetUIText(UITextKey.experimentDescription);
            _termCoreBrain.text = Managers.Definition.GetUIText(UITextKey.coreBrainTerm);
            _descriptionCoreBrain.text = Managers.Definition.GetUIText(UITextKey.coreBrainDescription);
            _termNormalBrain.text = Managers.Definition.GetUIText(UITextKey.normalBrainTerm);
            _descriptionNormalBrain.text = Managers.Definition.GetUIText(UITextKey.normalBrainDescription);
            _termIntellect.text = Managers.Definition.GetUIText(UITextKey.intellectTerm);
            _descriptionIntellect.text = Managers.Definition.GetUIText(UITextKey.intellectDescription);
            _termChannel.text = Managers.Definition.GetUIText(UITextKey.channelTerm);
            _descriptionChannel.text = Managers.Definition.GetUIText(UITextKey.channelDescription);
            _termBrainNetwork.text = Managers.Definition.GetUIText(UITextKey.brainNetworkTerm);
            _descriptionBrainNetwork.text = Managers.Definition.GetUIText(UITextKey.brainNetworkDescription);
            _termDecomposition.text = Managers.Definition.GetUIText(UITextKey.decompositionTerm);
            _descriptionDecomposition.text = Managers.Definition.GetUIText(UITextKey.decompositionDescription);
            _termIntellectLimit.text = Managers.Definition.GetUIText(UITextKey.intellectLimitTerm);
            _descriptionIntellectLimit.text = Managers.Definition.GetUIText(UITextKey.intellectLimitDescription);
            _termMultiplier.text = Managers.Definition.GetUIText(UITextKey.multiplierTerm);
            _descriptionMultiplier.text = Managers.Definition.GetUIText(UITextKey.multiplierDescription);
            _termDepth.text = Managers.Definition.GetUIText(UITextKey.depthTerm);
            _descriptionDepth.text = Managers.Definition.GetUIText(UITextKey.depthDescription);
            _termExperimentGoal.text = Managers.Definition.GetUIText(UITextKey.experimentGoalTerm);
            _descriptionExperimentGoal.text = Managers.Definition.GetUIText(UITextKey.experimentGoalDescription);
            _termReset.text = Managers.Definition.GetUIText(UITextKey.resetTerm);
            _descriptionReset.text = Managers.Definition.GetUIText(UITextKey.resetDescription);
            _termNP.text = Managers.Definition.GetUIText(UITextKey.npTerm);
            _descriptionNP.text = Managers.Definition.GetUIText(UITextKey.npDescription);
            _termTP.text = Managers.Definition.GetUIText(UITextKey.tpTerm);
            _descriptionTP.text = Managers.Definition.GetUIText(UITextKey.tpDescription);
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
