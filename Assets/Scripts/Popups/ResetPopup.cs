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
            //TODO: ????????? ???????????? Popup??? csv????????? ?????? ???????????? text??? ????????? ??????????????? ????????? ????????? ???????????? ?????? ??????????????? ????????????
            //???????????? ?????? JIRA ????????? ??????
            _headerTextComplete.text = Managers.Definition.GetTextData(13005);
            _headerTextIncomplete.text = Managers.Definition.GetTextData(13006);

            _resetButtonText.text = Managers.Definition.GetTextData(13001);
            _cancelButtonText.text = Managers.Definition.GetTextData(13002);

            //TODO: ????????? ?????? ????????? ?????? ???????????? ??????????????? ?????? text??? 2?????? ???????????? ?????? ?????? ???????????? ?????????Managers.Definition.GetTextData(13002);

            //?????? ?????????
            _expLvKeyComplete.text = Managers.Definition.GetTextData(13007);
            _attemptsKeyComplete.text = Managers.Definition.GetTextData(13008);
            _expGoalKeyComplete.text = Managers.Definition.GetTextData(13010);
            _elapesdTimeKeyComplete.text = Managers.Definition.GetTextData(13011);
            _multiplierRewardKeyComplete.text = Managers.Definition.GetTextData(13013);
            _tpRewardKeyComplete.text = Managers.Definition.GetTextData(13014);

            //?????? ????????? ??? 
            _expLvKeyIncomplete.text = Managers.Definition.GetTextData(13007);
            _attemptsKeyIncomplete.text = Managers.Definition.GetTextData(13009);
            _expGoalKeyIncomplete.text = Managers.Definition.GetTextData(13010);
            _currentCoreIntellectKeyIncomplete.text = Managers.Definition.GetTextData(13012);
            _multiplierRewardKeyIncomplete.text = Managers.Definition.GetTextData(13013);
            _tpRewardKeyIncomplete.text = Managers.Definition.GetTextData(13014);

            _messageComplete.text = Managers.Definition.GetTextData(13015);
            _messageIncomplete.text = Managers.Definition.GetTextData(13016);

            _expGoalTextComplete.text = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[UserData.ExperimentLevel].ToString();
            _expGoalTextIncomplete.text = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[UserData.ExperimentLevel].ToString();

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

            bool complete = UserData.IsCompleteExp;

            _textGroupComplete.SetActive(complete);
            _textGroupIncomplete.SetActive(!complete);
            _cancelButton.SetActive(!complete);

            _expLvTextComplete.text = Managers.Definition.GetTextFormatData(8, UserData.ExperimentLevel.ToString());
            _attemptsTextComplete.text = Managers.Definition.GetTextFormatData(9, (UserData.ResetCounts[UserData.ExperimentLevel] + 1).ToString());
            _expLvTextIncomplete.text = Managers.Definition.GetTextFormatData(8, UserData.ExperimentLevel.ToString());
            _attemptsTextIncomplete.text = Managers.Definition.GetTextFormatData(9, (UserData.ResetCounts[UserData.ExperimentLevel] + 1).ToString());

            inputMap.Clear();
            inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
            inputMap.Add("tpu027", new UpArrowNotation(UserData.TPUpgrades[27].UpgradeCount));
            string tpStr = Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.tpRewardForReset).ToString(ECurrencyType.TP);
            _tpRewardTextComplete.text = Managers.Definition.GetTextFormatData(13, tpStr);
            _tpRewardTextIncomplete.text = Managers.Definition.GetTextFormatData(13, tpStr);

            inputMap.Clear();
            inputMap.Add("coreBrainIntellect", UserData.CoreIntellect);
            inputMap.Add("tpu004", new UpArrowNotation(UserData.TPUpgrades[4].UpgradeCount));
            string multStr = Managers.Definition.CalcEquationForKey(inputMap, DefinitionKey.multiplierRewardForReset).ToString(ECurrencyType.MULTIPLIER);
            _multiplierRewardTextComplete.text = Managers.Definition.GetTextFormatData(12, multStr);
            _multiplierRewardTextIncomplete.text = Managers.Definition.GetTextFormatData(12, multStr);

            _npCarryOverTextComplete.text = _brainNetwork.GetNPCarryOver().ToString(ECurrencyType.NP) + " NP";
            _npCarryOverTextIncomplete.text = _brainNetwork.GetNPCarryOver().ToString(ECurrencyType.NP) + "NP";

            _npCarryOverTextComplete.text = _brainNetwork.GetNPCarryOver().ToString(ECurrencyType.NP) + " NP";
            _npCarryOverTextIncomplete.text = _brainNetwork.GetNPCarryOver().ToString(ECurrencyType.NP) + "NP";

            if (complete)
            {
                _titleText.text = Managers.Definition.GetTextData(13003);

                long elapsedSecsNano = DateTimeOffset.Now.ToUnixTimeMilliseconds() * 1000000 - UserData.ExperimentStartTime;
                long elapsedSecs = elapsedSecsNano / 1000000000;
                long elapsedMins = elapsedSecs / 60;
                elapsedSecs %= 60;
                long elapsedHours = elapsedMins / 60;
                elapsedMins %= 60;
                long elapsedDays = elapsedHours / 24;
                elapsedHours %= 24;
                _elapesdTimeTextComplete.text = Managers.Definition.GetTextFormatData(11, elapsedDays.ToString(), elapsedHours.ToString(), elapsedMins.ToString(), elapsedSecs.ToString());
            }
            else
            {
                _titleText.text = Managers.Definition.GetTextData(13004);

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