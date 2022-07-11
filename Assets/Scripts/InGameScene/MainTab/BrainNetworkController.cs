using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainNetworkController : BaseTabController<MainTabApplication>
{

    [SerializeField]
    private List<Brain> _brains;
    [SerializeField]
    private List<Channel> _channels;

    public override void Init(MainTabApplication app)
    {
        base.Init(app);
    }
    public override void Set()
    {

    }

    public override void AdvanceTime(float dt_sec)
    {
        foreach (var brain in _brains)
        {
            brain.AdvanceTime(dt_sec);
        }

        foreach (var channel in _channels)
        {
            channel.AdvanceTime(dt_sec);
        }
    }

    public override void Dispose()
    {

    }
}
