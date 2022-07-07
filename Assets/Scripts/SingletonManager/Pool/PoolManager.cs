using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    #region Singelton
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
                if (FindObjectsOfType<PoolManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("PoolManager");
                    _instance = go.AddComponent<PoolManager>();
                }
            }

            return _instance;
        }
    }
    #endregion

    private Dictionary<EPrefabsType, Dictionary<string, List<PoolObject>>> _dicPool = new Dictionary<EPrefabsType, Dictionary<string, List<PoolObject>>>();

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        foreach (EPrefabsType type in Enum.GetValues(typeof(EPrefabsType)))
        {
            _dicPool.Add(type, new Dictionary<string, List<PoolObject>>());
        }

       // NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ChangeSceneState);
    }

    public void OnNotification(Notification noti)
    {
        List<PoolObject> removeList = new List<PoolObject>();
        foreach (var dic in _dicPool)
        {
            foreach (var pool in dic.Value)
            {
                foreach (var poolObj in pool.Value)
                {
                    if (poolObj.IsDie())
                    {
                        removeList.Add(poolObj);
                    }
                }
                foreach (var remove in removeList)
                {
                    pool.Value.Remove(remove);
                    Destroy(remove.gameObject);
                }
            }
        }
    }


    public GameObject GrabPrefabs(EPrefabsType type, string name, Transform layer)
    {
        if (!_dicPool[type].ContainsKey(name))
        {
            _dicPool[type].Add(name, new List<PoolObject>());
        }

        if (_dicPool[type][name].Count < 1)
        {
            _dicPool[type][name].Add(CreatePoolObject(type, name));
        }

        PoolObject obj = _dicPool[type][name][0];
        _dicPool[type][name].Remove(obj);
        obj.EnableObject(layer);

        return (obj.gameObject);
    }

    public void DespawnObject(EPrefabsType type, GameObject obj)
    {
        if (obj.TryGetComponent<PoolObject>(out PoolObject poolObj))
        {
            if (_dicPool[type].ContainsKey(poolObj.Name))
            {
                poolObj.DisableObject(this.transform);
                _dicPool[type][poolObj.Name].Add(poolObj);
            }
        }
    }

    private PoolObject CreatePoolObject(EPrefabsType type, string name)
    {
        GameObject go = Resources.Load(GetPath(type) + name, typeof(GameObject)) as GameObject;

        if (go == null)
            return null;

        go = Instantiate(go, this.transform);

        if (go.TryGetComponent<PoolObject>(out PoolObject poolObj))
        {
            poolObj.SetData(name);
            return poolObj;
        }
        else
        {
            poolObj = go.AddComponent<PoolObject>();
            poolObj.SetData(name);
            return poolObj;
        }
    }

    private string GetPath(EPrefabsType type)
    {
        switch (type)
        {
            //MainTab
            case EPrefabsType.TabApplication:
                return "Prefabs/InGame/TabApp/";
        }
        return "Prefabs/";
    }
}
public enum EPrefabsType
{
    TabApplication,
}

