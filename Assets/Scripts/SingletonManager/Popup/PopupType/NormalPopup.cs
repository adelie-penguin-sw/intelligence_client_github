using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class NormalPopup : PopupBase
{
    [SerializeField]
    public TextMeshProUGUI text;

    public override void Init()
    {
        base.Init();
        GameObject go = transform.GetChild(0).gameObject;
        go.transform.Translate(new Vector3(Random.Range(-400, 400), Random.Range(-500, 500), 0));
        text.text = $"{Random.Range(1, 1000)}";
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
