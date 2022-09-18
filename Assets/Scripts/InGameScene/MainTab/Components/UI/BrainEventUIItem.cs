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
        Managers.Notification.PostNotification(ENotiMessage.DRAG_END_CREATEBRAIN);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Managers.Notification.PostNotification(ENotiMessage.DRAG_END_CREATEBRAIN);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        Managers.Notification.PostNotification(ENotiMessage.DRAG_START_CREATEBRAIN);
    }
}
