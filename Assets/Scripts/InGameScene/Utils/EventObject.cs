using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    protected EEventObjectType _objType;
    [SerializeField]
    protected Vector2 _posStartDrag;
    [SerializeField]
    protected Vector2 _posCurDrag;
    [SerializeField]
    protected Vector2 _posEndDrag;
    [SerializeField]
    protected Vector2 _posStartDown;

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _posStartDrag = eventData.position;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        _posCurDrag = eventData.position;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _posEndDrag = eventData.position;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _posStartDown = eventData.position;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }
}

public enum EEventObjectType
{
    CREATE_BRAIN_UI,

}