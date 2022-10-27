using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationManager
{
    private static bool _appIsClosing = false;

    void OnApplicationQuit()
    {
        // release reference on exit
        _appIsClosing = true;
    }

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
    CANCEL_CREATEBRAIN,

    MOUSE_DOWN_BRAIN,
    MOUSE_UP_BRAIN,
    MOUSE_EXIT_BRAIN,
    MOUSE_ENTER_BRAIN,

    ONCLICK_SELL_BRAIN,
    ONCLICK_RESET_BUTTON,
    ONCLICK_RESET_NETWORK,
    ONCLICK_UPGRADE_BRAIN_MULTIPLIER,
    ONCLICK_UPGRADE_BRAIN_LIMIT,
    ONCLICK_TPUPGRADE,
    ONCLICK_CHANGE_TAB,

    EXPERIMENT_COMPLETE,

    QUEST_CREATE_BRAIN,
    QUEST_CREATE_CHANNEL,
    QUEST_BRAIN_INTELLIGENCE_UPGRADE,
    QUEST_BRAIN_SELL,

    // <<ShopTab>>
    ONCLICK_AD_REWARD_DOUBLE_TP,
    ONCLICK_AD_REWARD_SKIP,
    ONCLICK_AD_REWARD_SPEED,

    ONCLICK_PAID_REWARD_REMOVE_AD,
    ONCLICK_PAID_REWARD_SKIP,
    ONCLICK_PAID_REWARD_BUY_TP1,
    ONCLICK_PAID_REWARD_BUY_TP2,
    ONCLICK_PAID_REWARD_BUY_TP3,
    ONCLICK_PAID_REWARD_BUY_TP4,

    // <<UserTab>>
    ONCLICK_TOP_RANK,
    ONCLICK_MY_RANK,
    ONCLICK_PERCENT_RANK,
    ONCLICK_HIGHER_RANK,
    ONCLICK_LOWER_RANK, 

    //server 통신 관련
    UPDATE_BRAIN_NETWORK,

    //UserData 관련
    UPDATE_TP,
    UPDATE_NP,

    LOGOUT,
}

/// <summary>
/// ??? ?? ??? Enum
/// </summary>
public enum EDataParamKey
{
    VECTOR2,
    INTEGER,

    CLASS_BRAIN,
    CLASS_CHANNEL,
    STRUCT_BRAINRELATION,
    BRAIN_ID,
    BULK_UPGRADE_COUNT, 
    EGAMESTATE,

}
