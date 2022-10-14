using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInfoPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI _txtUserName;
    [SerializeField] private TextMeshProUGUI _txtExperimentLevel;
    [SerializeField] private TextMeshProUGUI _txtMaxDepth;
    [SerializeField] private TextMeshProUGUI _txtExpGoal;

    public override void Init()
    {
        base.Init();
        _txtUserName.text = UserData.Username;
        _txtExperimentLevel.text = UserData.ExperimentLevel.ToString();
        _txtMaxDepth.text = UserData.MaxDepth.ToString();
        _txtExpGoal.text = UserData.ExpGoalStr;
    }
}
