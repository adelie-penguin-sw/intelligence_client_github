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
        Debug.Log(DefinitionManager.Instance.CSVData);
    }
}
