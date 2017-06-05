using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using FrameWork;
namespace UnityEngine.UI
{
    /// <summary>
    /// 分布类型
    /// </summary>
    public enum ListType
    {
        Horizontal,
        Vertical,
    }

    public abstract class ListItemMgrStrategy
    {
        protected List cfg = null;
        protected ListBase listbase = null;
        protected int cols = 0;
        protected int rows = 0;
        protected ListType type;
        public ListItemMgrStrategy(List cfg, ListBase listbase)
        {
            this.cfg = cfg;
            this.listbase = listbase;
            this.type = this.cfg.type;
        }

        /// <summary>
        /// ListItemMgrStrategy 中保存ListBase 的Items
        /// </summary>
        public List<ListItem> Items
        {
            get { return this.listbase.Items(); }
        }

        public void SetColsAndRows(int cols, int rows)
        {
            this.cols = cols;
            this.rows = rows;
        }

        public abstract void Refresh();
        /// <summary>
        /// 分配item。添加点击
        /// </summary>
        /// <param name="count"></param>
        /// <param name="onClick"></param>
        public abstract void ReallocItems(int count, OnTouchEventHandle onClick);
        /// <summary>
        /// 设置位置
        /// </summary>
        public abstract void RepositionIitem();
        /// <summary>
        /// 设置content的大小
        /// </summary>
        public abstract void ReCalcContentSize();
    }

    public class NormalList : ListItemMgrStrategy
    {
        public NormalList(List cfg, ListBase listbase)
            : base(cfg, listbase)
        {
        }

