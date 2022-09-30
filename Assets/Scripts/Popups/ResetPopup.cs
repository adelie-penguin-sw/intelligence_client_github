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
        [SerializeField] private TextMeshProUGUI _expGoalTextComplete;
        [SerializeField] private TextMeshProUGUI _elapesdTimeTextComplete;
        [SerializeField] private TextMeshProUGUI _tpRewardTextComplete;

        [SerializeField] private TextMeshProUGUI _currentCoreIntellectTextIncomplete;
        [SerializeField] private TextMeshProUGUI _expGoalTextIncomplete;
        [SerializeField] private TextMeshProUGUI _tpRewardTextIncomplete;

        private Dictionary<string, UpArrowNotation> inputMap = new Dictionary<string, UpArrowNotation>();

        public override void Init()
        {
            base.Init();

            _expGoalTextComplete.text = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[UserData.ExperimentLevel].ToString();
            _expGoalTextIncomplete.text = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[UserData.ExperimentLevel].ToString();
        }

        private Hashtable _sendData = new Hashtable();
        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            bool complete = InGame.InGameManager.IsCompleteExp;

            _textGroupComplete.SetActive(complete);
            _textGroupIncomplete.SetActive(!complete);
            _cancelButton.SetActive(!complete);

            if (complete)
            {
                _titleText.text = "Experiment Complete";
                _expLvTextComplete.text = string.Format("You've just completed\n<b>Lv.{0} experiment</b>\nafter <b>{1} attempt(s).</b>", UserData.ExperimentLevel, UserData.ResetCounts[UserData.ExperimentLevel] + 1);

                // 해당 레벨의 실험을 최초 시작하고부터 성공하기까지 소요된 총 시간, 분, 초가 들어가야함!!
                // 팝업 내에서 시간이 계속 흐르는 문제가 있음
                long elapsedSecsNano = DateTimeOffset.Now.ToUnixTimeMilliseconds() * 1000000 - UserData.ExperimentStartTime;
                long elapsedSecs = elapsedSecsNano / 1000000000;
                long elapsedMins = elapsedSecs / 60;
                elapsedSecs %= 60;
                long elapsedHours = elapsedMins / 60;
                elapsedMins %= 60;
                _elapesdTimeTextComplete.text = string.Format("{0:D4}h {1:D2}m {2:D2}s", elapsedHours, elapsedMins, elapsedSecs);

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                _tpRewardTextIncomplete.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpRewardForReset) + " TP";
            }
            else
            {
                _titleText.text = "Reset Network";

                _currentCoreIntellectTextIncomplete.text = UserData.CoreIntellect.ToString();

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                _tpRewardTextIncomplete.text = Managers.Definition.CalcEquationToString(inputMap, DefinitionKey.tpRewardForReset) + " TP";
            }
        }

        public async void OnClick_Reset()
        {
            var res = await Managers.Network.API_NetworkReset();
            if (res != null)
            {
                Hashtable sendData = new Hashtable();
                sendData.Add(EDataParamKey.SINGLE_NETWORK_WRAPPER, new SingleNetworkWrapper(res));
                Managers.Notification.PostNotification(ENotiMessage.ONCLICK_RESET_NETWORK, sendData);
                Dispose();
            }
        }

    }
}