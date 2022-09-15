using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEditor;

public class DefinitionManager : MonoBehaviour
{
    #region Singelton
    private static DefinitionManager _instance;
    public static DefinitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DefinitionManager>();
                if (FindObjectsOfType<DefinitionManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("DefinitionManager");
                    _instance = go.AddComponent<DefinitionManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    private List<Dictionary<string, object>> _csvData;
    public List<Dictionary<string, object>> CSVData { get { return _csvData; } }

    private async void LoadS3Data()
    {
        string res = await NetworkManager.Instance.API_S3Data("base.csv");
        if (res != null)
        {
            _csvData = CSVReader.Read(res);
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadS3Data();
    }
}
