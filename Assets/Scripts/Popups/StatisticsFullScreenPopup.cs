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
            _title.text = Managers.Definition.GetTextData(11001);

            // 속성값 이름
            _gameStartTimeKey.text = Managers.Definition.GetTextData(11002);
            _totalPlayTimeKey.text = Managers.Definition.GetTextData(11003);
            _totalAttemptsKey.text = Managers.Definition.GetTextData(11004);
            _experimentLevelKey.text = Managers.Definition.GetTextData(11005);
            _experimentStartTimeKey.text = Managers.Definition.GetTextData(11006);
            _experimentPlayTimeKey.text = Managers.Definition.GetTextData(11007);
            _currentAttemptsKey.text = Managers.Definition.GetTextData(11008);
            _currentCoreIntellectKey.text = Managers.Definition.GetTextData(11009);
            _highestCoreIntellectKey.text = Managers.Definition.GetTextData(11010);
            _baseMultiplierKey.text = Managers.Definition.GetTextData(11011);
            _totalGeneratedBrainsKey.text = Managers.Definition.GetTextData(11012);
            _totalGeneratedChannelsKey.text = Managers.Definition.GetTextData(11013);
            _totalDecomposedBrainsKey.text = Managers.Definition.GetTextData(11014);
            _totalTapCountKey.text = Managers.Definition.GetTextData(11015);
            _totalGeneratedASNsKey.text = Managers.Definition.GetTextData(11016);
            _totalNPGainKey.text = Managers.Definition.GetTextData(11017);
            _totalTPGainKey.text = Managers.Definition.GetTextData(11018);

            // 속성값 중 실시간 업데이트 필요없는거
            _gameStartTimeValue.text = Managers.Definition.GetTextFormatData(19, "0", "0", "0", "0", "0", "0");     // 정보 따로 필요함
            _totalAttemptsValue.text = Managers.Definition.GetTextFormatData(9, (UserData.ResetCounts.Sum() + 1).ToString());
            _experimentLevelValue.text = Managers.Definition.GetTextFormatData(8, UserData.ExperimentLevel.ToString());
            System.DateTime dt = new System.DateTime(UserData.ExperimentStartTime / 100);
            _experimentStartTimeValue.text = Managers.Definition.GetTextFormatData(19, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString(), dt.Hour.ToString(), dt.Minute.ToString(), dt.Second.ToString());
            _currentAttemptsValue.text = Managers.Definition.GetTextFormatData(9, UserData.ResetCounts[UserData.ExperimentLevel].ToString());
            _baseMultiplierValue.text = Managers.Definition.GetTextFormatData(12, "1");     // 계산 따로 해야함
            _totalGeneratedBrainsValue.text = Managers.Definition.GetTextFormatData(9, "0");     // 정보 따로 필요함
            _totalGeneratedChannelsValue.text = Managers.Definition.GetTextFormatData(9, "0");     // 정보 따로 필요함
            _totalDecomposedBrainsValue.text = Managers.Definition.GetTextFormatData(9, "0");       // 정보 따로 필요함
            _totalTapCountValue.text = Managers.Definition.GetTextFormatData(9, "0");       // 정보 따로 필요함
            _totalGeneratedASNsValue.text = Managers.Definition.GetTextFormatData(9, "0");     // 정보 따로 필요함
            _totalNPGainValue.text = Managers.Definition.GetTextFormatData(14, "0");       // 정보 따로 필요함
            _totalTPGainValue.text = Managers.Definition.GetTextFormatData(13, "0");       // 정보 따로 필요함
        }

        public override void Set()
        {
            base.Set();
        }

        public override void AdvanceTime(float dt_sec)
        {
            base.AdvanceTime(dt_sec);

            // 속성값 중 실시간 업데이트 해야되는거
            _totalPlayTimeValue.text = Managers.Definition.GetTextFormatData(16, "0", "0", "0", "0");       // 정보 따로 필요함
            _experimentPlayTimeValue.text = Managers.Definition.GetTextFormatData(16, "0", "0", "0", "0");     // 계산 따로 해야함
            _currentCoreIntellectValue.text = Managers.Definition.GetTextFormatData(10, UserData.CoreIntellect.ToString(ECurrencyType.INTELLECT));
            _highestCoreIntellectValue.text = Managers.Definition.GetTextFormatData(10, "0");       // 정보 따로 필요함
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
