using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveUserData
{
    /// <summary>
    /// 유저 토큰
    /// </summary>
    private static string _token;

    public static string Token
    {
        get
        {
            return _token;
        }
        set
        {
            _token = value;
            PlayerPrefs.SetString("Token", value);
        }
    }

    /// <summary>
    /// 사운드 볼륨 0~1 사이의 실수
    /// </summary>
    private static float _sfxVolume;      
    public static float SFXVolume
    {
        get
        {
            return _sfxVolume;
        }
        set
        {
            _sfxVolume = value;
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }

    /// <summary>
    /// 브레인 생성 대기 시간
    /// </summary>
    private static float _waitBrainClickTime;
    public static float WaitBrainClickTime
    {
        get
        {
            if(_waitBrainClickTime <= 0.0f)
            {
                _waitBrainClickTime = 0.3f;
                PlayerPrefs.SetFloat("WaitBrainClickTime", _waitBrainClickTime);
            }
            return _waitBrainClickTime;
        }
        set
        {
            _waitBrainClickTime = value;
            PlayerPrefs.SetFloat("WaitBrainClickTime", _waitBrainClickTime);
        }
    }

    /// <summary>
    /// 텍스트 언어
    /// </summary>
    private static ETextLanguage _lang = ETextLanguage.KOR;

    public static ETextLanguage Lang
    {
        get
        {
            return _lang;
        }
        set
        {
            _lang = value;
            PlayerPrefs.SetInt("Language", (int)value);
        }
    }


    public static void LoadAllData()
    {
        _token = PlayerPrefs.GetString("Token");
        _waitBrainClickTime = PlayerPrefs.GetFloat("WaitBrainClickTime");
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
        _lang = (ETextLanguage)PlayerPrefs.GetInt("Language");
    }
}
