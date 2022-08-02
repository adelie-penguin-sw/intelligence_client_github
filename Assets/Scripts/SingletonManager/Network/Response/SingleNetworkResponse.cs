using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SingleNetworkResponse
{
    public int statusCode;
    public List<AnsEquations> ansEquations;
    public List<Distances> distances;
    public float NP;
    public float TP;
    public List<Structure> structures;
    public List<Coordinates> coordinates;
    public List<Skin> skin;
    public List<UpgradeCondition> upgradeCondition;
    public int calcTime;
    public Dictionary<int, float> achievements;
}

[Serializable]
public struct AnsEquations
{
    public int id;
    public List<int> ansEquation;
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