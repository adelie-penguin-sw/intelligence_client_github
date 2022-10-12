using MainTab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SingleNetworkResponse
{
    public AnsEquation NP;
    public AnsEquation TP;
    public List<Achievements> achievements;
    public List<BrainAttributes> brainAttributes;
    public long calcTime;
    public long allBrainCount;
    public int experimentLevel;
    public long experimentStartTime;
    public List<int> resetCounts;
    public int statusCode;
    public List<Structure> structures;
    public List<UpgradeCondition> upgradeCondition;
    public long totalBrainGenCount;
    public long maxDepth;
    public long brainUpgradePower;
    public AnsEquation multiplierRewardForReset;
}

[Serializable]
public class SingleNetworkWrapper
{
    [ShowInInspector] public Dictionary<long, BrainAttributes> brainAttributesDic = new Dictionary<long, BrainAttributes>();
    //[ShowInInspector] public List<Structure> structures = new List<Structure>();
    //[ShowInInspector] public Dictionary<long, Structure> structuresDic = new Dictionary<long, Structure>();
    [ShowInInspector] public Dictionary<long, UpgradeCondition> upgradeConditionDic = new Dictionary<long, UpgradeCondition>();
    [ShowInInspector] public Dictionary<long, HashSet<long>> senderBrainsDic = new Dictionary<long, HashSet<long>>();
    [ShowInInspector] public Dictionary<long, HashSet<long>> receiverBrainsDic = new Dictionary<long, HashSet<long>>();
    [ShowInInspector] public long calcTime;
    [ShowInInspector] public List<Achievements> achievements = new List<Achievements>();
    [ShowInInspector] public int experimentLevel;
    [ShowInInspector] public long totalBrainGenCount;
    [ShowInInspector] public long maxDepth;
    [ShowInInspector] public long brainUpgradePower;
    [ShowInInspector] public UpArrowNotation multiplierRewardForReset;

    public SingleNetworkWrapper()
    {

    }

