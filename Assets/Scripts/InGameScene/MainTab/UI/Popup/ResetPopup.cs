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

        public override void Init()
        {
            base.Init();

            _expGoalTextComplete.text = "10^20";        // 실제 값 써야함!!
            _expGoalTextIncomplete.text = "10^20";      // 실제 값 써야함!!
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
                // 이거는 실험 레벨이랑 해당 레벨 실험 완료할때까지 시도 횟수 들어가야함!!
                _expLvTextComplete.text = string.Format("You've just completed\n<b>Lv.{0} experiment</b>\nafter <b>{1} attempt(s).</b>", 1, 1);

                // 해당 레벨의 실험을 최초 시작하고부터 성공하기까지 소요된 총 시간, 분, 초가 들어가야함!!
                _elapesdTimeTextComplete.text = string.Format("0000h 00m 00s");

                // TP리워드 계산해서 넣어줘야함!!
                _tpRewardTextComplete.text = string.Format("{0} TP", 1);
            }
            else
            {
                // 현재 코어 브레인 지능 들어가야함!!
                _currentCoreIntellectTextIncomplete.text = "0";

                // TP리워드 계산해서 넣어줘야함!!
                _tpRewardTextIncomplete.text = string.Format("{0} TP", 0);
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            NotificationManager.Instance.PostNotification(ENotiMessage.CLOSE_RESET_POPUP);
        }

        public async void OnClick_Reset()
        {
            var res = await NetworkManager.Instance.API_NetworkReset();
            if (res != null)
            {
                Hashtable sendData = new Hashtable();
                sendData.Add(EDataParamKey.SINGLE_NETWORK_WRAPPER, new SingleNetworkWrapper(res));
                NotificationManager.Instance.PostNotification(ENotiMessage.ONCLICK_RESET_NETWORK, sendData);
                Dispose();
            }
        }

    }
}