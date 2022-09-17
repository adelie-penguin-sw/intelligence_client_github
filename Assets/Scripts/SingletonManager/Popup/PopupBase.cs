using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class PopupBase : MonoBehaviour
{
    #region static variables
    #endregion

    #region constant
    #endregion

    #region private field
    [SerializeField] private PopupType _popupType = PopupType.UNKNOWN;
    [SerializeField] private EPrefabsType _prefabType = EPrefabsType.POPUP;
    #endregion

    #region property
    public PopupType PopupType
    { 
        set
        {
            _popupType = value;
        }
    }
    #endregion

    public virtual void Init()
    {
    }

    public virtual void Set()
    {
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
    }

    public virtual void AdvanceTime(float dt_sec)
    {

    }

    public virtual void Dispose()
    {
        PopupManager.Instance.Delete(_popupType, this);
        PoolManager.Instance.DespawnObject(_prefabType, gameObject);
    }
}
