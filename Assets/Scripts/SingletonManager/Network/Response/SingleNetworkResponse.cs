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
    public double NP; //나중에 뭔가 서버랑 이야기해서 바꿔야할듯? 
    public double TP; //이것두
    public List<Structure> structures;
    public List<Coordinates> coordinates;
    public List<Skin> skin;
    public List<UpgradeCondition> upgradeCondition;
    public int calcTime;
    public List<Achievements> achievements;
}

public class SingleNetworkWrapper
{
    public Dictionary<int, AnsEquations> ansEquationsDic = new Dictionary<int, AnsEquations>();
    public Dictionary<int, Distances> distancesDic = new Dictionary<int, Distances>();
    public Dictionary<int, Structure> structuresDic = new Dictionary<int, Structure>();
    public Dictionary<int, Coordinates> coordinatesDic = new Dictionary<int, Coordinates>();
    public Dictionary<int, Skin> skinDic = new Dictionary<int, Skin>();
    public Dictionary<int, UpgradeCondition> upgradeConditionDic = new Dictionary<int, UpgradeCondition>();
    public int calcTime;
    public List<Achievements> achievements = new List<Achievements>();

    public SingleNetworkWrapper(SingleNetworkResponse res)
    {
        if (res != null)
        {
            foreach (var data in res.ansEquations)
            {
                ansEquationsDic.Add(data.id, data);
            }

            foreach (var data in res.distances)
            {
                distancesDic.Add(data.id, data);
            }

            foreach (var data in res.structures)
            {
                structuresDic.Add(data.id, data);
            }

            foreach (var data in res.coordinates)
            {
                coordinatesDic.Add(data.id, data);
            }

            foreach (var data in res.skin)
            {
                skinDic.Add(data.id, data);
            }

            foreach (var data in res.upgradeCondition)
            {
                upgradeConditionDic.Add(data.id, data);
            }

            UserData.NP = res.NP;
            UserData.TP = res.TP;
            calcTime = res.calcTime;

            achievements = res.achievements;
        }
    }
}