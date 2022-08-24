using MainTab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class LeaderboardResponse
{
    public int statusCode;
    public List<ViewSingleLeaderboard> allRank;
    public long selfRank;
}
