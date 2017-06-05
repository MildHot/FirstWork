using UnityEngine;
using System.Collections;
namespace FrameWork
{ 

    public enum UILayer
    {
        Base=0,
        Normal,
        FullSceen,
        Tip,
        Guide,
        MessageBox,
        Effect,
        Top,
        Max,
    }

    /// <summary>
    /// 状态改变委托
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="newState"></param>
    /// <param name="oldState"></param>
    public delegate void StateChangeEvent(object ui, EnumObjectState newState, EnumObjectState oldState);

    public abstract class BaseUI : MonoBehaviour
    {
       
        public event StateChangeEvent StateChanged;

        #region 缓存 gameobject  transform

        private GameObject _cacheGameObject;
        public GameObject CacheGameObject
        {
            get
            {
                if (_cacheGameObject == null)
                {
                    this._cacheGameObject = this.gameObject;
                }
                return _cacheGameObject;
            }
        }

        private Transform _cacheTransform;
        public Transform CacheTransform
        {
            get
            {
                if (_cacheTransform == null)
                {
                    this._cacheTransform = this.transform;
                }
                return _cacheTransform;
            }
        }

        #endregion

        #region 状态  UI Type

        protected EnumObjectState _state = EnumObjectState.None;
        public EnumObjectState State
        {
            get { return _state; }
            set
            {
                if (_state != value) {
                    EnumObjectState oldState = this._state;
                    if (StateChanged != null) {
                        StateChanged(this, this._state, oldState);
                    }
                }
            }
        }

        public abstract EnumUIType GetUIType();
        public abstract UILayer GetUILayer();

        #endregion

        #region 执行流程

        void Awake()
        {
            this.State = EnumObjectState.Initial;
            OnAwake();
        }

        void Start()
        {
            OnStart();
        }

        void Update()
        {
            if (this.State == EnumObjectState.Ready)
                OnUpdate(Time.deltaTime);
        }

       public void Release()
        {
            this.State = EnumObjectState.Closing;
            GameObject.Destroy(this.CacheGameObject);
            OnRelease();
        }

        void OnDestroy()
        {
            this.State = EnumObjectState.None;
        }

        protected virtual void OnAwake()
        {
            this.State = EnumObjectState.Loading;
            this.OnPlayOpenUIAudio();
        }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnRelease()
        {
            this.State = EnumObjectState.None;
            this.OnPlayCloseUIAudio();
        }

        protected virtual void OnPlayOpenUIAudio()
        {

        }

        protected virtual void OnPlayCloseUIAudio()
        {

        }

        #endregion

        protected virtual void Set(params object[] uiParams)
        {
            this.State = EnumObjectState.Loading;
        }

        public virtual void SetUIparam(params object[] uiParams)
        {

        }

        protected virtual void OnLoadData()
        {

        }

        public void SetUIWhenOpening(params object[] uiParams)
        {
            SetUIparam(uiParams);
            CoroutineController.Instance.StartCoroutine(AsyncOnLoadData());
        }

        private IEnumerator AsyncOnLoadData()
        {
            yield return new WaitForSeconds(0);
            if (this.State == EnumObjectState.Loading)
            {
                this.OnLoadData();
                this.State = EnumObjectState.Ready;
            }
        }
    }
}

