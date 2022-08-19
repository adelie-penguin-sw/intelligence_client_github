using MainTab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SingleNetworkResponse
{
    public int statusCode;
    public List<AnsEquations> ansEquations;
    public List<Multiplier> multipliers;
    public List<Distances> distances;
    public AnsEquation NP; //나중에 뭔가 서버랑 이야기해서 바꿔야할듯? 
    public AnsEquation TP; //이것두
    public List<Structure> structures;
    public List<Coordinates> coordinates;
    public List<Skin> skin;
    public List<UpgradeCondition> upgradeCondition;
    public long calcTime;
    public List<Achievements> achievements;
}

[Serializable]
public class SingleNetworkWrapper
{
    [ShowInInspector] public Dictionary<long, AnsEquations> ansEquationsDic = new Dictionary<long, AnsEquations>();
    [ShowInInspector] public Dictionary<long, Multiplier> multipliersDic = new Dictionary<long, Multiplier>();
    [ShowInInspector] public Dictionary<long, Distances> distancesDic = new Dictionary<long, Distances>();
    [ShowInInspector] public Dictionary<long, Structure> structuresDic = new Dictionary<long, Structure>();
    [ShowInInspector] public Dictionary<long, Coordinates> coordinatesDic = new Dictionary<long, Coordinates>();
    [ShowInInspector] public Dictionary<long, Skin> skinDic = new Dictionary<long, Skin>();
    [ShowInInspector] public Dictionary<long, UpgradeCondition> upgradeConditionDic = new Dictionary<long, UpgradeCondition>();
    public long calcTime;
    public List<Achievements> achievements = new List<Achievements>();

    public SingleNetworkWrapper(SingleNetworkResponse res)
    {
        if (res != null)
        {
            foreach (var data in res.ansEquations)
            {
                ansEquationsDic.Add(data.id, data);
            }

            foreach (var data in res.multipliers)
            {
                multipliersDic.Add(data.id, data);
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

            UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

            UserData.TP = new UpArrowNotation(
            res.TP.top3Coeffs[0],
            res.TP.top3Coeffs[1],
            res.TP.top3Coeffs[2],
            res.TP.operatorLayerCount);
            calcTime = res.calcTime;

            achievements = res.achievements;
        }
    }

    /// <summary>
    /// 인자값에 해당하는 id를 가진 brain 데이터 가져오는 함수
    /// </summary>
    /// <param name="id">브레인 아이</param>
    /// <returns>brainData</returns>
    public BrainData GetBrainDataForID(long id)
    {
        BrainData data = new BrainData();

        data.id = id;
        data.brainType = (id == 0) ? EBrainType.MAINBRAIN : EBrainType.NORMALBRAIN;

        if (ansEquationsDic.ContainsKey(id))
        {
            foreach (AnsEquation ans in ansEquationsDic[id].ansEquation)
            {
                data.intellectEquation.Add(new UpArrowNotation(ans.top3Coeffs[0],
                                                       ans.top3Coeffs[1],
                                                       ans.top3Coeffs[2],
                                                       ans.operatorLayerCount));
            }
        }

        if (multipliersDic.ContainsKey(id))
        {
            AnsEquation m = multipliersDic[id].multiplier;
            data.multiplier = new UpArrowNotation(m.top3Coeffs[0],
                                                  m.top3Coeffs[1],
                                                  m.top3Coeffs[2],
                                                  m.operatorLayerCount);
        }

        if (distancesDic.ContainsKey(id))
            data.distance = distancesDic[id].distance;

        if (coordinatesDic.ContainsKey(id))
            data.coordinates = new Vector2((float)coordinatesDic[id].x, (float)coordinatesDic[id].y);

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
        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

        ansEquationsDic.Clear();
        foreach (var data in res.ansEquations)
        {
            ansEquationsDic.Add(data.id, data);
        }

        multipliersDic.Clear();
        foreach (var data in res.multipliers)
        {
            multipliersDic.Add(data.id, data);
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
    public void UpdateSingleNetworkData(CreateSingleNetworkChannelRequest req, CreateSingleNetworkChannelResponse res, Action callback)
    {

        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

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

        if (!structuresDic.ContainsKey(req.from))
        {
            Structure data = new Structure();
            data.id = req.from;
            data.structure = new List<long>();
            structuresDic.Add(req.from, data);
        }
        structuresDic[req.from].structure.Add(req.to);

        callback();
    }
    
    public void UpdateSingleNetworkData(CreateSingleNetworkBrainNumberResponse res, Action callback)
    {
        ansEquationsDic.Clear();
        foreach (var data in res.ansEquations)
        {
            ansEquationsDic.Add(data.id, data);
        }

        multipliersDic.Clear();
        foreach (var data in res.multipliers)
        {
            multipliersDic.Add(data.id, data);
        }

        distancesDic.Clear();
        foreach (var data in res.distances)
        {
            distancesDic.Add(data.id, data);
        }

        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

        calcTime = res.calcTime;

        callback();
    }

    public void UpdateSingleNetworkData(DeleteSingleNetworkBrainResponse res)
    {
        ansEquationsDic.Clear();
        foreach (var data in res.ansEquations)
        {
            ansEquationsDic.Add(data.id, data);
        }

        multipliersDic.Clear();
        foreach (var data in res.multipliers)
        {
            multipliersDic.Add(data.id, data);
        }

        distancesDic.Clear();
        foreach (var data in res.distances)
        {
            distancesDic.Add(data.id, data);
        }

        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

        calcTime = res.calcTime;

        foreach(var brainId in res.deletedBrains)
        {
            RemoveDataForID(brainId);
        }
    }

    private void RemoveDataForID(long id)
    {
        if (ansEquationsDic.ContainsKey(id))
        {
            ansEquationsDic.Remove(id);
        }
        if (multipliersDic.ContainsKey(id))
        {
            multipliersDic.Remove(id);
        }
        if (distancesDic.ContainsKey(id))
        {
            distancesDic.Remove(id);
        }
        if (structuresDic.ContainsKey(id))
        {
            structuresDic.Remove(id);
        }
        if (coordinatesDic.ContainsKey(id))
        {
            coordinatesDic.Remove(id);
        }
        if (skinDic.ContainsKey(id))
        {
            skinDic.Remove(id);
        }
        if (upgradeConditionDic.ContainsKey(id))
        {
            upgradeConditionDic.Remove(id);
        }
    }
}