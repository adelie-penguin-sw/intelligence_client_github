using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInfoPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI _txtUserName;
    [SerializeField] private TextMeshProUGUI _txtExperimentLevel;
    [SerializeField] private TextMeshProUGUI _txtAttempts;
    [SerializeField] private TextMeshProUGUI _txtCoreIntellect;
    [SerializeField] private TextMeshProUGUI _txtNP;
    [SerializeField] private TextMeshProUGUI _txtTP;
    [SerializeField] private TextMeshProUGUI _txtTotalBrainGenCount;
    [SerializeField] private TextMeshProUGUI _txtMaxDepth;

    public override void Init()
    {
        base.Init();
        _txtUserName.text = UserData.Username;
        _txtExperimentLevel.text = $"Lv. {UserData.ExperimentLevel}";
        _txtAttempts.text = $"{UserData.ResetCounts[UserData.ExperimentLevel]}회";
        _txtCoreIntellect.text = UserData.CoreIntellect.ToString(ECurrencyType.INTELLECT);
        _txtNP.text = $"{UserData.NP.ToString(ECurrencyType.NP)} NP";
        _txtTP.text = $"{UserData.TP.ToString(ECurrencyType.TP)} TP";
        _txtTotalBrainGenCount.text = $"{(UserData.TotalBrainGenCount - 1).ToString()}회";
        _txtMaxDepth.text = $"{UserData.MaxDepth.ToString()}단";
    }
}
