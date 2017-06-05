using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace FrameWork
{
    public class UIManager : Singleton<UIManager>
    {
        class UIInfoData
        {
            public EnumUIType UIType { get; private set; }
            public string Path { get; private set; }
            public object[] UIparams { get; private set; }

            public Type ScriptType { get; private set; }
            public UIInfoData(EnumUIType _uiType, string _path, params object[] _uiParams)
            {
                UIType = _uiType;
                Path = _path;
                UIparams = _uiParams;
                ScriptType = UIPathDefines.GetUIScriptByType(UIType);
            }
        }

        /// <summary>
        /// 当前打开的UI
        /// </summary>
        private Dictionary<EnumUIType, GameObject> dicOpenUIs = null;
        /// <summary>
        /// 需要打开的UI
        /// </summary>
        private Stack<UIInfoData> stackOpenUIs = null;
        private Transform uiContainer;
        public override void Init()
        {
            dicOpenUIs = new Dictionary<EnumUIType, GameObject>();
            stackOpenUIs = new Stack<UIInfoData>();

            if (null == uiContainer)
            {
                UnityEngine.Object _prefabObj = Resources.Load("Prefabs/RootUI");
                GameObject _uiObject = MonoBehaviour.Instantiate(_prefabObj) as GameObject;
                _uiObject.name = "RootUI";
                uiContainer = _uiObject.transform.FindChild("Container");

            }
            CreateUILayers();
        }



        #region 创建UI层

        private void CreateUILayers()
        {
            for (int i = 0; i < (int)UILayer.Max; i++)
            {
                GameObject go = new GameObject();
                go.transform.parent = uiContainer;
                go.name = ((UILayer)i).ToString();
                RectTransform rect= go.GetOrAddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 1);
                rect.offsetMin = new Vector2(0, 0);
                rect.offsetMax = new Vector2(0, 0);
                rect.pivot = new Vector2(0.5f, 0.5f);
                rect.localScale = new Vector3(1, 1, 1);
            }
        }

        #endregion


        #region 得到指定UI

        /// <summary>
        /// 获取指定ui
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_type"></param>
        /// <returns></returns>
        public T GetUI<T>(EnumUIType _type) where T : BaseUI
        {
            GameObject _retObj = GetUIObject(_type);

            if (_retObj != null)
            {
                return _retObj.GetComponent<T>();
            }
            return null;
        }

        public GameObject GetUIObject(EnumUIType _uiType)
        {
            GameObject _retObj = null;
            if (!dicOpenUIs.TryGetValue(_uiType, out _retObj))
            {
                Debug.LogError(" dic is not have gameobject  =" + _uiType);
            }
            return _retObj;
        }

        #endregion

        #region 预加载

        public void PreLoadUI(EnumUIType[] _uiTypes)
        {
            for (int i = 0; i < _uiTypes.Length; i++)
            {
                PreLoadUI(_uiTypes[i]);
            }
        }

        public void PreLoadUI(EnumUIType _uiType)
        {
            string path = UIPathDefines.GetPrefabsPathByType(_uiType);
            Resources.Load(path);
        }

        #endregion

        #region 打开UI


        public void OpenUI(EnumUIType[] _uiTypes)
        {
            OpenUI(false, _uiTypes, null);
        }

        public void OpenUI(EnumUIType _uiType, params object[] _uiParams)
        {
            EnumUIType[] _uiTypes = new EnumUIType[1];
            _uiTypes[0] = _uiType;
            OpenUI(false, _uiTypes, _uiParams);
        }

        public void OpenUICloseOthers(EnumUIType[] _uiTypes)
        {
            OpenUI(true, _uiTypes, null);
        }

        public void OpenUICloseOthers(EnumUIType _uiType, params object[] _uiParams)
        {
            EnumUIType[] _uiTypes = new EnumUIType[1];
            _uiTypes[0] = _uiType;
            OpenUI(true, _uiTypes, _uiParams);
        }

        public void OpenUI(bool _isCloseOthers, EnumUIType[] _uiTypes, params object[] _uiParams)
        {
            //关闭其他ui
            if (_isCloseOthers)
            {
                CloseUIAll();
            }
            //要打开的ui
            for (int i = 0; i < _uiTypes.Length; i++)
            {
                EnumUIType _uiType = _uiTypes[i];
                if (!dicOpenUIs.ContainsKey(_uiType))
                {
                    string _path = UIPathDefines.GetPrefabsPathByType(_uiType);
                    stackOpenUIs.Push(new UIInfoData(_uiType, _path, _uiParams));
                }
            }
            //打开ui
            if (stackOpenUIs.Count > 0)
            {
                CoroutineController.Instance.StartCoroutine(AsyncLoadData());
            }
        }

        private IEnumerator<int> AsyncLoadData()
        {
          
            UIInfoData _uiInfoData = null;
            UnityEngine.Object _uiprefab = null;
            GameObject _uiObject = null;
            if (stackOpenUIs != null && stackOpenUIs.Count > 0)
            {
                do
                {
                    _uiInfoData = stackOpenUIs.Pop();
                    _uiprefab = Resources.Load(_uiInfoData.Path);
                    if (_uiprefab != null)
                    {
                        _uiObject = MonoBehaviour.Instantiate(_uiprefab) as GameObject;

                        if (_uiObject == null)
                        {
                            yield break;
                        }
                        RectTransform rt = _uiObject.GetComponent<RectTransform>();
                        BaseUI _baseui = _uiObject.GetComponent<BaseUI>();
                        if (_baseui != null)
                        {
                            _baseui.SetUIWhenOpening(_uiInfoData.UIparams);
                        }
                        else
                        {
                            _baseui = _uiObject.AddComponent(_uiInfoData.ScriptType) as BaseUI;
                        }
                        Transform layer = uiContainer.FindChild(_baseui.GetUILayer().ToString());
                        rt.SetParent(layer, false);
                        dicOpenUIs.Add(_uiInfoData.UIType, _uiObject);
                    }
                    else
                    {
                        Debug.Log(" 当前路径不存在  " + _uiInfoData.Path);
                    }
                } while (stackOpenUIs.Count > 0);
            }

            yield return 0;
        }



        #endregion

        #region 关闭UI

        public void CloseUIAll()
        {
            List<EnumUIType> _listKey = new List<EnumUIType>(dicOpenUIs.Keys);
            for (int i = 0; i < _listKey.Count; i++)
            {
                CloseUI(_listKey[i]);
            }
            // CloseUI(_listKey.ToArray());
            dicOpenUIs.Clear();
        }

        public void CloseUI(EnumUIType[] _uiTypes)
        {
            for (int i = 0; i < _uiTypes.Length; i++)
            {
                CloseUI(_uiTypes[i]);
            }
        }
        public void CloseUI(EnumUIType _uiType)
        {
            GameObject _uiObj = null;
            if (!dicOpenUIs.TryGetValue(_uiType, out _uiObj))
            {
                Debug.LogWarning(" CloseUI dicOpenUIs.TryGetValue   Failure  = " + _uiType.ToString());
                return;
            }
            CloseUI(_uiType, _uiObj);
        }

        public void CloseUI(EnumUIType _uiType, GameObject _uiObj)
        {
            if (_uiObj == null)
            {
                dicOpenUIs.Remove(_uiType);
            }
            else
            {
                BaseUI _baseUI = _uiObj.GetComponent<BaseUI>();
                if (_baseUI == null)
                {
                    GameObject.Destroy(_uiObj);
                    dicOpenUIs.Remove(_uiType);
                }
                else
                {
                    _baseUI.StateChanged += CloseUIHandle;
                    _baseUI.Release();
                }
            }
        }
        /// <summary>
        /// 关闭UI操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="newState"></param>
        /// <param name="oldState"></param>
        public void CloseUIHandle(object sender, EnumObjectState newState, EnumObjectState oldState)
        {
            if (newState == EnumObjectState.Closing)
            {
                BaseUI _baseUI = sender as BaseUI;
                dicOpenUIs.Remove(_baseUI.GetUIType());
                _baseUI.StateChanged -= CloseUIHandle;
            }
        }



        #endregion
    }
}

