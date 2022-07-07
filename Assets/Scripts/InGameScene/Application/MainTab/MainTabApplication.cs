using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainTabApplication : BaseApplication
{
    public override void Init()
    {
        base.Init();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void AdvanceTime(float dt_sec)
    {
        base.AdvanceTime(dt_sec);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Dispose()
    {
        base.Dispose();
        PoolManager.Instance.DespawnObject(EPrefabsType.TabApplication, this.gameObject);
    }
}
