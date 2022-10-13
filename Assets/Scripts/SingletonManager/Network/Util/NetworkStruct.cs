using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class NetworkStruct 
{

}


[Serializable]
public class BrainAttributes
{
    public List<AnsEquation> ansEquation = new List<AnsEquation>();
    public long distance;
    public long id;
    public long multiplierUpgradeCount;
    public long limitUpgradeCount;
    public long skinCode;
    public double x;
    public double y;
}

[Serializable]
public struct AnsEquation
{
    public List<double> top3Coeffs;
    public int operatorLayerCount;
}


[Serializable]
public struct Structure
{
    public long from;
    public long to;
}

[Serializable]
public struct UpgradeCondition
{
    public long id;
    public bool unlocked;
    public long upgrade;
    public bool maxed;
}

[Serializable]
public struct Achievements
{
    public long id;
    public long achievement;
}

[Serializable]
public struct ViewSingleLeaderboard
{
    public long rank;
    public string email;
    public long resetCount;
    public AnsEquation maximumCoreIntellect;
}

[Serializable] 
public struct QuestAttributes
{
    public long questId;
    public long questLevel;
    public bool isCompleted;
}