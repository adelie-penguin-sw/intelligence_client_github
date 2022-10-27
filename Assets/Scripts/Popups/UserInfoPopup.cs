using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInfoPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI _txtUserName;

    [SerializeField] private TextMeshProUGUI _txtExperimentLevelKey;
    [SerializeField] private TextMeshProUGUI _txtAttemptsKey;
    [SerializeField] private TextMeshProUGUI _txtCoreIntellectKey;
    [SerializeField] private TextMeshProUGUI _txtNPKey;
    [SerializeField] private TextMeshProUGUI _txtTPKey;
    [SerializeField] private TextMeshProUGUI _txtTotalBrainGenCountKey;
    [SerializeField] private TextMeshProUGUI _txtMaxDepthKey;

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
        _txtUserName.text = UserData.username;

        _txtExperimentLevelKey.text = Managers.Definition.GetTextData(14002);
        _txtAttemptsKey.text = Managers.Definition.GetTextData(14003);
        _txtCoreIntellectKey.text = Managers.Definition.GetTextData(14004);
        _txtNPKey.text = Managers.Definition.GetTextData(14005);
        _txtTPKey.text = Managers.Definition.GetTextData(14006);
        _txtTotalBrainGenCountKey.text = Managers.Definition.GetTextData(14007);
        _txtMaxDepthKey.text = Managers.Definition.GetTextData(14008);

        _txtExperimentLevel.text = Managers.Definition.GetTextFormatData(8, UserData.ExperimentLevel.ToString());
        _txtAttempts.text = Managers.Definition.GetTextFormatData(9, UserData.ResetCounts[UserData.ExperimentLevel].ToString());
        _txtCoreIntellect.text = Managers.Definition.GetTextFormatData(10, UserData.CoreIntellect.ToString(ECurrencyType.INTELLECT));
        _txtNP.text = Managers.Definition.GetTextFormatData(14, UserData.NP.ToString(ECurrencyType.NP));
        _txtTP.text = Managers.Definition.GetTextFormatData(13, UserData.TP.ToString(ECurrencyType.TP));
        _txtTotalBrainGenCount.text = Managers.Definition.GetTextFormatData(9, (UserData.TotalBrainGenCount - 1).ToString());
        _txtMaxDepth.text = Managers.Definition.GetTextFormatData(15, UserData.MaxDepth.ToString());
    }
}
