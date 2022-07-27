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

    #region attributes
    [SerializeField] private Button[] _closeBtn; // 닫는 버튼
    /*[SerializeField] private EPopupType _popupType = EPopupType.Normal;*/ //필요없어 보임
    [SerializeField] private EPrefabsType _prefabType = EPrefabsType.POPUP;

    private PopupBase _next;
    private PopupBase _prev;

    #endregion

    #region [get, set]
    public PopupBase Next
    {
        get { return _next; }
        set { _next = value; }
    }
    public PopupBase Prev
    {
        get { return _prev; }
        set { _prev = value; }
    }
    #endregion

    public virtual void Init()
    {
    }

    public virtual void Set()
    {
        if (_closeBtn == null)
        {
            Debug.LogError("[self] expected close button");
        }
        gameObject.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        foreach (var closeBtn in _closeBtn)
        {
            closeBtn.onClick.AddListener(Dispose);
        }
    }

    public virtual void AdvanceTime(float dt_sec)
    {

    }

    public virtual void Dispose()
    {
        if (this == PopupManager.Instance.Head && this == PopupManager.Instance.Tail)
        {
            PopupManager.Instance.Head = null;
            PopupManager.Instance.Tail = null;
        }
        else if (this == PopupManager.Instance.Head)
        {
            PopupManager.Instance.Head = this.Next;
            this.Next.Prev = null;
        }
        else if (this == PopupManager.Instance.Tail)
        {
            PopupManager.Instance.Tail = this.Prev;
            this.Prev.Next = null;
        }
        else
        {
            if (this.Next != null)
                this.Prev.Next = this.Next;
            if (this.Prev != null)
                this.Next.Prev = this.Prev;
        }
        _next = null;
        _prev = null;
        foreach (var closeBtn in _closeBtn)
        {
            closeBtn.onClick.RemoveAllListeners();
        }
        PoolManager.Instance.DespawnObject(_prefabType, gameObject);
    }
}
