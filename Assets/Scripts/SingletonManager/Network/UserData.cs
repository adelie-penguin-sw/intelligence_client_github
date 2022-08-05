using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Prefers로 저장되는 데이터 , 글로벌 유저 데이터 저장
/// </summary>
public class UserData
{
    public static string token;
    public static UpArrowNotation TP = new UpArrowNotation();
    public static UpArrowNotation NP = new UpArrowNotation();
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
