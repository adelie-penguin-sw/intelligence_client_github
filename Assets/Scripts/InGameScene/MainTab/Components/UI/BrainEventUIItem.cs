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
        NotificationManager.Instance.PostNotification(ENotiMessage.DRAG_END_CREATEBRAIN);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        NotificationManager.Instance.PostNotification(ENotiMessage.DRAG_START_CREATEBRAIN);
    }
}
