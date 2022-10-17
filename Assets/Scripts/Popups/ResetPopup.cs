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

        [SerializeField] private GameObject _textGroupComplete;
        [SerializeField] private GameObject _textGroupIncomplete;
        [SerializeField] private GameObject _cancelButton;

        [SerializeField] private TextMeshProUGUI _expLvTextComplete;
        [SerializeField] private TextMeshProUGUI _attemptsTextComplete;
        [SerializeField] private TextMeshProUGUI _expGoalTextComplete;
        [SerializeField] private TextMeshProUGUI _elapesdTimeTextComplete;
        [SerializeField] private TextMeshProUGUI _multiplierRewardTextComplete;
        [SerializeField] private TextMeshProUGUI _tpRewardTextComplete;

        [SerializeField] private TextMeshProUGUI _expLvTextIncomplete;
        [SerializeField] private TextMeshProUGUI _attemptsTextIncomplete;
        [SerializeField] private TextMeshProUGUI _expGoalTextIncomplete;
        [SerializeField] private TextMeshProUGUI _currentCoreIntellectTextIncomplete;
        [SerializeField] private TextMeshProUGUI _multiplierRewardTextIncomplete;
        [SerializeField] private TextMeshProUGUI _tpRewardTextIncomplete;

        [SerializeField] private GameObject _multiplierRewardComplete;
        [SerializeField] private GameObject _multiplierRewardIncomplete;

        private Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

        public override void Init()
        {
            base.Init();

            _expGoalTextComplete.text = UserData.ExpGoalStr;
            _expGoalTextIncomplete.text = UserData.ExpGoalStr;

            _multiplierRewardComplete.SetActive(UserData.TPUpgrades[0].UpgradeCount > 0);
            _multiplierRewardIncomplete.gameObject.SetActive(UserData.TPUpgrades[0].UpgradeCount > 0);
        }

        private Hashtable _sendData = new Hashtable();
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            bool complete = InGame.InGameManager.IsCompleteExp;

            _textGroupComplete.SetActive(complete);
            _textGroupIncomplete.SetActive(!complete);
            _cancelButton.SetActive(!complete);

            _expLvTextComplete.text = $"Lv. {UserData.ExperimentLevel}";
            _attemptsTextComplete.text = $"{UserData.ResetCounts[UserData.ExperimentLevel] + 1} Attempts";

            if (complete)
            {
                _titleText.text = "Experiment Complete";

                long elapsedSecsNano = DateTimeOffset.Now.ToUnixTimeMilliseconds() * 1000000 - UserData.ExperimentStartTime;
                long elapsedSecs = elapsedSecsNano / 1000000000;
                long elapsedMins = elapsedSecs / 60;
                elapsedSecs %= 60;
                long elapsedHours = elapsedMins / 60;
                elapsedMins %= 60;
                long elapsedDays = elapsedHours / 24;
                elapsedHours %= 24;
                _elapesdTimeTextComplete.text = string.Format("{0:D4}d {0:D2}h {1:D2}m {2:D2}s", elapsedDays, elapsedHours, elapsedMins, elapsedSecs);

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                inputMap.Add("tpu027", new UpArrowNotation(UserData.TPUpgrades[27].UpgradeCount));
                _tpRewardTextComplete.text = Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.tpRewardForReset).ToString(ECurrencyType.TP) + " TP";

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                inputMap.Add("tpu004", new UpArrowNotation(UserData.TPUpgrades[4].UpgradeCount));
                _multiplierRewardTextComplete.text = "x" + Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.multiplierRewardForReset).ToString(ECurrencyType.MULTIPLIER);
            }
            else
            {
                _titleText.text = "Reset Now?";

                _currentCoreIntellectTextIncomplete.text = UserData.CoreIntellect.ToString();

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                inputMap.Add("tpu027", new UpArrowNotation(UserData.TPUpgrades[27].UpgradeCount));
                _tpRewardTextIncomplete.text = Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.tpRewardForReset).ToString(ECurrencyType.TP) + " TP";

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                inputMap.Add("tpu004", new UpArrowNotation(UserData.TPUpgrades[4].UpgradeCount));
                _multiplierRewardTextIncomplete.text = "x" + Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.multiplierRewardForReset).ToString(ECurrencyType.MULTIPLIER);
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