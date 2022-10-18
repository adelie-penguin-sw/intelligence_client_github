using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextKey
{
    #region LoginSceneUIText
    public const string setUserNameText = "SetUserNameText";
    public const string userNameExistText = "UserNameExistText";
    #endregion


    #region DropDownMenuButtonText
    public const string dropDownMenuItemTextStatistics = "DropDownMenuItemText_Statistics";
    public const string dropDownMenuItemTextOptions = "DropDownMenuItemText_Options";
    public const string dropDownMenuItemTextHelp = "DropDownMenuItemText_Help";
    public const string dropDownMenuItemTextAbout = "DropDownMenuItemText_About";
    #endregion


    #region HelpPopupText
    public const string helpPopupTitleText = "HelpPopupTitleText";
    public const string helpPopupTabButtonTextHowToPlay = "HelpPopupTabButtonText_HowToPlay";
    public const string helpPopupTabButtonTextTerms = "HelpPopupTabButtonText_Terms";
    public const string howToExploreNetworkTitle = "HowToExploreNetworkTitle";
    public const string howToExploreNetworkDescription = "HowToExploreNetworkDescription";
    public const string howToCreateBrainTitle = "HowToCreateBrainTitle";
    public const string howToCreateBrainDescription = "HowToCreateBrainDescription";
    public const string howToCreateChannelTitle = "HowToCreateChannelTitle";
    public const string howToCreateChannelDescription = "HowToCreateChannelDescription";
    public const string howToViewBrainInfoTitle = "HowToViewBrainInfoTitle";
    public const string howToViewBrainInfoDescription = "HowToViewBrainInfoDescription";
    public const string experimentTerm = "ExperimentTerm";
    public const string experimentDescription = "ExperimentDescription";
    public const string coreBrainTerm = "CoreBrainTerm";
    public const string coreBrainDescription = "CoreBrainDescription";
    public const string normalBrainTerm = "NormalBrainTerm";
    public const string normalBrainDescription = "NormalBrainDescription";
    public const string intellectTerm = "IntellectTerm";
    public const string intellectDescription = "IntellectDescription";
    public const string channelTerm = "ChannelTerm";
    public const string channelDescription = "ChannelDescription";
    public const string brainNetworkTerm = "BrainNetworkTerm";
    public const string brainNetworkDescription = "BrainNetworkDescription";
    public const string decompositionTerm = "DecompositionTerm";
    public const string decompositionDescription = "DecompositionDescription";
    public const string intellectLimitTerm = "IntellectLimitTerm";
    public const string intellectLimitDescription = "IntellectLimitDescription";
    public const string multiplierTerm = "MultiplierTerm";
    public const string multiplierDescription = "MultiplierDescription";
    public const string depthTerm = "DepthTerm";
    public const string depthDescription = "DepthDescription";
    public const string experimentGoalTerm = "ExperimentGoalTerm";
    public const string experimentGoalDescription = "ExperimentGoalDescription";
    public const string resetTerm = "ResetTerm";
    public const string resetDescription = "ResetDescription";
    public const string npTerm = "NPTerm";
    public const string npDescription = "NPDescription";
    public const string tpTerm = "TPTerm";
    public const string tpDescription = "TPDescription";
    #endregion


    #region ShowCostUIText
    public const string costText = "CostText";
    public const string tooCloseText = "TooCloseText";
    public const string remainingFreeBrainsText = "RemainingFreeBrainsText";
    public const string selectBrainText = "SelectBrainText";
    public const string notFromCoreText = "NotFromCoreText";
    public const string notToIsolatedText = "NotToIsolatedText";
    public const string alreadyHasChannelText = "AlreadyHasChannelText";
    public const string maxDepthExceededText = "MaxDepthExceededText";
    public const string allReceiverIdenticalText = "AllReceiverIdenticalText";
    public const string notLoopText = "NotLoopText";
    public const string alreadyConnectionText = "AlreadyConnectionText";
    #endregion


    #region BrainInfoPopupText
    public const string brainInfoPopupTitleText = "BrainInfoPopupTitleText";
    public const string brainInfoIDTitle = "BrainInfoIDTitle";
    public const string brainInfoTypeTitle = "BrainInfoTypeTitle";
    public const string brainInfoTypeCoreBrain = "BrainInfoType_CoreBrain";
    public const string brainInfoTypeNormalBrain = "BrainInfoType_NormalBrain";
    public const string brainInfoIntellectTitle = "BrainInfoIntellectTitle";
    public const string brainInfoIntellectLimitTitle = "BrainInfoIntellectLimitTitle";
    public const string brainInfoMultiplierTitle = "BrainInfoMultiplierTitle";
    public const string brainInfoStoredNPTitle = "BrainInfoStoredNPTitle";
    public const string brainInfoDistanceTitle = "BrainInfoDistanceTitle";
    public const string brainInfoStatusTitle = "BrainInfoStatusTitle";
    public const string brainInfoStatusNotConnected = "BrainInfoStatus_NotConnected";
    public const string brainInfoStatusIdle = "BrainInfoStatus_Idle";
    public const string brainInfoStatusWorking = "BrainInfoStatus_Working";
    public const string brainInfoStatusLimitExceeded = "BrainInfoStatus_LimitExceeded";
    public const string brainInfoResetButtonText = "BrainInfoResetButtonText";
    public const string brainInfoUpgradeIntellectButtonText = "BrainInfoUpgradeIntellectButtonText";
    public const string brainInfoUpgradeMultiplierButtonText = "BrainInfoUpgradeMultiplierButtonText";
    public const string brainInfoUpgradeLimitButtonText = "BrainInfoUpgradeLimitButtonText";
    public const string brainInfoDecomposeButtonTextNoSender = "BrainInfoDecomposeButtonText_NoSender";
    public const string brainInfoDecomposeButtonTextHasSender = "BrainInfoDecomposeButtonText_HasSender";
    #endregion


    #region ResetPopupText
    public const string resetPopupResetButtonText = "ResetPopupResetButtonText";
    public const string resetPopupCancelButtonText = "ResetPopupCancelButtonText";
    public const string resetPopupCompleteTitleText = "ResetPopupCompleteTitleText";
    public const string resetPopupIncompleteTitleText = "ResetPopupIncompleteTitleText";
    public const string resetPopupCompleteHeaderText = "ResetPopupCompleteHeaderText";
    public const string resetPopupIncompleteHeaderText = "ResetPopupIncompleteHeaderText";
    public const string resetPopupExperimentLevelKey = "ResetPopupExperimentLevelKey";
    public const string resetPopupExperimentLevelValue = "ResetPopupExperimentLevelValue";
    public const string resetPopupAttemptsCompleteKey = "ResetPopupAttemptsCompleteKey";
    public const string resetPopupAttemptsCompleteValue = "ResetPopupAttemptsCompleteValue";
    public const string resetPopupAttemptsIncompleteKey = "ResetPopupAttemptsIncompleteKey";
    public const string resetPopupAttemptsIncompleteValue = "ResetPopupAttemptsIncompleteValue";
    public const string resetPopupGoalKey = "ResetPopupGoalKey";
    public const string resetPopupGoalValue = "ResetPopupGoalValue";
    public const string resetPopupElapsedTimeCompleteKey = "ResetPopupElapsedTimeCompleteKey";
    public const string resetPopupElapsedTimeCompleteValue = "ResetPopupElapsedTimeCompleteValue";
    public const string resetPopupIntellectIncompleteKey = "ResetPopupIntellectIncompleteKey";
    public const string resetPopupIntellectIncompleteValue = "ResetPopupIntellectIncompleteValue";
    public const string resetPopupMultiplierKey = "ResetPopupMultiplierKey";
    public const string resetPopupMultiplierValue = "ResetPopupMultiplierValue";
    public const string resetPopupTPKey = "ResetPopupTPKey";
    public const string resetPopupTPValue = "ResetPopupTPValue";
    public const string resetPopupCompleteMessage = "ResetPopupCompleteMessage";
    public const string resetPopupIncompleteMessage = "ResetPopupIncompleteMessage";
    #endregion


    #region UserInfoPopupText
    public const string userInfoPopupTitleText = "UserInfoPopupTitleText";
    public const string userInfoPopupExperimentLevelKey = "UserInfoPopupExperimentLevelKey";
    public const string userInfoPopupExperimentLevelValue = "UserInfoPopupExperimentLevelValue";
    public const string userInfoPopupAttemptsKey = "UserInfoPopupAttemptsKey";
    public const string userInfoPopupAttemptsValue = "UserInfoPopupAttemptsValue";
    public const string userInfoPopupCoreIntellectKey = "UserInfoPopupCoreIntellectKey";
    public const string userInfoPopupCoreIntellectValue = "UserInfoPopupCoreIntellectValue";
    public const string userInfoPopupNPKey = "UserInfoPopupNPKey";
    public const string userInfoPopupNPValue = "UserInfoPopupNPValue";
    public const string userInfoPopupTPKey = "UserInfoPopupTPKey";
    public const string userInfoPopupTPValue = "UserInfoPopupTPValue";
    public const string userInfoPopupTotalBrainGenCountKey = "UserInfoPopupTotalBrainGenCountKey";
    public const string userInfoPopupTotalBrainGenCountValue = "UserInfoPopupTotalBrainGenCountValue";
    public const string userInfoPopupMaxDepthKey = "UserInfoPopupMaxDepthKey";
    public const string userInfoPopupMaxDepthValue = "UserInfoPopupMaxDepthValue";
    #endregion
}
