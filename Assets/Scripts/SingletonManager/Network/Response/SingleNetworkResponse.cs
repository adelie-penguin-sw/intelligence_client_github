using MainTab;
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

    public BrainData GetBrainDataForID(int id)
    {
        BrainData data = new BrainData();

        data.id = id;
        data.brainType = (id == 1) ? EBrainType.MAINBRAIN : EBrainType.NORMALBRAIN;

        if (ansEquationsDic.ContainsKey(id))
            data.intellect = new UpArrowNotation(ansEquationsDic[id].ansEquation);


        if (distancesDic.ContainsKey(id))
            data.distance = distancesDic[id].distance;

        if(coordinatesDic.ContainsKey(id))
            data.coordinates = new Vector2(coordinatesDic[id].x, coordinatesDic[id].y);

        if (structuresDic.ContainsKey(id))
        {
            foreach (var receiverId in structuresDic[id].structure)
            {
                data._receiverIdList.Add(receiverId);
            }
        }

        //data.skinCode = skinDic[id].skincode;
        //data.UpgradeCondition = upgradeConditionDic[id].upgrade;
        return data;
    }

    /// <summary>
    /// 브레인 추가시 받는 데이터 업데이트 해주는 함수
    /// </summary>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void UpdateSingleNetworkData(CreateSingleNetworkBrainRequest req, CreateSingleNetworkBrainResponse res)
    {
        UserData.NP = res.NP;
        ansEquationsDic.Clear();
        foreach (var data in res.ansEquations)
        {
            ansEquationsDic.Add(data.id, data);
        }

        calcTime = res.calcTime;

        distancesDic.Clear();
        foreach (var data in res.distances)
        {
            distancesDic.Add(data.id, data);
        }

        Coordinates coordinates = new Coordinates();
        coordinates.x = req.x;
        coordinates.y = req.y;
        coordinatesDic.Add(res.newBrain, coordinates);
    }

    /// <summary>
    /// 채널 추가시 받는 데이터 업데이트 해주는 함수
    /// </summary>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void UpdateSingleNetworkData(CreateSingleNetworkChannelRequest req, CreateSingleNetworkChannelResponse res)
    {
        UserData.NP = res.NP;

        ansEquationsDic.Clear();
        foreach (var data in res.ansEquations)
        {
            ansEquationsDic.Add(data.id, data);
        }

        calcTime = res.calcTime;

        distancesDic.Clear();
        foreach (var data in res.distances)
        {
            distancesDic.Add(data.id, data);
        }

        if (structuresDic.ContainsKey(req.from))
        {
            structuresDic[req.from].structure.Add(req.to);
        }
    }
}