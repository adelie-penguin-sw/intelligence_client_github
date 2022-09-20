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
            if (_stackDic.ContainsKey(type))
                _stackDic.Add(type, new Stack<PopupBase>());
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
    private GameObject _importantPopupGroup = null;
    private GameObject _normalPopupGroup = null;
    private Dictionary<PopupType, Stack<PopupBase>> _stackDic = new Dictionary<PopupType, Stack<PopupBase>>();
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
        _normalPopupGroup = _canvas.transform.GetChild(0).gameObject;
        _importantPopupGroup = _canvas.transform.GetChild(1).gameObject;
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

    private GameObject CreatePopup(EPrefabsType type, string name, Transform layer)
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
    public GameObject CreatePopup(EPrefabsType type, string name, PopupType popuoType)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            CreateCanvas();
        }

        go = Create
    }

    public GameObject CreateNormalPopup(EPrefabsType type, string name)
    { 
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            CreateCanvas();
            //
            //return null;
        }
        if (_normalPopupGroup == null)
        {
            Debug.LogError("[Self] expected group");
            return null;
        }
        go = CreatePopup(type, name, _normalPopupGroup.transform);
        PopupBase popup = go.GetComponent<PopupBase>();
        _stackDic[PopupType.NORMAL].Push(popup);
        popup.Init();
        popup.PopupType = PopupType.NORMAL;
        return go;
    }

    public GameObject CreateImportantPopup(EPrefabsType type, string name)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            return null;
        }
        if (_importantPopupGroup == null)
        {
            Debug.LogError("[Self] expected group");
            return null;
        }
        go = CreatePopup(type, name, _importantPopupGroup.transform);
        PopupBase popup = go.GetComponent<PopupBase>();
        _stackDic[PopupType.IMPORTANT].Push(popup);
        popup.Init();
        popup.PopupType = PopupType.IMPORTANT;
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
    UNKNOWN = 0,
    NORMAL,
    IMPORTANT,
}
