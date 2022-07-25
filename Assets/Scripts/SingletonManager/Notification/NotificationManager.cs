using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static NotificationManager;

/// <summary>
/// ??? ??? ??? ??? <br />
/// ????? ???? ?? <br />
/// </summary>
public class NotificationManager : MonoBehaviour
{
    #region Singelton
    private static bool _appIsClosing = false;
    private static NotificationManager _instance;
    public static NotificationManager Instance
    {
        get
        {
            if (_appIsClosing)
            {
                return null;
            }

            if (_instance == null)
            {
                _instance = FindObjectOfType<NotificationManager>();
                if (FindObjectsOfType<NotificationManager>().Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                    return _instance;
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject("Default Notification Center");
                    _instance = go.AddComponent<NotificationManager>();
                }
            }

            return _instance;
        }
    }

    void OnApplicationQuit()
    {
        // release reference on exit
        _appIsClosing = true;
    }
    #endregion

    Hashtable _notifications = new Hashtable();

    /// <summary>
    /// ???? ??? ??? ??? ?? ??
    /// </summary>
    /// <param name="noti"></param>
    public delegate void DelFunction(Notification noti);

    #region Add Observer
    /// <summary>
    /// ??? ?? ???
    /// </summary>
    /// <param name="observer">?? ?? Delegate</param>
    /// <param name="msg">?? ???</param>
    public void AddObserver(DelFunction observer, ENotiMessage msg/*, Component sender*/)
    {
        if (_notifications[msg] == null)
        {
            _notifications[msg] = new List<DelFunction>();
        }

        List<DelFunction> notifyList = _notifications[msg] as List<DelFunction>;

        if (!notifyList.Contains(observer)) { notifyList.Add(observer); }
    }
    #endregion

    #region Remove Observer
    /// <summary>
    /// ??? ?? ???
    /// </summary>
    /// <param name="observer">?? ?????? Delegate</param>
    /// <param name="msg">?? ???</param>
    public void RemoveObserver(DelFunction observer, ENotiMessage msg)
    {
        List<DelFunction> notifyList = (List<DelFunction>)_notifications[msg];

        if (notifyList != null)
        {
            if (notifyList.Contains(observer))
            {
                notifyList.Remove(observer);
            }

            if (notifyList.Count == 0)
            {
                _notifications.Remove(msg);
            }
        }
    }
    #endregion

    #region Post Notification
    /// <summary>
    /// ?? ???? ???? ?? ?? ??
    /// </summary>
    /// <param name="aMsg">???</param>
    public void PostNotification(ENotiMessage aMsg)
    {
        PostNotification(aMsg, null);
    }

    /// <summary>
    /// ?? ???? ???? ?? ?? ??
    /// </summary>
    /// <param name="aMsg">???</param>
    /// <param name="aData">??? ???</param>
    public void PostNotification(ENotiMessage aMsg, Hashtable aData)
    {
        //PostNotification(new Notification(aMsg, aData));
        PostNotification(Notification.Instantiate(aMsg, aData));
    }

    /// <summary>
    /// ?? ???? ???? ?? ?? ??
    /// </summary>
    /// <param name="aNotification">Notification class</param>
    public void PostNotification(Notification aNotification)
    {
        List<DelFunction> notifyList = (List<DelFunction>)_notifications[aNotification.msg];
        if (notifyList == null)
        {
            Debug.Log("Notify list not found in PostNotification: " + aNotification.msg);
            return;
        }

        List<DelFunction> observersToRemove = new List<DelFunction>();

        foreach (DelFunction observer in notifyList)
        {
            if (observer == null)
            {
                observersToRemove.Add(observer);
            }
            else
            {
                observer(aNotification);
            }
        }

        foreach (DelFunction observer in observersToRemove)
        {
            notifyList.Remove(observer);
        }
    }
    #endregion

}

/// <summary>
/// ??? ??? ?? ??? ? ? ?? Data ???
/// </summary>
public class Notification
{
    public ENotiMessage msg;
    public Hashtable data;
    private static Notification _instance = new Notification(ENotiMessage.UNKNOWN);

    private Notification(ENotiMessage aMsg)
    {
        data = new Hashtable();
        msg = aMsg;
    }

    /// <summary>
    /// Notification ??? ??<br />
    /// ?? ???? ???? ??<br />
    /// </summary>
    /// <param name="aMsg">???</param>
    /// <param name="aData">?? ??? ?????</param>
    /// <returns>Notification? ???? ??</returns>
    public static Notification Instantiate(ENotiMessage aMsg, Hashtable aData)
    {
        _instance.msg = aMsg;
        if (aData != null)
        {
            _instance.data.Clear();
            _instance.data = aData;
        }
        return _instance;
    }

    /// <summary>
    /// Notification ??? ??<br />
    /// ?? ???? ??? ??<br />
    /// </summary>
    /// <typeparam name="T">?? ???? ???</typeparam>
    /// <param name="aMsg">???</param>
    /// <param name="dataParam">?? ???? ?? Enum</param>
    /// <param name="data">???</param>
    /// <returns></returns>
    public static Notification Instantiate<T>(ENotiMessage aMsg, EDataParamKey dataParam, T data)
    {
        _instance.msg = aMsg;

        if (data != null)
        {
            _instance.data.Clear();
            _instance.data.Add(dataParam, data);
        }
        return _instance;
    }

    /// <summary>
    /// Notification ??? ??<br />
    /// ???? ?? ??<br />
    /// </summary>
    /// <param name="aMsg">???</param>
    /// <returns></returns>
    public static Notification Instantiate(ENotiMessage aMsg)
    {
        _instance.msg = aMsg;
        _instance.data.Clear();
        return _instance;
    }

}

/// <summary>
/// ??? ??? Enum
/// </summary>
public enum ENotiMessage
{
    UNKNOWN = 0,

    //  <<MainTab>>
    //  Input????
    DRAG_START_CREATEBRAIN,
    DRAG_END_CREATEBRAIN,

    MOUSE_DOWN_BRAIN,
    MOUSE_UP_BRAIN,
    MOUSE_EXIT_BRAIN,
    MOUSE_ENTER_BRAIN,

    CREATE_BRAIN,
    CREATE_CHANNEL,

    ONCLICK_SELL_BRAIN,

}

/// <summary>
/// ??? ?? ??? Enum
/// </summary>
public enum EDataParamKey
{
    VECTOR2,
    CLASS_BRAIN,
    CLASS_CHANNEL,
    STRUCT_BRAINRELATION,
}
