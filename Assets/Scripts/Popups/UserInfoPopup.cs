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
        _txtUserName.text = UserData.Username;

        _txtExperimentLevelKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupExperimentLevelKey);
        _txtAttemptsKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupAttemptsKey);
        _txtCoreIntellectKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupCoreIntellectKey);
        _txtNPKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupNPKey);
        _txtTPKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupTPKey);
        _txtTotalBrainGenCountKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupTotalBrainGenCountKey);
        _txtMaxDepthKey.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupMaxDepthKey);

        _txtExperimentLevel.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupExperimentLevelValue, UserData.ExperimentLevel.ToString());
        _txtAttempts.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupAttemptsValue, UserData.ResetCounts[UserData.ExperimentLevel].ToString());
        _txtCoreIntellect.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupCoreIntellectValue, UserData.CoreIntellect.ToString(ECurrencyType.INTELLECT));
        _txtNP.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupNPValue, UserData.NP.ToString(ECurrencyType.NP));
        _txtTP.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupTPValue, UserData.TP.ToString(ECurrencyType.TP));
        _txtTotalBrainGenCount.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupTotalBrainGenCountValue, (UserData.TotalBrainGenCount - 1).ToString());
        _txtMaxDepth.text = Managers.Definition.GetUIText(UITextKey.userInfoPopupMaxDepthValue, UserData.MaxDepth.ToString());
    }
}
