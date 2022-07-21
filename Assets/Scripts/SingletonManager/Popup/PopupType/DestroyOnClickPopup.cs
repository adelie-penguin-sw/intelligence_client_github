using UnityEngine;
using TMPro;
using System.Collections;

public class DestroyOnClickPopup : PopupBase
{
    [SerializeField]
    public TextMeshProUGUI text;

    public override void Init()
    {
        base.Init();
        text.text = "Popup1";
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