        public override void ReallocItems(int count, OnTouchEventHandle onClick)
        {
            var contentRect = this.listbase.contentRect;

            this.cfg.ItemTemp.SetActive(true);
            for (int i = this.Items.Count; i < count; i++)
            {
                var item = GameObject.Instantiate(this.cfg.ItemTemp) as GameObject;
                item.name = "item" + i;

                var rect = item.transform as RectTransform;
                rect.pivot = new Vector2(0, 1);
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.localPosition = Vector3.zero;
                rect.localScale = Vector3.one;

                item.gameObject.transform.SetParent(contentRect, false);

                var t = item.GetOrAddComponent<ListItem>();
                t.Index = i;
                var txtNum = item.gameObject.transform.FindChild("txtNum").GetComponent<Text>();
                txtNum.text = t.Index.ToString();
                if (this.cfg.CanSelect)
                {
                    t.OnClick = onClick;
                }
                t.ItemSize = this.cfg.ItemSize;
                this.Items.Add(t);
            }

            this.cfg.ItemTemp.SetActive(false);

            for (int i = count; i < this.Items.Count; i++)
            {
                this.Items[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < count; i++)
            {
                var item = this.Items[i];
                item.gameObject.SetActive(true);
            }
        }


        public override void RepositionIitem()
        {
            int posX = 0;
            int posY = 0;
            Vector2 gridSize = new Vector2(this.cfg.ItemSize.x + this.cfg.ItemSpace.x, this.cfg.ItemSize.y + this.cfg.ItemSpace.y);
            //行(3行4列)
            Debug.Log(" 行  " + rows + " 列   " + cols);

            if (this.type == ListType.Vertical)
            {
                //垂直滚动，Item从左-->>右
                for (int i = 0; i < rows; i++)
                {
                    // 0，4，8，
                    int tmpIndex = i * cols;
                    //列
                    for (int j = 0; j < cols; j++)
                    {
                        // tempIndex=0==> 0+0,0+1,0+2,0+3
                        // tempIndex=4==> 4+0,4+1,4+2,4+3
                        int index = tmpIndex + j;

                        var item = this.Items[index];
                        var rect = item.gameObject.transform as RectTransform;

                        posX = (int)gridSize.x * j;

                        rect.localPosition = new Vector2(posX, posY);
                    }
                    posY += -(int)gridSize.y;
                }
            }
            else
            {
                //水平滚动，从Item从上-->>下
                for (int i = 0; i < cols; i++)
                {
                    // 0，4，8，
                    int tmpIndex = i * rows;
                    //列
                    for (int j = 0; j < rows; j++)
                    {
                        // tempIndex=0==> 0+0,0+1,0+2,0+3
                        // tempIndex=4==> 4+0,4+1,4+2,4+3
                        int index = tmpIndex + j;

                        var item = this.Items[index];
                        var rect = item.gameObject.transform as RectTransform;

                        posY = -(int)gridSize.y * j;

                        rect.localPosition = new Vector2(posX, posY);
                    }
                    posX += (int)gridSize.x;
                }
            }
        }


        public override void ReCalcContentSize()
        {
            Vector2 gridSize = new Vector2(this.cfg.ItemSize.x + this.cfg.ItemSpace.x, this.cfg.ItemSize.y + this.cfg.ItemSpace.y);
            var contentRect = this.listbase.contentRect;
            int oldPosX = (int)contentRect.sizeDelta.x;
            int oldPosY = (int)contentRect.sizeDelta.y;
            var lastSize = contentRect.sizeDelta;
            int posX = (int)(cols * gridSize.x);
            int posY = (int)(rows * gridSize.y);

            if (this.type == ListType.Vertical)
            {
                contentRect.sizeDelta = new Vector2(oldPosX, posY);
            }
            else
            {
                contentRect.sizeDelta = new Vector2(posX, oldPosY);
            }

        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }

    public class VirtualList : ListItemMgrStrategy
    {
        private OnTouchEventHandle onClick;

        private int lastStartIndex = -1;
        private int lastEndIndex = -1;
        private BetterList<int> newIndexs = new BetterList<int>();
        private BetterList<int> hideIndexs = new BetterList<int>();
        private BetterList<ListItem> hideItems = new BetterList<ListItem>();

        public VirtualList(List cfg, ListBase listbase)
            : base(cfg, listbase)
        {
            var scroll = cfg.gameObject.GetComponent<ScrollRect>();
            //添加滚动事件回调  
            scroll.onValueChanged.AddListener(delegate { OnScrollValueChange(); });
        }

        private void OnScrollValueChange()
        {
            this.RepositionIitem();
        }

        public override void ReallocItems(int count, OnTouchEventHandle onClick)
        {
            this.onClick = onClick;
        }

        public override void RepositionIitem()
        {
            var selects = this.listbase.RawSelects();

            float scrollOffset = 0;
            int startRow = 0;
            int endRow = 0;
            int startIndex = 0;
            int endIndex = 0;

            Vector2 gridSize = new Vector2(this.cfg.ItemSize.x + this.cfg.ItemSpace.x, this.cfg.ItemSize.y + this.cfg.ItemSpace.y);
            RectTransform parentRect = this.cfg.gameObject.transform as RectTransform;
            var contentRect = this.listbase.contentRect;

            if (this.type == ListType.Horizontal)
            {
                scrollOffset = -1 * contentRect.anchoredPosition.x;
                startRow = Mathf.FloorToInt((float)scrollOffset / gridSize.x);
                endRow = Mathf.FloorToInt((float)(scrollOffset + parentRect.rect.size.x) / gridSize.x);

                startIndex = Math.Min(this.listbase.Count, startRow * rows);
                endIndex = Math.Min(this.listbase.Count, (endRow + 1) * rows);
                startIndex = startIndex <= 0 ? 0 : startIndex;
                endIndex = endIndex <= 0 ? 0 : endIndex;
            }
            else
            {
                scrollOffset = 1 * contentRect.anchoredPosition.y;
                startRow = Mathf.FloorToInt(scrollOffset / gridSize.y);
                endRow = Mathf.FloorToInt((scrollOffset + parentRect.rect.size.y) / gridSize.y);

                startIndex = Math.Min(this.listbase.Count, startRow * cols);
                endIndex = Math.Min(this.listbase.Count, (endRow + 1) * cols);
                startIndex = startIndex < 0 ? 0 : startIndex;
                endIndex = endIndex < 0 ? 0 : endIndex;
            }

            this.newIndexs.Clear();
            for (int i = startIndex; i < endIndex; i++)
            {
                if (i < this.lastStartIndex || i >= this.lastEndIndex)
                {
                    this.newIndexs.Add(i);
                }
            }
            this.hideIndexs.Clear();
            for (int i = this.lastStartIndex; i < this.lastEndIndex; i++)
            {
                if (i < startIndex || i >= endIndex)
                {
                    this.hideIndexs.Add(i);
                }
            }

            //collect hide items
            for (int i = 0; i < Items.Count; i++)
            {
                if (this.hideIndexs.IndexOf(Items[i].Index) < 0)
                    continue;
                if (this.hideItems.IndexOf(Items[i]) >= 0)
                    continue;
                this.hideItems.Add(Items[i]);
            }

            // new add        
            Vector2 itemPos = new UnityEngine.Vector2(0, 0);
            for (int i = 0, n = this.newIndexs.size; i < n; i++)
            {
                var item = this.createItem();
                item.Index = this.newIndexs[i];
                item.transform.FindChild("txtNum").GetComponent<Text>().text = item.Index.ToString();
                item.gameObject.name = item.Index.ToString();
                RectTransform rect = item.gameObject.transform as UnityEngine.RectTransform;

                if (this.type == ListType.Horizontal)
                {
                    itemPos.x = 1 * ((item.Index / this.rows) * gridSize.y);
                    itemPos.y = -1 * (Mathf.FloorToInt(item.Index % this.rows) * gridSize.x);
                }
                else
                {
                    itemPos.x = 1 * ((item.Index % this.cols) * gridSize.x);
                    itemPos.y = -1 * ((Mathf.FloorToInt(item.Index / this.cols) * gridSize.y));
                }
                rect.anchoredPosition = new Vector2(itemPos.x, itemPos.y);
                item.gameObject.SetActive(true);
                item.Selected = selects.IndexOf(item.Index) >= 0;
                //if (this.onVirtualItemChange != null)
                //    this.onVirtualItemChange(item);
            }

            // hide left items
            for (int i = 0, n = this.hideItems.size; i < n; i++)
            {
                var item = this.hideItems[i];
                item.gameObject.SetActive(false);
            }

            this.lastStartIndex = startIndex;
            this.lastEndIndex = endIndex;
        }

        private ListItem createItem()
        {
            ListItem item = null;
            if (this.hideItems.size > 0)
            {
                item = hideItems[hideItems.size - 1];
                if (item != null)
                {
                    hideItems.Remove(item);
                    return item;
                }
            }
            this.cfg.ItemTemp.SetActive(true);
            var itemobj = GameObject.Instantiate(this.cfg.ItemTemp) as UnityEngine.GameObject;
            itemobj.transform.SetParent(this.listbase.contentRect.gameObject.transform, false);
            itemobj.name = "item";
            RectTransform rect = itemobj.transform as UnityEngine.RectTransform;
            rect.pivot = new Vector2(0, 1);
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            item = rect.gameObject.GetOrAddComponent<ListItem>();
            if (this.cfg.CanSelect)
            {
                item.OnClick = this.onClick;
            }

            this.Items.Add(item);
            this.cfg.ItemTemp.SetActive(false);
            return item;
        }

        public override void ReCalcContentSize()
        {
            Vector2 gridSize = new Vector2(this.cfg.ItemSize.x + this.cfg.ItemSpace.x, this.cfg.ItemSize.y + this.cfg.ItemSpace.y);
            float axisRowSize = 0;
            Vector2 lastSize = this.listbase.contentRect.sizeDelta;
            if (type == ListType.Horizontal)
            {
                axisRowSize = cols * (gridSize.x);
                this.listbase.contentRect.sizeDelta = new Vector2(axisRowSize, this.listbase.contentRect.sizeDelta.y);
            }
            else
            {
                axisRowSize = rows * (gridSize.y);
                this.listbase.contentRect.sizeDelta = new Vector2(this.listbase.contentRect.sizeDelta.x, axisRowSize);
            }

            //if (this.onContentSizeChange != null && ((lastSize.x != contentRect.sizeDelta.x) || (lastSize.y != contentRect.sizeDelta.y)))
            //{
            //    this.onContentSizeChange(contentRect.sizeDelta);
            //}
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }


    public class ListBase
    {
        /// <summary>
        /// 行
        /// </summary>
        protected int rows = 0;
        /// <summary>
        /// 列
        /// </summary>
        protected int cols = 0;
        /// <summary>
        /// 创建item的数量
        /// </summary>
        protected int _count = 0;
        /// <summary>
        /// ListBase中存放item的集合
        /// </summary>
        protected List<ListItem> items = new List<ListItem>();
        /// <summary>
        /// 选择的索引
        /// </summary>
        private List<int> selects = new List<int>();
        public RectTransform contentRect;

        protected List cfg = null;
        protected ListItemMgrStrategy itemMgr = null;
        public ListBase(List cfg)
        {
            this.cfg = cfg;
            this.contentRect = CreateContent();
            this.CreateScrollable();
            this.InitItemMgrStrategy();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitItemMgrStrategy()
        {
            if (this.cfg.IsVirtualItem)
            {
                itemMgr = new VirtualList(this.cfg, this);
            }
            else
            {
                itemMgr = new NormalList(this.cfg, this);
            }
        }

        /// <summary>
        /// 添加scrollRect
        /// </summary>
        private void CreateScrollable()
        {
            if (!this.cfg.CanScroll) return;
            ScrollRect scroll = this.cfg.gameObject.GetOrAddComponent<ScrollRect>();
            scroll.decelerationRate = 0.01f;
            scroll.content = contentRect;
            scroll.horizontal = (this.cfg.type == ListType.Horizontal);
            scroll.vertical = !scroll.horizontal;

            this.cfg.gameObject.AddComponent<Image>();
            Mask mask = this.cfg.gameObject.GetOrAddComponent<Mask>();
            mask.showMaskGraphic = false;
        }

        /// <summary>
        /// 创建content
        /// </summary>
        /// <returns></returns>
        private RectTransform CreateContent()
        {
            if (!this.cfg.CanScroll)
                return this.cfg.gameObject.GetComponent<RectTransform>();
            RectTransform content = this.cfg.transform.FindChild("content") as RectTransform;
            if (content != null)
                return content as RectTransform;
            GameObject contentObj = new GameObject("content");
            RectTransform contentRect = contentObj.AddComponent<RectTransform>();
            contentRect.pivot = new Vector2(0, 1);
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(0, 1);
            Vector2 tmpSizeDelta = (this.cfg.gameObject.transform as RectTransform).sizeDelta;
            contentRect.sizeDelta = tmpSizeDelta;
            contentRect.SetParent(this.cfg.gameObject.transform, false);
            return contentRect;
        }

        /// <summary>
        /// 计算行|列
        /// </summary>
        private void CalcColsAndRows()
        {
            Vector2 gridSize = new Vector2(this.cfg.ItemSize.x + this.cfg.ItemSpace.x, this.cfg.ItemSize.y + this.cfg.ItemSpace.y);
            if (this.cfg.type == ListType.Vertical)
            {
                rows = (int)(contentRect.sizeDelta.x / gridSize.x);
                cols = Mathf.CeilToInt(this.Count / rows);
            }
            else
            {
                cols = (int)(contentRect.sizeDelta.y / gridSize.y);
                rows = Mathf.CeilToInt(this.Count / cols);
            }
        }

        /// <summary>
        /// 设置创建Item数量
        /// </summary>
        public int Count
        {
            get { return this._count; }
            set
            {
                if (this.cfg.ItemTemp == null)
                    Debug.Log("需要设置Item");
                this._count = value;
                ReallocItems(value);

            }
        }

        /// <summary>
        /// 是否可以多选
        /// </summary>
        public bool MultipleChoice
        {
            get { return this.cfg.MultipleChoice; }
            set
            {
                this.Selected = -1;
                this.cfg.MultipleChoice = value;
            }
        }


        public Vector2 Size
        {
            get { return (this.cfg.gameObject.transform as RectTransform).sizeDelta; }
        }

        /// <summary>
        /// 选择一个的Item索引
        /// </summary>
        public int Selected
        {
            get { return this.selects.Count > 0 ? this.selects[0] : -1; }
            set
            {
                bool valib = value >= 0 && value < this._count;
                this.SingleSelect(valib ? value : -1);
            }
        }

        /// <summary>
        /// 选择多个
        /// </summary>
        public List<int> Selecteds
        {
            get { return this.selects; }
            set
            {
                foreach (var item in items)
                {
                    item.Selected = this.selects.IndexOf(item.Index) >= 0;
                }
            }
        }

        public ListItem GetItem(int index)
        {
            return items[index];
        }

        /// <summary>
        /// 滚动到初始位置
        /// </summary>
        public void ScrollTop()
        {
            if (!this.cfg.CanScroll) return;
            var scorll = this.cfg.GetComponent<ScrollRect>();
            if (this.cfg.type == ListType.Horizontal)
            {
                contentRect.anchoredPosition = new Vector2(0, contentRect.anchoredPosition.y);
            }
            else
            {
                contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 0);
            }
        }
        /// <summary>
        /// 滚动到指定位置
        /// </summary>
        /// <param name="axialRow"></param>
        public void ScrollByAxialRow(int axialRow)
        {
            if (!this.cfg.CanScroll) return;

            Vector2 gridSize = new Vector2(this.cfg.ItemSize.x + this.cfg.ItemSpace.x, this.cfg.ItemSize.y + this.cfg.ItemSpace.y);

            float offset = 0;

            var scorll = this.cfg.GetComponent<ScrollRect>();
            if (this.cfg.type == ListType.Horizontal)
            {
                offset = -1 * gridSize.x * axialRow;
                contentRect.anchoredPosition = new Vector2(offset, contentRect.anchoredPosition.y);
            }
            else
            {
                offset = 1 * gridSize.y * axialRow;
                contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, offset);
            }
        }

        public void Refresh()
        {

        }

        private void ReallocItems(int count)
        {
            this.itemMgr.ReallocItems(count, OnClick);
            this.RelayoutItems();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="_listener"></param>
        /// <param name="_args"></param>
        /// <param name="_params"></param>
        private void OnClick(GameObject _listener, object _args, params object[] _params)
        {
            ListItem item = _listener.GetComponent<ListItem>();
            if (item == null) return;

            if (this.MultipleChoice)
            {
                this.MultSelect(item, !item.Selected);
            }
            else
            {
                this.SingleSelect(item.Index);
            }

            if (this.cfg.OnClickItem != null)
            {
                this.cfg.OnClickItem(item.Index);
            }
        }



        public void RelayoutItems()
        {
            this.CalcColsAndRows();
            this.itemMgr.SetColsAndRows(this.rows, this.cols);
            this.itemMgr.ReCalcContentSize();
            this.itemMgr.RepositionIitem();
        }

        /// <summary>
        /// 多选
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isSelected"></param>
        public void MultSelect(ListItem item, bool isSelected)
        {
            item.Selected = isSelected;
            if (item.Selected)
            {
                if (this.selects.IndexOf(item.Index) < 0)
                {
                    this.selects.Add(item.Index);
                }
            }
            else
            {
                int index = this.selects.IndexOf(item.Index);
                if (index >= 0)
                {
                    this.selects.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 单选
        /// </summary>
        /// <param name="index"></param>
        public void SingleSelect(int index)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                var item = items[i];
                item.Selected = (item.Index == index);
            }
        }

        /// <summary>
        /// ListBase中的到items 的方法
        /// </summary>
        /// <returns></returns>
        public List<ListItem> Items()
        {
            return this.items;
        }

        public List<int> RawSelects()
        {
            return this.selects;
        }
    }


    public class List : MonoBehaviour
    {
        public ListBase listbase;

        public Action<int> OnClickItem = null;
        public Action<int> OnValueChange = null;
        public Action<ListItem> OnVirtualItem = null;
        public Action<Vector2> OnContentSizeChange = null;

        public Vector2 ItemSize = Vector2.zero;
        public Vector2 ItemSpace = Vector2.zero;
        /// <summary>
        /// 水平/垂直
        /// </summary>
        public ListType type = ListType.Vertical;
        /// <summary>
        /// 克隆的item
        /// </summary>
        public GameObject ItemTemp = null;
        /// <summary>
        /// 是否可以滚动
        /// </summary>
        public bool CanScroll = false;
        /// <summary>
        /// 是否是动态创建Item
        /// </summary>
        public bool IsVirtualItem = false;
        /// <summary>
        /// 是否可以选择
        /// </summary>
        public bool CanSelect = true;
        /// <summary>
        /// 是否可以多选
        /// </summary>
        public bool MultipleChoice = false;
       

        void Awake()
        {
            listbase = new ListBase(this);
        }

        public int Count
        {
            get { return this.listbase.Count; }
            set { this.listbase.Count = value; }
        }

        public Vector2 Size
        {
            get;
            set;
        }

        public int Selected
        {
            get { return this.listbase.Selected; }
        }

        public List<int> Selecteds
        {
            get { return this.listbase.Selecteds; }
        }

        public void Refresh()
        {

        }

        public ListItem GetItem(int index)
        {
            return this.listbase.GetItem(index);
        }

        /// <summary>
        /// 滚动到初始位置
        /// </summary>
        public void ScrollTop()
        {
            this.listbase.ScrollTop();
        }

        /// <summary>
        /// 滚动到指定的行/列
        /// </summary>
        /// <param name="axialRow">行|列</param>
        public void ScrollByAxialRow(int axialRow)
        {
            this.listbase.ScrollByAxialRow(axialRow);
        }

    }
}


