using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{

    private Dictionary<EPrefabsType, Dictionary<string, List<PoolObject>>> _dicPool = new Dictionary<EPrefabsType, Dictionary<string, List<PoolObject>>>();
    private Transform _layer;

    public void Awake()
    {
        _layer = new GameObject("PoolLayer").transform;
        foreach (EPrefabsType type in Enum.GetValues(typeof(EPrefabsType)))
        {
            _dicPool.Add(type, new Dictionary<string, List<PoolObject>>());
        }

    }
    //public void Awake()
    //{
    //    foreach (EPrefabsType type in Enum.GetValues(typeof(EPrefabsType)))
    //    {
    //        _dicPool.Add(type, new Dictionary<string, List<PoolObject>>());
    //    }

    //   // NotificationManager.Instance.AddObserver(OnNotification, ENotiMessage.ChangeSceneState);
    //}

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
                    GameObject.Destroy(remove.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// 해당하는 오브젝트가 풀에 존재한다면 풀에서 꺼내오고<br />
    /// 없으면 생성시켜 반환해준다.<br />
    /// </summary>
    /// <param name="type">프리팹 타입</param>
    /// <param name="name">프리팹 이름</param>
    /// <param name="layer">생성시킬 레이어</param>
    /// <returns>꺼내온 GameObject 반환</returns>
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

    /// <summary>
    /// 해당 오브젝트를 다시 풀에 돌려준다.
    /// </summary>
    /// <param name="type">프리팹 타입</param>
    /// <param name="obj">반환 할 오브젝트</param>
    public void DespawnObject(EPrefabsType type, GameObject obj)
    {
        if (obj.TryGetComponent<PoolObject>(out PoolObject poolObj))
        {
            if (_dicPool[type].ContainsKey(poolObj.Name))
            {
                poolObj.DisableObject(_layer);
                _dicPool[type][poolObj.Name].Add(poolObj);
            }
        }
    }

    private PoolObject CreatePoolObject(EPrefabsType type, string name)
    {
        GameObject go = Resources.Load(GetPath(type) + name, typeof(GameObject)) as GameObject;

        if (go == null)
            return null;

        go = GameObject.Instantiate(go, _layer);

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
            case EPrefabsType.TAP_APPLICATION:
                return "Prefabs/InGame/TabApp/";
            case EPrefabsType.CHANNEL:
            case EPrefabsType.BRAIN:
                return "Prefabs/InGame/";
            case EPrefabsType.POPUP:
            case EPrefabsType.RANK_ITEM:
                return "Prefabs/Popup/";
            case EPrefabsType.UI:
                return "Prefabs/InGame/UI/";
        }
        return "Prefabs/";
    }
}

/// <summary>
/// 프리팹 타입을 나타내는 Enum
/// </summary>
public enum EPrefabsType
{
    //Popup
    POPUP,

    //RankItem
    RANK_ITEM,

    //Application
    TAP_APPLICATION,

    //MainTab
    BRAIN,
    CHANNEL,

    UI,

}

