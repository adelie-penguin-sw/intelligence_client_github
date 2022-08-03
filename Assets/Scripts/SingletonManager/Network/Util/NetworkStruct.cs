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
    public int id;
    public List<float> ansEquation;
}

[Serializable]
public struct Coordinates
{
    public int id;
    public float x;
    public float y;
}


[Serializable]
public struct Distances
{
    public int id;
    public float x;
    public float y;
}


[Serializable]
public struct Skin
{
    public int id;
    public int skincode;
}

[Serializable]
public struct Structure
{
    public int id;
    public List<int> structure;
}

[Serializable]
public struct UpgradeCondition
{
    public int id;
    public int upgrade;
}

[Serializable]
public struct Achievements
{
    public int id;
    public int achievement;
}