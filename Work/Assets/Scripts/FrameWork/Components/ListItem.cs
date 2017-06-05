using UnityEngine;
using System.Collections;
using FrameWork;
namespace UnityEngine.UI
{
    public class ListItem : MonoBehaviour
    {
        public GameObject selectedState = null;
        private Vector2 _itemSize = Vector2.zero;
        private int _index;
        private bool _selected = false;

        void Awake()
        {
            Selected = _selected;
        }

        public Vector2 ItemSize
        {
            get { return _itemSize; }
            set { _itemSize = value; }
        }
        
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value;
                if (selectedState != null) selectedState.SetActive(_selected);
            }
        }

        public OnTouchEventHandle OnClick {
            set { EventTriggerListener.Get(this.gameObject).SetEventHandle(EnumTouchEventType.OnClick, value); }
        }
        
    }
}

