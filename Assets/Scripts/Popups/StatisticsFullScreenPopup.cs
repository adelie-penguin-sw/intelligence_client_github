using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace InGame
{
    public class StatisticsFullScreenPopup : PopupBase
    {
        [SerializeField] private TextMeshProUGUI _title;

        [SerializeField] private TextMeshProUGUI _gameStartTimeKey;
        [SerializeField] private TextMeshProUGUI _totalPlayTimeKey;
        [SerializeField] private TextMeshProUGUI _totalAttemptsKey;
        [SerializeField] private TextMeshProUGUI _experimentLevelKey;
        [SerializeField] private TextMeshProUGUI _experimentStartTimeKey;
        [SerializeField] private TextMeshProUGUI _experimentPlayTimeKey;
        [SerializeField] private TextMeshProUGUI _currentAttemptsKey;
        [SerializeField] private TextMeshProUGUI _currentCoreIntellectKey;
        [SerializeField] private TextMeshProUGUI _highestCoreIntellectKey;
        [SerializeField] private TextMeshProUGUI _baseMultiplierKey;
        [SerializeField] private TextMeshProUGUI _totalGeneratedBrainsKey;
        [SerializeField] private TextMeshProUGUI _totalGeneratedChannelsKey;
        [SerializeField] private TextMeshProUGUI _totalDecomposedBrainsKey;
        [SerializeField] private TextMeshProUGUI _totalTapCountKey;
        [SerializeField] private TextMeshProUGUI _totalGeneratedASNsKey;
        [SerializeField] private TextMeshProUGUI _totalNPGainKey;
        [SerializeField] private TextMeshProUGUI _totalTPGainKey;

        [SerializeField] private TextMeshProUGUI _gameStartTimeValue;
        [SerializeField] private TextMeshProUGUI _totalPlayTimeValue;
        [SerializeField] private TextMeshProUGUI _totalAttemptsValue;
        [SerializeField] private TextMeshProUGUI _experimentLevelValue;
        [SerializeField] private TextMeshProUGUI _experimentStartTimeValue;
        [SerializeField] private TextMeshProUGUI _experimentPlayTimeValue;
        [SerializeField] private TextMeshProUGUI _currentAttemptsValue;
        [SerializeField] private TextMeshProUGUI _currentCoreIntellectValue;
        [SerializeField] private TextMeshProUGUI _highestCoreIntellectValue;
        [SerializeField] private TextMeshProUGUI _baseMultiplierValue;
        [SerializeField] private TextMeshProUGUI _totalGeneratedBrainsValue;
        [SerializeField] private TextMeshProUGUI _totalGeneratedChannelsValue;
        [SerializeField] private TextMeshProUGUI _totalDecomposedBrainsValue;
        [SerializeField] private TextMeshProUGUI _totalTapCountValue;
        [SerializeField] private TextMeshProUGUI _totalGeneratedASNsValue;
        [SerializeField] private TextMeshProUGUI _totalNPGainValue;
        [SerializeField] private TextMeshProUGUI _totalTPGainValue;

        public override void Init()
        {
            base.Init();

            // 팝업 제목
            _title.text = Managers.Definition.GetUIText(UITextKey.statisticsPopupTitleText);

            // 속성값 이름
            _gameStartTimeKey.text = Managers.Definition.GetUIText(UITextKey.statisticsGameStartTimeKey);
            _totalPlayTimeKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalPlayTimeKey);
            _totalAttemptsKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalAttemptsKey);
            _experimentLevelKey.text = Managers.Definition.GetUIText(UITextKey.statisticsExperimentLevelKey);
            _experimentStartTimeKey.text = Managers.Definition.GetUIText(UITextKey.statisticsExperimentStartTimeKey);
            _experimentPlayTimeKey.text = Managers.Definition.GetUIText(UITextKey.statisticsExperimentPlayTimeKey);
            _currentAttemptsKey.text = Managers.Definition.GetUIText(UITextKey.statisticsCurrentAttemptsKey);
            _currentCoreIntellectKey.text = Managers.Definition.GetUIText(UITextKey.statisticsCurrentCoreIntellectKey);
            _highestCoreIntellectKey.text = Managers.Definition.GetUIText(UITextKey.statisticsHighestCoreIntellectKey);
            _baseMultiplierKey.text = Managers.Definition.GetUIText(UITextKey.statisticsBaseMultiplierKey);
            _totalGeneratedBrainsKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalGeneratedBrainsKey);
            _totalGeneratedChannelsKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalGeneratedChannelsKey);
            _totalDecomposedBrainsKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalDecomposedBrainsKey);
            _totalTapCountKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalTapCountKey);
            _totalGeneratedASNsKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalGeneratedASNsKey);
            _totalNPGainKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalNPGainKey);
            _totalTPGainKey.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalTPGainKey);

            // 속성값 중 실시간 업데이트 필요없는거
            _gameStartTimeValue.text = Managers.Definition.GetUIText(UITextKey.statisticsGameStartTimeValue, "0", "0", "0", "0", "0", "0");     // 정보 따로 필요함
            _totalAttemptsValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalAttemptsValue, (UserData.ResetCounts.Sum() + 1).ToString());
            _experimentLevelValue.text = Managers.Definition.GetUIText(UITextKey.statisticsExperimentLevelValue, UserData.ExperimentLevel.ToString());
            System.DateTime dt = new System.DateTime(UserData.ExperimentStartTime / 100);
            _experimentStartTimeValue.text = Managers.Definition.GetUIText(UITextKey.statisticsExperimentStartTimeValue, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), dt.Hour.ToString(), dt.Minute.ToString(), dt.Second.ToString());
            _currentAttemptsValue.text = Managers.Definition.GetUIText(UITextKey.statisticsCurrentAttemptsValue, UserData.ResetCounts[UserData.ExperimentLevel].ToString());
            _baseMultiplierValue.text = Managers.Definition.GetUIText(UITextKey.statisticsBaseMultiplierValue, "1");     // 계산 따로 해야함
            _totalGeneratedBrainsValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalGeneratedBrainsValue, "0");     // 정보 따로 필요함
            _totalGeneratedChannelsValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalGeneratedChannelsValue, "0");     // 정보 따로 필요함
            _totalDecomposedBrainsValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalDecomposedBrainsValue, "0");       // 정보 따로 필요함
            _totalTapCountValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalTapCountValue, "0");       // 정보 따로 필요함
            _totalGeneratedASNsValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalGeneratedASNsValue, "0");     // 정보 따로 필요함
            _totalNPGainValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalNPGainValue, "0");       // 정보 따로 필요함
            _totalTPGainValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalTPGainValue, "0");       // 정보 따로 필요함
        }

        public override void Set()
        {
            base.Set();
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            // 속성값 중 실시간 업데이트 해야되는거
            _totalPlayTimeValue.text = Managers.Definition.GetUIText(UITextKey.statisticsTotalPlayTimeValue, "0", "0", "0", "0");       // 정보 따로 필요함
            _experimentPlayTimeValue.text = Managers.Definition.GetUIText(UITextKey.statisticsExperimentPlayTimeValue, "0", "0", "0", "0");     // 계산 따로 해야함
            _currentCoreIntellectValue.text = Managers.Definition.GetUIText(UITextKey.statisticsCurrentCoreIntellectValue, UserData.CoreIntellect.ToString(ECurrencyType.INTELLECT));
            _highestCoreIntellectValue.text = Managers.Definition.GetUIText(UITextKey.statisticsHighestCoreIntellectValue, "0");       // 정보 따로 필요함
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
