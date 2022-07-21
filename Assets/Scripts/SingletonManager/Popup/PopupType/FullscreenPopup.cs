using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class FullscreenPopup : PopupBase
{
    [SerializeField]
    public TextMeshProUGUI text;

    public override void Init()
    {
        base.Init();
        GameObject go = transform.GetChild(0).gameObject;
        text.text = "FullscreenPopup";
        gameObject.name = text.text;
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
        PoolManager.Instance.DespawnObject(EPrefabsType.Popup, gameObject);
    }
}
