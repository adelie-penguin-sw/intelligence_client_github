using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: 주석 추가해주셔야할듯

/// <summary>
/// Player Prefers로 저장되는 데이터 , 글로벌 유저 데이터 저장
/// </summary>
public class UserData
{
    /// <summary>
    /// 유저 네임
    /// </summary>
    public static string username;
    /// <summary>
    /// 연구 달성 상태 여부
    /// </summary>
    public static bool IsCompleteExp = false;
    public static long LastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

    #region 재화
    private static UpArrowNotation _tp = new UpArrowNotation();
    public static UpArrowNotation TP
    {
        get
        {
            return _tp;
        }
        set
        {
            _tp = value;
            Managers.Notification.PostNotification(ENotiMessage.UPDATE_TP);
        }
    }

    private static UpArrowNotation _np = new UpArrowNotation();
    public static UpArrowNotation NP
    {
        get
        {
            return _np;
        }
        set
        {
            _np = value;
            Managers.Notification.PostNotification(ENotiMessage.UPDATE_NP);
        }
    }

    #endregion

    #region CoreBrain
    public static UpArrowNotation CoreIntellect
    {
        get
        {
            double elapsedTime = (double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - LastCalcTime) / 1000f;
            UpArrowNotation intellect = Equation.GetCurrentIntellect(SingleNetworkWrapper.GetBrainDataForID(0).intellectEquation, elapsedTime);
            UpArrowNotation maxIntellect = Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[ExperimentLevel];
            if (intellect >= maxIntellect)
                return maxIntellect;
            return intellect;
        }
    }
    #endregion

    #region BrainNetwork
    private static SingleNetworkWrapper _singleNetworkWrapper = new SingleNetworkWrapper();
    public static SingleNetworkWrapper SingleNetworkWrapper
    {
        get
        {
            return _singleNetworkWrapper;
        }
        set
        {
            _singleNetworkWrapper = value;
        }
    }
    #endregion

    #region BrainNetwork 정보
    private static long _experimentStartTime = 0;
    public static long ExperimentStartTime
    {
        get
        {
            return _experimentStartTime;
        }
        set
        {
            _experimentStartTime = value;
        }
    }

    private static List<int> _resetCounts = new List<int>();
    public static List<int> ResetCounts
    {
        get
        {
            return _resetCounts;
        }
        set
        {
            _resetCounts = value;
        }
    }

    public static long TotalBrainGenCount
    {
        get
        {
            return _singleNetworkWrapper.totalBrainGenCount;
        }
        set
        {
            _singleNetworkWrapper.totalBrainGenCount = value;
        }
    }

    public static int ExperimentLevel
    {
        get
        {
            return _singleNetworkWrapper.experimentLevel;
        }
    }

    public static long MaxDepth
    {
        get
        {
            return _singleNetworkWrapper.maxDepth;
        }
    }

    public static UpArrowNotation MultiplierRewardForReset
    {
        get
        {
            return _singleNetworkWrapper.multiplierRewardForReset;
        }
        set
        {
            _singleNetworkWrapper.multiplierRewardForReset = value;
        }
    }
    #endregion

    #region TPUpgrade

    private static Dictionary<long, TPUpgrade> _tpUpgrades = new Dictionary<long, TPUpgrade>();
    public static Dictionary<long, TPUpgrade> TPUpgrades
    {
        get
        {
            return _tpUpgrades;
        }
    }

    public static void UpdateTPUpgradeCounts(List<UpgradeCondition> upgradeConditions)
    {
        _tpUpgrades.Clear();

        foreach (UpgradeCondition cond in upgradeConditions)
        {
            _tpUpgrades.Add(cond.id, new TPUpgrade(cond.unlocked, cond.upgrade, cond.maxed));
        }
        for (int i = 1; i <= 29; i++)
        {
            if (!_tpUpgrades.ContainsKey(i))
            {
                _tpUpgrades.Add(i, new TPUpgrade(false, 0, false));
            }
        }
    }
    #endregion

    #region Tutorial Quest
    public static Dictionary<long, QuestAttributes> DicQuest { get { return _singleNetworkWrapper.questAttributeDic; } }
    public static bool IsTutorialClear
    {
        get
        {
            foreach (var quest in DicQuest.Values)
            {
                if (!quest.isCompleted) return false;
            }
            return true;
        }
    }

    public static long CurrentTutorialKey
    {
        get
        {
            foreach (var quest in DicQuest.Values)
            {
                if (!quest.isCompleted) return quest.questId;
            }
            return -1;
        }
    }

    public static void UpdateTutorialQuest(CompleteQuestResponse res)
    {
        if (res != null)
        {
            _singleNetworkWrapper.questAttributeDic.Clear();
            foreach (var quest in res.questAttributes)
            {
                _singleNetworkWrapper.questAttributeDic.Add(quest.questId, quest);
            }
        }
    }
    #endregion
}
public struct TPUpgrade
{
    public bool Unlocked;
    public long UpgradeCount;
    public bool Maxed;

    public TPUpgrade(bool unlocked, long upgradeCount, bool maxed)
    {
        Unlocked = unlocked;
        UpgradeCount = upgradeCount;
        Maxed = maxed;
    }
}
