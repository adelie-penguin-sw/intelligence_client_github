using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    private static Managers Instance { get { Init(); return _instance; } }

    PoolManager _poolManager = new PoolManager();

    public static PoolManager Pool { get { return Instance._poolManager; } }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        
    }

    static void Init()
    {
        if(_instance == null)
        {
            GameObject obj = GameObject.Find("@Managers");

            if(obj == null)
            {
                obj = new GameObject { name = "@Managers" };
                obj.AddComponent<Managers>();
            }

            DontDestroyOnLoad(obj);
            _instance = obj.GetComponent<Managers>();
        }
    }
}
