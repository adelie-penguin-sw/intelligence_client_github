using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ErrorPopup : PopupBase
{
    [SerializeField] private TextMeshProUGUI _textTitle;
    public void Init(ErrorResponse error)
    {
        base.Init();
        _textTitle.text = string.Format("ErrorCode : {0}\n{1}", error.statusCode, error.msg);
    }
}
