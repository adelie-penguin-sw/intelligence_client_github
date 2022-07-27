using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class AlertPopup : PopupBase
{
    [SerializeField]
    public TextMeshProUGUI text;

    public override void Init()
    {
        base.Init();
    }

    public override void Set()
    {
        base.Set();
    }

    public override void AdvanceTime(float dt_sec)
    {
        base.AdvanceTime(dt_sec);
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
