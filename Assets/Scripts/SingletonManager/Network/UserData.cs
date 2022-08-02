using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public static string token;
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
