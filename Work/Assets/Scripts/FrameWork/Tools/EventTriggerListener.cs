using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace FrameWork
{
    public delegate void OnTouchEventHandle(GameObject _listener, object _args, params object[] _params);
    public enum EnumTouchEventType
    {
        OnClick,
        //OnDoubleClick,
        //OnDown,
        //OnUp,
        //OnEnter,
        //OnExit,
        //OnSelect,
        //OnUpdateSelect,
        //OnDeSelect,
        //OnDrag,
        //OnDragEnd,
        //OnDrop,
        //OnScroll,
        //OnMove,
        //OnTouchBegin,
        //OnTouchEnd,
        //OnTouching,
    }
    public class TouchHandle
    {
        private event OnTouchEventHandle eventHandle = null;
        private object[] handleParams;

        public TouchHandle(OnTouchEventHandle _handle, params object[] _params)
        {
            SetHandle(_handle, _params);
        }

        public TouchHandle()
        {

        }

        public void SetHandle(OnTouchEventHandle _handle, params object[] _params)
        {
            DestoryHandle();
            eventHandle += _handle;
            handleParams = _params;
        }

        public void CallEventHandle(GameObject _lsitener, object _args)
        {
            if (null != eventHandle)
            {
                eventHandle(_lsitener, _args, handleParams);
            }
        }


        public void DestoryHandle()
        {
            if (null != eventHandle)
            {
                eventHandle -= eventHandle;
                eventHandle = null;
            }
        }

    }
    public class EventTriggerListener : MonoBehaviour,
    IPointerClickHandler/*,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler,

    ISelectHandler,
    IUpdateSelectedHandler,
    IDeselectHandler,

    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IScrollHandler,
    IMoveHandler*/
    {
        public float touchingDelta = 0;
        public float touchingBeginDelta = 0.5f;
        public bool pressed;

        public TouchHandle onClick;
        public TouchHandle onDoubleClick;
        public TouchHandle onDown;
        public TouchHandle onEnter;
        public TouchHandle onExit;
        public TouchHandle onUp;
        public TouchHandle onSelect;
        public TouchHandle onUpdateSelect;
        public TouchHandle onDeSelect;
        public TouchHandle onDrag;
        public TouchHandle onDragEnd;
        public TouchHandle onDrop;
        public TouchHandle onScroll;
        public TouchHandle onMove;
        public TouchHandle onTouchBegin;
        public TouchHandle onTouchEnd;
        public TouchHandle onTouching;

        /// <summary>
        /// Get the specified go.
        /// </summary>
        /// <param name="go">Go.</param>
        static public EventTriggerListener Get(GameObject go)
        {
         
            return go.GetOrAddComponent<EventTriggerListener>();
        }

        void OnDestory()
        {
            this.RemoveAllHandle();
        }

        private void RemoveAllHandle()
        {
            this.RemoveHandle(onClick);
            this.RemoveHandle(onDoubleClick);
            this.RemoveHandle(onDown);
            this.RemoveHandle(onEnter);
            this.RemoveHandle(onExit);
            this.RemoveHandle(onUp);
            this.RemoveHandle(onDrop);
            this.RemoveHandle(onDrag);
            this.RemoveHandle(onDragEnd);
            this.RemoveHandle(onScroll);
            this.RemoveHandle(onMove);
            this.RemoveHandle(onUpdateSelect);
            this.RemoveHandle(onSelect);
            this.RemoveHandle(onDeSelect);
            this.RemoveHandle(onTouchBegin);
        }

        private void RemoveHandle(TouchHandle _handle)
        {
            if (null != _handle)
            {
                _handle.DestoryHandle();
                _handle = null;
            }
        }

        #region IDragHandler implementation

        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
                onDrag.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IEndDragHandler implementation

        public void OnEndDrag(PointerEventData eventData)
        {
            if (onDragEnd != null)
                onDragEnd.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IDropHandler implementation
        public void OnDrop(PointerEventData eventData)
        {
            if (onDrop != null)
                onDrop.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerClickHandler implementation
        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
                onClick.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerDownHandler implementation
        public void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
                onDown.CallEventHandle(this.gameObject, eventData);
            if (onTouchBegin != null)
                onTouchBegin.CallEventHandle(this.gameObject, eventData);
            if (onTouching != null)
                if (touchingBeginDelta >= 0)
                {
                    this.Invoke("SendTouchingMessage", touchingBeginDelta);
                }
        }
        #endregion

        void SendTouchingMessage()
        {
            if (onTouching != null)
            {
                onTouching.CallEventHandle(this.gameObject, null);
                if (touchingDelta > 0)
                {
                    this.Invoke("SendTouchingMessage", touchingDelta);
                }
            }
        }


        #region IPointerUpHandler implementation
        public void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null)
                onUp.CallEventHandle(this.gameObject, eventData);

            if (pressed && onTouchEnd != null)
            {
                onTouchEnd.CallEventHandle(gameObject, eventData);
            }
            if (onTouching != null)
            {
                this.CancelInvoke("SendTouchingMessage");
            }
            pressed = false;
        }
        #endregion

        #region IPointerEnterHandler implementation
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
                onEnter.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IPointerExitHandler implementation
        public void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null)
                onExit.CallEventHandle(this.gameObject, eventData);

            if (pressed && onTouchEnd != null)
            {
                onTouchEnd.CallEventHandle(gameObject, eventData);
            }
            if (onTouching != null)
            {
                this.CancelInvoke("SendTouchingMessage");
            }
            pressed = false;
        }
        #endregion

        #region ISelectHandler implementation
        public void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null)
                onSelect.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IUpdateSelectedHandler implementation
        public void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null)
                onUpdateSelect.CallEventHandle(this.gameObject, eventData);
        }
        #endregion

        #region IDeselectHandler implementation

        public void OnDeselect(BaseEventData eventData)
        {
            if (onDeSelect != null)
                onDeSelect.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IScrollHandler implementation

        public void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null)
                onScroll.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region IMoveHandler implementation

        public void OnMove(AxisEventData eventData)
        {
            if (onMove != null)
                onMove.CallEventHandle(this.gameObject, eventData);
        }

        #endregion

        #region  IPointerExitHandler, IPointerDownHandler, IPointerUpHandler


        #endregion

        public void SetEventHandle(EnumTouchEventType _type, OnTouchEventHandle _handle, params object[] _params)
        {
            switch (_type)
            {
                case EnumTouchEventType.OnClick:
                    if (null == onClick)
                    {
                        onClick = new TouchHandle();
                    }
                    onClick.SetHandle(_handle, _params);
                    break;
                //case EnumTouchEventType.OnDoubleClick:
                //    if (null == onDoubleClick)
                //    {
                //        onDoubleClick = new TouchHandle();
                //    }
                //    onDoubleClick.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnDown:
                //    if (onDown == null)
                //    {
                //        onDown = new TouchHandle();
                //    }
                //    onDown.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnUp:
                //    if (onUp == null)
                //    {
                //        onUp = new TouchHandle();
                //    }
                //    onUp.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnEnter:
                //    if (onEnter == null)
                //    {
                //        onEnter = new TouchHandle();
                //    }
                //    onEnter.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnExit:
                //    if (onExit == null)
                //    {
                //        onExit = new TouchHandle();
                //    }
                //    onExit.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnDrag:
                //    if (onDrag == null)
                //    {
                //        onDrag = new TouchHandle();
                //    }
                //    onDrag.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnDrop:
                //    if (onDrop == null)
                //    {
                //        onDrop = new TouchHandle();
                //    }
                //    onDrop.SetHandle(_handle, _params);
                //    break;

                //case EnumTouchEventType.OnDragEnd:
                //    if (onDragEnd == null)
                //    {
                //        onDragEnd = new TouchHandle();
                //    }
                //    onDragEnd.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnSelect:
                //    if (onSelect == null)
                //    {
                //        onSelect = new TouchHandle();
                //    }
                //    onSelect.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnUpdateSelect:
                //    if (onUpdateSelect == null)
                //    {
                //        onUpdateSelect = new TouchHandle();
                //    }
                //    onUpdateSelect.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnDeSelect:
                //    if (onDeSelect == null)
                //    {
                //        onDeSelect = new TouchHandle();
                //    }
                //    onDeSelect.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnScroll:
                //    if (onScroll == null)
                //    {
                //        onScroll = new TouchHandle();
                //    }
                //    onScroll.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnMove:
                //    if (onMove == null)
                //    {
                //        onMove = new TouchHandle();
                //    }
                //    onMove.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnTouchBegin:
                //    if (onTouchBegin == null)
                //    {
                //        onTouchBegin = new TouchHandle();
                //    }
                //    onTouchBegin.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnTouching:
                //    if (onTouching == null)
                //    {
                //        onTouching = new TouchHandle();
                //    }
                //    onTouching.SetHandle(_handle, _params);
                //    break;
                //case EnumTouchEventType.OnTouchEnd:
                //    if (onTouchEnd == null)
                //    {
                //        onTouchEnd = new TouchHandle();
                //    }
                //    onTouchEnd.SetHandle(_handle, _params);
                //    break;
            }
        }
    }
}

