using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Prefers로 저장되는 데이터 , 글로벌 유저 데이터 저장
/// </summary>
public class UserData
{
    public static string token;

    private static UpArrowNotation _coreIntellect = new UpArrowNotation();
    public static UpArrowNotation CoreIntellect
    {
        get
        {
            return _coreIntellect;
        }
        set
        {
            _coreIntellect = value;
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

    private static int _experimentLevel = 0;
    public static int ExperimentLevel
    {
        get
        {
            return _experimentLevel;
        }
        set
        {
            _experimentLevel = value;
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

    private static Dictionary<long, long> _tpUpgradeCounts = new Dictionary<long, long>();
    public static Dictionary<long, long> TPUpgradeCounts
    {
        get
        {
            return _tpUpgradeCounts;
        }
    }

    public static void UpdateTPUpgradeCounts(List<UpgradeCondition> upgradeConditions)
    {
        _tpUpgradeCounts.Clear();

        foreach (UpgradeCondition cond in upgradeConditions)
        {
            _tpUpgradeCounts.Add(cond.id, cond.upgrade);
        }
        for (int i = 1; i <= 8; i++)
        {
            if (!_tpUpgradeCounts.ContainsKey(i))
            {
                _tpUpgradeCounts.Add(i, 0);
            }
        }
        if (_tpUpgradeCounts.ContainsKey(0))
        {
            _tpUpgradeCounts.Remove(0);
        }
    }

    private static long _pastBrainGenCount = 0;
    public static long PastBrainGenCount
    {
        get
        {
            return _pastBrainGenCount;
        }
        set
        {
            _pastBrainGenCount = value;
        }
    }

    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);

        switch(key)
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