    public SingleNetworkWrapper(SingleNetworkResponse res)
    {
        if (res != null)
        {
            foreach(var data in res.brainAttributes)
            {
                brainAttributesDic.Add(data.id, data);
            }

            foreach (var data in res.structures)
            {
                if (!senderBrainsDic.ContainsKey(data.to))
                    senderBrainsDic.Add(data.to, new HashSet<long>());
                senderBrainsDic[data.to].Add(data.from);

                if (!receiverBrainsDic.ContainsKey(data.from))
                    receiverBrainsDic.Add(data.from, new HashSet<long>());
                receiverBrainsDic[data.from].Add(data.to);
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

            UserData.ExperimentStartTime = res.experimentStartTime;
            UserData.ResetCounts = res.resetCounts;

            achievements = res.achievements;

            experimentLevel = res.experimentLevel;

            totalBrainGenCount = res.allBrainCount;

            maxDepth = res.maxDepth;

            brainUpgradePower = res.brainUpgradePower;

            multiplierRewardForReset = new UpArrowNotation(
                res.multiplierRewardForReset.top3Coeffs[0],
                res.multiplierRewardForReset.top3Coeffs[1],
                res.multiplierRewardForReset.top3Coeffs[2],
                res.multiplierRewardForReset.operatorLayerCount);
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

        if(brainAttributesDic.ContainsKey(id))
        {
            foreach (AnsEquation ans in brainAttributesDic[id].ansEquation)
            {
                data.intellectEquation.Add(new UpArrowNotation(ans.top3Coeffs[0],
                                                       ans.top3Coeffs[1],
                                                       ans.top3Coeffs[2],
                                                       ans.operatorLayerCount));
            }


            AnsEquation m = brainAttributesDic[id].multiplier;
            if (m.top3Coeffs != null)
            {
                data.upgradedMultiplier = new UpArrowNotation(m.top3Coeffs[0],
                                                      m.top3Coeffs[1],
                                                      m.top3Coeffs[2],
                                                      m.operatorLayerCount);
            }
            else
            {
                data.upgradedMultiplier = new UpArrowNotation(1);
            }

            data.distance = brainAttributesDic[id].distance;

            data.coordinates = new Vector2((float)brainAttributesDic[id].x, (float)brainAttributesDic[id].y);
        }

        if(receiverBrainsDic.ContainsKey(id))
        {
            data.receiverIds = receiverBrainsDic[id];
        }

        if(senderBrainsDic.ContainsKey(id))
        {
            data.senderIds = senderBrainsDic[id];
        }

        Queue<long> q = new Queue<long>();
        HashSet<long> visitSet = new HashSet<long>();
        q.Enqueue(id);
        visitSet.Add(id);
        while(q.Count > 0)
        {
            long curID = q.Dequeue();
            if(senderBrainsDic.ContainsKey(curID))
            {
                foreach(var sender in senderBrainsDic[curID])
                {
                    if(!visitSet.Contains(sender) && receiverBrainsDic.ContainsKey(sender) && receiverBrainsDic[sender].Count == 1)
                    {
                        data.deletableSenderIds.Add(sender);
                        q.Enqueue(sender);
                        visitSet.Add(sender);
                    }
                }
            }
        }
        return data;
    }

    public List<long> GetAllDeletableSenderIdListForID(long id)
    {
        List<long> resultList = new List<long>();

        BrainData data = GetBrainDataForID(id);

        foreach (long deletableSenderID in data.deletableSenderIds)
        {
            resultList.Add(deletableSenderID);
            resultList.AddRange(GetAllDeletableSenderIdListForID(deletableSenderID));
        }
        return resultList;
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

        long brainId;
        foreach (var attribute in res.brainAttributes)
        {
            brainId = attribute.id;
            if (!brainAttributesDic.ContainsKey(brainId))
            {
                brainAttributesDic.Add(brainId, attribute);
            }
            else
            {
                brainAttributesDic[brainId] = attribute;
            }
        }
        calcTime = res.calcTime;

        totalBrainGenCount = res.allBrainCount;
    }

    /// <summary>
    /// 고정 브레인 추가시 받는 데이터 업데이트 해주는 함수
    /// </summary>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void UpdateSingleNetworkData(CreateFixedNetworkBrainRequest req, CreateFixedNetworkBrainResponse res)
    {
        UserData.TP = new UpArrowNotation(
            res.TP.top3Coeffs[0],
            res.TP.top3Coeffs[1],
            res.TP.top3Coeffs[2],
            res.TP.operatorLayerCount);

        long brainId;
        foreach (var attribute in res.FNBrainAttributes)
        {
            brainId = attribute.id;
            if (!brainAttributesDic.ContainsKey(brainId))
            {
                brainAttributesDic.Add(brainId, attribute);
            }
            else
            {
                brainAttributesDic[brainId] = attribute;
            }
        }
    }

    /// <summary>
    /// 채널 추가시 받는 데이터 업데이트 해주는 함수
    /// </summary>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void UpdateSingleNetworkData(CreateSingleNetworkChannelRequest req, CreateSingleNetworkChannelResponse res)
    {
        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

        long brainId;
        foreach (var attribute in res.brainAttributes)
        {
            brainId = attribute.id;
            if (!brainAttributesDic.ContainsKey(brainId))
            {
                brainAttributesDic.Add(brainId, attribute);
            }
            else
            {
                brainAttributesDic[brainId] = attribute;
            }
        }

        calcTime = res.calcTime;

        if (!senderBrainsDic.ContainsKey(req.to))
            senderBrainsDic.Add(req.to, new HashSet<long>());
        senderBrainsDic[req.to].Add(req.from);

        if (!receiverBrainsDic.ContainsKey(req.from))
            receiverBrainsDic.Add(req.from, new HashSet<long>());
        receiverBrainsDic[req.from].Add(req.to);
    }

    /// <summary>
    /// 고정 채널 추가시 받는 데이터 업데이트 해주는 함수
    /// </summary>
    /// <param name="req"></param>
    /// <param name="res"></param>
    public void UpdateSingleNetworkData(CreateFixedNetworkChannelRequest req, CreateFixedNetworkChannelResponse res)
    {
        UserData.TP = new UpArrowNotation(
            res.TP.top3Coeffs[0],
            res.TP.top3Coeffs[1],
            res.TP.top3Coeffs[2],
            res.TP.operatorLayerCount);

        long brainId;
        foreach (var attribute in res.FNBrainAttributes)
        {
            brainId = attribute.id;
            if (!brainAttributesDic.ContainsKey(brainId))
            {
                brainAttributesDic.Add(brainId, attribute);
            }
            else
            {
                brainAttributesDic[brainId] = attribute;
            }
        }

        if (!senderBrainsDic.ContainsKey(req.to))
            senderBrainsDic.Add(req.to, new HashSet<long>());
        senderBrainsDic[req.to].Add(req.from);

        if (!receiverBrainsDic.ContainsKey(req.from))
            receiverBrainsDic.Add(req.from, new HashSet<long>());
        receiverBrainsDic[req.from].Add(req.to);
    }

    public void UpdateSingleNetworkData(CreateSingleNetworkBrainNumberResponse res)
    {
        long brainId;
        foreach (var attribute in res.brainAttributes)
        {
            brainId = attribute.id;
            if (!brainAttributesDic.ContainsKey(brainId))
            {
                brainAttributesDic.Add(brainId, attribute);
            }
            else
            {
                brainAttributesDic[brainId] = attribute;
            }
        }

        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

        calcTime = res.calcTime;
    }

    public void UpdateSingleNetworkData(DeleteSingleNetworkBrainResponse res)
    {
        long brainId;
        foreach (var attribute in res.brainAttributes)
        {
            brainId = attribute.id;
            if (!brainAttributesDic.ContainsKey(brainId))
            {
                brainAttributesDic.Add(brainId, attribute);
            }
            else
            {
                brainAttributesDic[brainId] = attribute;
            }
        }

        UserData.NP = new UpArrowNotation(
            res.NP.top3Coeffs[0],
            res.NP.top3Coeffs[1],
            res.NP.top3Coeffs[2],
            res.NP.operatorLayerCount);

        calcTime = res.calcTime;
        foreach(var removeId in res.deletedBrains)
        {
            RemoveDataForID(removeId);
        }
    }

    private void RemoveDataForID(long id)
    {
        if(brainAttributesDic.ContainsKey(id))
        {
            brainAttributesDic.Remove(id);
        }
        if (upgradeConditionDic.ContainsKey(id))
        {
            upgradeConditionDic.Remove(id);
        }
        if (senderBrainsDic.ContainsKey(id))
        {
            senderBrainsDic.Remove(id);
        }
        if (receiverBrainsDic.ContainsKey(id))
        {
            receiverBrainsDic.Remove(id);
        }
    }
}