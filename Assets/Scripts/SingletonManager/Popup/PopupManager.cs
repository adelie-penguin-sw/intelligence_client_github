using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        //NotificationCenter.Instance.AddObserver(OnNotification, ENotiMessage.ChangeSceneState);

        _tmpPopup = _head;
        while (_tmpPopup != null)
        {
            _tmpPopup.Init();
            _tmpPopup = _tmpPopup.Next;
        }
    }

    private void Update()
    {
        _tmpPopup = _head;
        while (_tmpPopup != null)
        {
            _tmpPopup.AdvanceTime(Time.deltaTime);
            _tmpPopup = _tmpPopup.Next;
        }
    }
    #endregion

    #region private
    private void InitInstance(GameObject go)
    {
        PopupBase _popupInstance = go.GetComponent<PopupBase>();
        _popupInstance.Init();
        _popupInstance.Set();
        if (_head == null)
        {
            _head = _popupInstance;
            _tail = _popupInstance;
        }
        else
        {
            _tail.Next = _popupInstance;
            _popupInstance.Prev = _tail;
            _tail = _popupInstance;
        }
    }

    private GameObject CreatePopup(EPrefabsType type, string name, Transform layer)
    {
        go = PoolManager.Instance.GrabPrefabs(type, name, _canvas.transform);
        go.transform.position = _canvas.transform.position;
        go.transform.localScale = new Vector3(1, 1, 1);
        return go;
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

        //_canvas = GameObject.Find("PopupCanvas").GetComponent<Canvas>();
        //if(_canvas == null)
        //{
        //    GameObject go = new GameObject("PopupCanvas");
        //    _canvas = go.AddComponent<Canvas>();
        //}
        //if (_canvas != null) return;
        //_canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private Canvas _canvas = null;
    private GameObject go = null;
    private PopupBase _head = null;
    private PopupBase _tail = null;
    private PopupBase _tmpPopup = null;
    #endregion

    #region property
    public PopupBase Head
    {
        get { return _head; }
        set { _head = value; }
    }
    public PopupBase Tail
    {
        get { return _tail; }
        set { _tail = value; }
    }
    #endregion


    public GameObject CreatePopup(EPrefabsType type, string name)
    {
        if (_canvas == null)
        {
            Debug.LogError("[Self] expected canvas");
            return null;
        }
        go = CreatePopup(type, name,  _canvas.transform);
        InitInstance(go);
        return go;
    }

    public void DeleteHead()
    {
        if (_head == null)
        {
            Debug.LogError("[Self] list count zero");
            return;
        }
        _head.Dispose();
    }

    public void DeleteAll()
    {
        while (_head != null)
        {
            DeleteHead();
        }
    }
}
