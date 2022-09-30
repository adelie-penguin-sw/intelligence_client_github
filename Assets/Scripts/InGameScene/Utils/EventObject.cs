using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UGUI에 해당하는 오브젝트의 터치 이벤트를 관리해주는 Class
/// </summary>
public class EventObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
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
    [SerializeField]
    protected bool _isPointerEntering = true;
    /// <summary>
    /// 드래그 시작시 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _posStartDrag = eventData.position;
    }

    /// <summary>
    /// 드래그중에 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnDrag(PointerEventData eventData)
    {
        _posCurDrag = eventData.position;
    }

    /// <summary>
    /// 드래그가 끝났을때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        _posEndDrag = eventData.position;
    }

    /// <summary>
    /// 이 오브젝트가 눌렸을때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        _posStartDown = eventData.position;
        _isPointerEntering = true;
    }

    /// <summary>
    /// 이 오브젝트에 포인터가 들어왔을 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerEntering = true;
    }


    /// <summary>
    /// 이 오브젝트로부터 포인터가 나갔을때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        _isPointerEntering = false;
    }

    /// <summary>
    /// 터치가 끝났을때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
    }

}

/// <summary>
/// 실행되는 EvenetObject의 종류
/// </summary>
public enum EEventObjectType
{
    /// <summary>
    /// 브레인 생성 UI
    /// </summary>
    CREATE_BRAIN_UI,
    
}