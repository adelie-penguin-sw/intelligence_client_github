using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
        foreach (var importantPopup in _importantStack)
        {
            importantPopup.Init();
        }
        foreach (var normalPopup in _normalStack)
        {
            normalPopup.Init();
        }
        //NotificationCenter.Instance.AddObserver(OnNotification, ENotiMessage.ChangeSceneState);

    }

    public void AdvanceTime(float dt_sec )
    {
        foreach (var importantPopup in _importantStack)
        {
            importantPopup.AdvanceTime(dt_sec);
        }
        foreach(var normalPopup in _normalStack)
        {
            normalPopup.AdvanceTime(dt_sec);
        }
    }
    #endregion

    #region private field
    private Canvas _canvas = null;
    private GameObject go = null;
    private GameObject _importantPopupGroup = null;
    private GameObject _normalPopupGroup = null;
    private Stack<PopupBase> _importantStack = new Stack<PopupBase>();
    private Stack<PopupBase> _normalStack = new Stack<PopupBase>();
    #endregion

    #region private method
    private void SetCanvas()
    {
        CreateCanvas();

        _normalPopupGroup = new GameObject("NormalPopupGroup");
        _importantPopupGroup = new GameObject("ImportantPopupGroup");
        _normalPopupGroup.AddComponent<RectTransform>();
        _importantPopupGroup.AddComponent<RectTransform>();
        _normalPopupGroup.transform.SetParent(_canvas.transform, false);
        _importantPopupGroup.transform.SetParent(_canvas.transform, false);
        CopySize(_normalPopupGroup);
        CopySize(_importantPopupGroup);
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

    private void CopySize(GameObject group)
    {
        RectTransform rectTransform = _canvas.GetComponent<RectTransform>();
        RectTransform objRectTransform = group.GetComponent<RectTransform>();
        objRectTransform.sizeDelta = rectTransform.sizeDelta;
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
    public int Size
    {
        get
        {
            return _importantStack.Count + _normalStack.Count;
        }
    }

    public int NormalSize
    {
        get
        {
            return _normalStack.Count;
        }
    }

    public int ImportantSize
    {
        get
        {
            return _importantStack.Count; 
        }
    }
    #endregion

    #region public method
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
        _normalStack.Push(popup);
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
        _importantStack.Push(popup);
        popup.Init();
        popup.PopupType = PopupType.IMPORTANT;
        return go;
    }

    public void DeleteCall(PopupType type)
    {
        if (type == PopupType.NORMAL && _normalStack.Count > 0)
        {
            _normalStack.Pop();
        }
        else if (type == PopupType.IMPORTANT && _importantStack.Count > 0)
        {
            _importantStack.Pop();
        }
    }

    public void DeleteNormal()
    {
        if (_normalStack.Count > 0)
        {
            _normalStack.Pop().Dispose();
        }
    }

    public void DeleteImportant()
    {
        if (_importantStack.Count > 0)
        {
            _importantStack.Pop().Dispose();
        }
    }

    public void DeleteNormalAll()
    {
        while (_normalStack.Count > 0)
        {
            _normalStack.Pop().Dispose();
        }
    }

    public void DeleteImportantAll()
    {
        while (_importantStack.Count > 0)
        {
            _importantStack.Pop().Dispose();
        }
    }
        
    public void DeleteAll()
    {
        DeleteImportantAll();
        DeleteNormalAll();
    }
    #endregion
}
public enum PopupType
{
    UNKNOWN = 0,
    NORMAL,
    IMPORTANT,
}
