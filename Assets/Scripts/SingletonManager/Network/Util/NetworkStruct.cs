using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class NetworkStruct 
{

}


[Serializable]
public struct AnsEquations
{
    public long id;
    public List<AnsEquation> ansEquation;
}
[Serializable]
public struct AnsEquation
{
    public TopCoeffs top3Layer;
    public int operatorLayerCount;
}

[Serializable]
public struct TopCoeffs
{
    public double top1Coeff;
    public double top2Coeff;
    public double top3Coeff;
}

[Serializable]
public struct Coordinates
{
    public long id;
    public double x;
    public double y;
}


[Serializable]
public struct Distances
{
    public long id;
    public long distance;
}


[Serializable]
public struct Skin
{
    public long id;
    public long skincode;
}

[Serializable]
public struct Structure
{
    public long id;
    public List<long> structure;
}

[Serializable]
public struct UpgradeCondition
{
    public long id;
    public long upgrade;
}

[Serializable]
public struct Achievements
{
    public long id;
    public long achievement;
}