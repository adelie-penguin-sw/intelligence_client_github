using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.Rendering.Universal.Internal;
using System.Text.RegularExpressions;

public class PopupManager : MonoBehaviour
{
    #region Singelton
    private static PopupManager _instance;
    public static PopupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PopupManager>();
                if (FindObjectsOfType<PopupManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("PopupManager");
                    go.layer = LayerMask.NameToLayer("Popup");
                    _instance = go.AddComponent<PopupManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region lifeCycle
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

    private void Update()
    {
        foreach (var importantPopup in _importantStack)
        {
            importantPopup.AdvanceTime(Time.deltaTime);
        }
        foreach(var normalPopup in _normalStack)
        {
            normalPopup.AdvanceTime(Time.deltaTime);
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
        GameObject go = GameObject.Find("PopupCanvas");
        if (go == null)
            go = PoolManager.Instance.GrabPrefabs(EPrefabsType.POPUP, "PopupCanvas", transform);

        if (go.TryGetComponent(out Canvas canvas))
        {
            _canvas = canvas;
        }
        else
        {
            _canvas = go.AddComponent<Canvas>();
            Debug.LogError("not canvas");
        }

        _normalPopupGroup = new GameObject("NormalPopupGroup");
        _importantPopupGroup = new GameObject("ImportantPopupGroup");
        _normalPopupGroup.AddComponent<RectTransform>();
        _importantPopupGroup.AddComponent<RectTransform>();
        _normalPopupGroup.transform.SetParent(_canvas.transform, false);
        _importantPopupGroup.transform.SetParent(_canvas.transform, false);
        CopySize(_normalPopupGroup);
        CopySize(_importantPopupGroup);
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
        go = PoolManager.Instance.GrabPrefabs(type, name, layer);
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
            return null;
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

    public void Delete(PopupType type, PopupBase popup)
    {
        if (type == PopupType.NORMAL)
        {
            DeleteNormal(); 
        }
        else if (type == PopupType.IMPORTANT)
        {
            DeleteImportant(); 
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
