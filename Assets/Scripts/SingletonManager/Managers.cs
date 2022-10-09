using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    private static Managers Instance { get { Init(); return _instance; } }

    [ShowInInspector] private PoolManager _poolManager = new PoolManager();
    [ShowInInspector] private PopupManager _popupManager = new PopupManager();
    [ShowInInspector] private NetworkManager _networkManager = new NetworkManager();
    [ShowInInspector] private NotificationManager  _notiManager = new NotificationManager();
    [ShowInInspector] private DefinitionManager _definitionManager = new DefinitionManager();

    public static PoolManager Pool { get { return Instance._poolManager; } }
    public static PopupManager Popup { get { return Instance._popupManager; } }
    public static NetworkManager Network { get { return Instance._networkManager; } }
    public static NotificationManager Notification { get { return Instance._notiManager; } }
    public static DefinitionManager Definition { get { return Instance._definitionManager; } }

    public static GameObject ManagerObj;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if(_popupManager!=null)
        {
            _popupManager.AdvanceTime(Time.deltaTime);
        }
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
            ManagerObj = obj;
            DontDestroyOnLoad(obj);
            _instance = obj.GetComponent<Managers>();
            _instance._poolManager.Init();
            _instance._popupManager.Init();
        }
    }
}
