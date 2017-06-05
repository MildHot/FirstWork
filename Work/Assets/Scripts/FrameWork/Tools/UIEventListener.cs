using UnityEngine;
using UnityEngine.EventSystems;
public class UIClickListener : MonoBehaviour, IPointerClickHandler
{
    public System.Action<GameObject, PointerEventData> onClick;
    static public UIClickListener Get(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        UIClickListener listener = go.GetComponent<UIClickListener>();
        if (listener == null)
            listener = go.AddComponent<UIClickListener>();
        return listener;
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject,eventData);
    }
}
public class UITouchListener : MonoBehaviour, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float touchingDelta = 0;
    public float touchingBeginDelta = 0.5f;
    public bool pressed;
    public System.Action<GameObject, PointerEventData> onTouchBegin;
    public System.Action<GameObject, PointerEventData> onTouchEnd;
    public System.Action<GameObject> onTouching;
    static public UITouchListener Get(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        UITouchListener listener = go.GetComponent<UITouchListener>();
        if (listener == null)
            listener = go.AddComponent<UITouchListener>();
        return listener;
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        if (onTouchBegin != null)
            onTouchBegin(gameObject, eventData);
        if (onTouching != null)
        {
            if (touchingBeginDelta >= 0)
            {
                this.Invoke("SendTouchingMessage", touchingBeginDelta);
            }
        }
    }
    void SendTouchingMessage()
    {
        if (onTouching != null)
        {
            onTouching(gameObject);
            if (touchingDelta > 0)
            {
                this.Invoke("SendTouchingMessage", touchingDelta);
            }
        }
    }
   
    public void OnPointerExit(PointerEventData eventData)
    {
        if (pressed && onTouchEnd != null)
        {
            onTouchEnd(gameObject, eventData);
        }
        if (onTouching != null)
        {
            this.CancelInvoke("SendTouchingMessage");
        }
        pressed = false;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (pressed && onTouchEnd != null)
        {
            onTouchEnd(gameObject, eventData);
        }
        if (onTouching != null)
        {
            this.CancelInvoke("SendTouchingMessage");
        }
        pressed = false;
    }
}
public class UIDragListener : MonoBehaviour, IDragHandler
{
    public System.Action<PointerEventData> onDrag;
    static public UIDragListener Get(GameObject go)
    {
        if (go == null)
        {
            return null;
        }
        UIDragListener listener = go.GetComponent<UIDragListener>();
        if (listener == null)
            listener = go.AddComponent<UIDragListener>();
        return listener;
    }
   
    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag(eventData);
        }
    }
}