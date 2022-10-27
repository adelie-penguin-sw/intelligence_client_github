using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace MainTab
{
    public class ResetPopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _titleText;

        [SerializeField] private TextMeshProUGUI _headerTextComplete;
        [SerializeField] private TextMeshProUGUI _headerTextIncomplete;

        [SerializeField] private GameObject _textGroupComplete;
        [SerializeField] private GameObject _textGroupIncomplete;
        [SerializeField] private GameObject _cancelButton;

        [SerializeField] private TextMeshProUGUI _resetButtonText;
        [SerializeField] private TextMeshProUGUI _cancelButtonText;

        [SerializeField] private TextMeshProUGUI _expLvKeyComplete;
        [SerializeField] private TextMeshProUGUI _attemptsKeyComplete;
        [SerializeField] private TextMeshProUGUI _expGoalKeyComplete;
        [SerializeField] private TextMeshProUGUI _elapesdTimeKeyComplete;
        [SerializeField] private TextMeshProUGUI _multiplierRewardKeyComplete;
        [SerializeField] private TextMeshProUGUI _tpRewardKeyComplete;
        [SerializeField] private TextMeshProUGUI _npCarryOverKeyComplete;

        [SerializeField] private TextMeshProUGUI _expLvTextComplete;
        [SerializeField] private TextMeshProUGUI _attemptsTextComplete;
        [SerializeField] private TextMeshProUGUI _expGoalTextComplete;
        [SerializeField] private TextMeshProUGUI _elapesdTimeTextComplete;
        [SerializeField] private TextMeshProUGUI _multiplierRewardTextComplete;
        [SerializeField] private TextMeshProUGUI _tpRewardTextComplete;
        [SerializeField] private TextMeshProUGUI _npCarryOverTextComplete;

        [SerializeField] private TextMeshProUGUI _expLvKeyIncomplete;
        [SerializeField] private TextMeshProUGUI _attemptsKeyIncomplete;
        [SerializeField] private TextMeshProUGUI _expGoalKeyIncomplete;
        [SerializeField] private TextMeshProUGUI _currentCoreIntellectKeyIncomplete;
        [SerializeField] private TextMeshProUGUI _multiplierRewardKeyIncomplete;
        [SerializeField] private TextMeshProUGUI _tpRewardKeyIncomplete;
        [SerializeField] private TextMeshProUGUI _npCarryOverKeyIncomplete;

        [SerializeField] private TextMeshProUGUI _expLvTextIncomplete;
        [SerializeField] private TextMeshProUGUI _attemptsTextIncomplete;
        [SerializeField] private TextMeshProUGUI _expGoalTextIncomplete;
        [SerializeField] private TextMeshProUGUI _currentCoreIntellectTextIncomplete;
        [SerializeField] private TextMeshProUGUI _multiplierRewardTextIncomplete;
        [SerializeField] private TextMeshProUGUI _tpRewardTextIncomplete;
        [SerializeField] private TextMeshProUGUI _npCarryOverTextIncomplete;

        [SerializeField] private TextMeshProUGUI _messageComplete;
        [SerializeField] private TextMeshProUGUI _messageIncomplete;

        [SerializeField] private GameObject _multiplierRewardComplete;
        [SerializeField] private GameObject _multiplierRewardIncomplete;

        [SerializeField] private GameObject _npCarryOverComplete;
        [SerializeField] private GameObject _npCarryOverIncomplete;

        private BrainNetwork _brainNetwork;

        private Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

        public void Init(BrainNetwork brainNetowrk)
        {
            base.Init();

            _headerTextComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupCompleteHeaderText);
            _headerTextIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupIncompleteHeaderText);

            _resetButtonText.text = Managers.Definition.GetUIText(UITextKey.resetPopupResetButtonText);
            _cancelButtonText.text = Managers.Definition.GetUIText(UITextKey.resetPopupCancelButtonText);

            _expLvKeyComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupExperimentLevelKey);
            _attemptsKeyComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupAttemptsCompleteKey);
            _expGoalKeyComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupGoalKey);
            _elapesdTimeKeyComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupElapsedTimeCompleteKey);
            _multiplierRewardKeyComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupMultiplierKey);
            _tpRewardKeyComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupTPKey);

            _expLvKeyIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupExperimentLevelKey);
            _attemptsKeyIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupAttemptsIncompleteKey);
            _expGoalKeyIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupGoalKey);
            _currentCoreIntellectKeyIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupIntellectIncompleteKey);
            _multiplierRewardKeyIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupMultiplierKey);
            _tpRewardKeyIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupTPKey);

            _messageComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupCompleteMessage);
            _messageIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupIncompleteMessage);

            _expGoalTextComplete.text = UserData.ExpGoalStr;
            _expGoalTextIncomplete.text = UserData.ExpGoalStr;

            _multiplierRewardComplete.SetActive(UserData.TPUpgrades[0].UpgradeCount > 0);
            _multiplierRewardIncomplete.SetActive(UserData.TPUpgrades[0].UpgradeCount > 0);

            _npCarryOverComplete.SetActive(UserData.TPUpgrades[25].UpgradeCount > 0);
            _npCarryOverIncomplete.SetActive(UserData.TPUpgrades[25].UpgradeCount > 0);

            _brainNetwork = brainNetowrk;
        }

        private Hashtable _sendData = new Hashtable();
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            bool complete = InGame.InGameManager.IsCompleteExp;

            _textGroupComplete.SetActive(complete);
            _textGroupIncomplete.SetActive(!complete);
            _cancelButton.SetActive(!complete);

            _expLvTextComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupExperimentLevelValue, UserData.ExperimentLevel.ToString());
            _attemptsTextComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupAttemptsCompleteValue, (UserData.ResetCounts[UserData.ExperimentLevel] + 1).ToString());
            _expLvTextIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupExperimentLevelValue, UserData.ExperimentLevel.ToString());
            _attemptsTextIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupAttemptsIncompleteValue, (UserData.ResetCounts[UserData.ExperimentLevel] + 1).ToString());

            inputMap.Clear();
            inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
            inputMap.Add("tpu027", new UpArrowNotation(UserData.TPUpgrades[27].UpgradeCount));
            string tpStr = Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.tpRewardForReset).ToString(ECurrencyType.TP);
            _tpRewardTextComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupTPValue, tpStr);
            _tpRewardTextIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupTPValue, tpStr);

            inputMap.Clear();
            inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
            inputMap.Add("tpu004", new UpArrowNotation(UserData.TPUpgrades[4].UpgradeCount));
            string multStr = Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.multiplierRewardForReset).ToString(ECurrencyType.MULTIPLIER);
            _multiplierRewardTextComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupMultiplierValue, multStr);
            _multiplierRewardTextIncomplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupMultiplierValue, multStr);

            _npCarryOverTextComplete.text = _brainNetwork.GetNPCarryOver().ToString(ECurrencyType.NP) + " NP";
            _npCarryOverTextIncomplete.text = _brainNetwork.GetNPCarryOver().ToString(ECurrencyType.NP) + "NP";

            if (complete)
            {
                _titleText.text = Managers.Definition.GetUIText(UITextKey.resetPopupCompleteTitleText);

                long elapsedSecsNano = DateTimeOffset.Now.ToUnixTimeMilliseconds() * 1000000 - UserData.ExperimentStartTime;
                long elapsedSecs = elapsedSecsNano / 1000000000;
                long elapsedMins = elapsedSecs / 60;
                elapsedSecs %= 60;
                long elapsedHours = elapsedMins / 60;
                elapsedMins %= 60;
                long elapsedDays = elapsedHours / 24;
                elapsedHours %= 24;
                _elapesdTimeTextComplete.text = Managers.Definition.GetUIText(UITextKey.resetPopupElapsedTimeCompleteValue, elapsedDays.ToString(), elapsedHours.ToString(), elapsedMins.ToString(), elapsedSecs.ToString());
            }
            else
            {
                _titleText.text = Managers.Definition.GetUIText(UITextKey.resetPopupIncompleteTitleText);

                _currentCoreIntellectTextIncomplete.text = UserData.CoreIntellect.ToString();
            }
        }

        public async void OnClick_Reset()
        {
            if (await Managers.Network.API_NetworkReset())
            {
                Managers.Notification.PostNotification(ENotiMessage.ONCLICK_RESET_NETWORK);
                Dispose();
            }
        }
    }
}