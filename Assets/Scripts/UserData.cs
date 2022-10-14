using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

/// <summary>
/// Player Prefers로 저장되는 데이터 , 글로벌 유저 데이터 저장
/// </summary>
public class UserData
{
    public static string token;

    private static string _username;
    public static string Username
    {
        get
        {
            return _username;
        }
        set
        {
            _username = value;
        }
    }

    public static UpArrowNotation CoreIntellect
    {
        get
        {
            double elapsedTime = (double)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - LastCalcTime) / 1000f;
            UpArrowNotation intellect = Equation.GetCurrentIntellect(SingleNetworkWrapper.GetBrainDataForID(0).intellectEquation, elapsedTime);
            return intellect;
        }
    }

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

    private static Dictionary<long, TPUpgrade> _tpUpgrades = new Dictionary<long, TPUpgrade>();
    public static Dictionary<long, TPUpgrade> TPUpgrades
    {
        get
        {
            return _tpUpgrades;
        }
    }

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

    public static string ExpGoalStr
    {
        get
        {
            return Managers.Definition.GetData<List<UpArrowNotation>>(DefinitionKey.experimentGoalList)[ExperimentLevel].ToString();
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

    public static long LastCalcTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

    private static Dictionary<long, QuestAttributes> _dicQuest = new Dictionary<long, QuestAttributes>();
    public static Dictionary<long, QuestAttributes> DicQuest { get { return _dicQuest; } }

    public static void UpdateTutorialQuest(CompleteQuestResponse res)
    {
        if (res != null)
        {
            _dicQuest.Clear();
            foreach (var quest in res.questAttributes)
            {
                _dicQuest.Add(quest.questId, quest);
            }
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
        if (_tpUpgrades.ContainsKey(0))
        {
            _tpUpgrades.Remove(0);
        }
    }


    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);

        switch (key)
        {
            case "Token":
                token = value;
                break;
        }
    }

    public static void LoadAllData()
    {
        token = PlayerPrefs.GetString("Token");
        Debug.Log(token);
    }
}
