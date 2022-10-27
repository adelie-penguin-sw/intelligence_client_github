using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrainEventUIItem : EventObject
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        switch(_objType)
        {
            case EEventObjectType.CREATE_BRAIN_UI:
                Managers.Notification.PostNotification(ENotiMessage.DRAG_END_CREATEBRAIN);
                break;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        switch (_objType)
        {
            case EEventObjectType.CREATE_BRAIN_UI:
                Managers.Notification.PostNotification(_isPointerEntering ? ENotiMessage.CANCEL_CREATEBRAIN : ENotiMessage.DRAG_END_CREATEBRAIN);
                break;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        switch (_objType)
        {
            case EEventObjectType.CREATE_BRAIN_UI:
                Managers.Notification.PostNotification(ENotiMessage.DRAG_START_CREATEBRAIN);
                break;
        }
    }
}
