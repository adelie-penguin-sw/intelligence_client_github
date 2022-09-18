﻿using System.Collections;
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
            NotificationManager.Instance.PostNotification(ENotiMessage.UPDATE_TP);
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
            NotificationManager.Instance.PostNotification(ENotiMessage.UPDATE_NP);
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
        if (_tpUpgradeCounts.ContainsKey(0))
        {
            _tpUpgradeCounts.Remove(0);
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
