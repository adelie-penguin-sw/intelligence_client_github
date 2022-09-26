using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

            _expGoalTextComplete.text = ((List<UpArrowNotation>)Managers.Definition["ExperimentGoalList"])[UserData.ExperimentLevel].ToString();
            _expGoalTextIncomplete.text = ((List<UpArrowNotation>)Managers.Definition["ExperimentGoalList"])[UserData.ExperimentLevel].ToString();
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

                // 이거는 실험 레벨이랑 해당 레벨 실험 완료할때까지 시도 횟수 들어가야함!!
                _expLvTextComplete.text = string.Format("You've just completed\n<b>Lv.{0} experiment</b>\nafter <b>{1} attempt(s).</b>", 1, 1);

                // 해당 레벨의 실험을 최초 시작하고부터 성공하기까지 소요된 총 시간, 분, 초가 들어가야함!!
                _elapesdTimeTextComplete.text = string.Format("0000h 00m 00s");

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                _tpRewardTextIncomplete.text = Managers.Definition.CalcEquation(inputMap, (string)Managers.Definition["TPrewardForReset"]) + " TP";
            }
            else
            {
                _titleText.text = "Reser Network";

                _currentCoreIntellectTextIncomplete.text = UserData.CoreIntellect.ToString();

                inputMap.Clear();
                inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
                _tpRewardTextIncomplete.text = Managers.Definition.CalcEquation(inputMap, (string)Managers.Definition["TPrewardForReset"]) + " TP";
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            Managers.Notification.PostNotification(ENotiMessage.CLOSE_RESET_POPUP);
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