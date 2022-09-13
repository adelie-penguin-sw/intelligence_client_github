using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTabApplication : BaseTabApplication
{
    [SerializeField] private BaseTabController<TpTabApplication>[] _controllers;
    [SerializeField] private TpTabView _tpTabView;

    public TpTabView TpTabView
    {
        get
        {
            return _tpTabView;
        }
    }

    [SerializeField] private TpTabModel _tpTabModel;
    public TpTabModel TpTabModel
    {
        get
        {
            return _tpTabModel;
        }
    }

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

    public override void LateAdvanceTime(float dt_sec)
    {
        base.LateAdvanceTime(dt_sec);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void Dispose()
    {
        base.Dispose();
    }

}
