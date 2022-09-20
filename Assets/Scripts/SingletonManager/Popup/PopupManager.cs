using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Rendering.Universal.Internal;
using System.Text.RegularExpressions;

public class PopupManager
{
    #region lifeCycle
    public void Init()
    {
        SetCanvas();

        foreach (PopupType type in Enum.GetValues(typeof(PopupType)))
        {
            if (!_stackDic.ContainsKey(type))
                _stackDic.Add(type, new Stack<PopupBase>());
            //if (!_groupDic.ContainsKey(type) && _canvas.transform.childCount >= (int)type)
            //{
            //    _groupDic.Add(type, _canvas.transform.GetChild((int)type).gameObject);
            //}
        }

        foreach (var item in _stackDic)
        {
            foreach (var popup in item.Value)
            {
                popup.Init();
            }
        }
        //NotificationCenter.Instance.AddObserver(OnNotification, ENotiMessage.ChangeSceneState);

    }

    public void AdvanceTime(float dt_sec )
    {
        foreach (var item in _stackDic)
        {
            foreach (var popup in item.Value)
            {
                popup.AdvanceTime(dt_sec);
            }
        }
    }
    #endregion

    #region private field
    private Canvas _canvas = null;
    private GameObject go = null;
    private Dictionary<PopupType, Stack<PopupBase>> _stackDic = new Dictionary<PopupType, Stack<PopupBase>>();
    private Dictionary<PopupType, GameObject> _groupDic = new Dictionary<PopupType, GameObject>();
    #endregion

    #region private methods
    private void SetCanvas()
    {
        CreateCanvas();
    }

    private void CreateCanvas()
    {
        GameObject go = GameObject.Find("PopupCanvas");
        if (go == null)
            go = Managers.Pool.GrabPrefabs(EPrefabsType.POPUP, "PopupCanvas", Managers.ManagerObj.transform);

        if (go.TryGetComponent(out Canvas canvas))
        {
            _canvas = canvas;
        }
        else
        {
            _canvas = go.AddComponent<Canvas>();
            Debug.LogError("not canvas");
        }

        // Popup Manager의 _canvas의 prefab이 변경될 때 popup을 세팅해줘야 함. 아니면 혹시 Init주석처럼??
        _groupDic.Add(PopupType.NORMAL, _canvas.transform.GetChild(0).gameObject);
        _groupDic.Add(PopupType.IMPORTANT, _canvas.transform.GetChild(1).gameObject);
    }

    private void OnNotification(Notification noti)
    {
        //switch (noti.msg)
        //{
        //    case ENotiMessage.ChangeSceneState:
        //        _canvas = null;
        //        DeleteAll();
        //        SetCanvas();
        //        if (_canvas == null) Debug.LogError("[Self] expected PopupCanvas");
        //        break;
        //}
    }

    private GameObject CreatePopupObj(EPrefabsType type, string name, Transform layer)
    {
        go = Managers.Pool.GrabPrefabs(type, name, layer);
        go.transform.position = layer.position;
        go.transform.localScale = new Vector3(1, 1, 1);
        return go;
    }
    #endregion

    #region property
    public int Count
    {
        get
        {
            int ans = 0;
            foreach (var item in _stackDic)
            {
                ans += item.Value.Count;
            }
            return ans;
        }
    }

    public int NormalCount
    {
        get
        {
            return _stackDic[PopupType.NORMAL].Count;
        }
    }

    public int ImportantCount
    {
        get
        {
            return _stackDic[PopupType.IMPORTANT].Count;
        }
    }
    #endregion

    #region public method
    public GameObject CreatePopup(EPrefabsType type, string name, PopupType popupType)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            CreateCanvas();
        }

        go = CreatePopupObj(type, name, _groupDic[popupType].transform);
        PopupBase popup = go.GetComponent<PopupBase>();
        _stackDic[popupType].Push(popup);
        popup.Init();
        popup.PopupType = popupType;
        return go;
    }

    public void DeleteCall(PopupType type)
    {
        if (_stackDic[type].Count > 0)
            _stackDic[type].Pop();
    }

    public void Delete(PopupType type)
    {
        if (_stackDic[type].Count > 0)
            _stackDic[type].Pop().Dispose();
    }

    public void DeleteAll(PopupType type)
    {
        while (_stackDic[type].Count > 0)
            _stackDic[type].Pop().Dispose(); 
    }

    public void DeleteAll()
    {
        foreach (var item in _stackDic)
        {
            DeleteAll(item.Key);
        }
        
    }
    #endregion
}
public enum PopupType
{
    NORMAL = 0,
    IMPORTANT,

    UNKNOWN = 101,
}
