using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace FrameWork
{

    public class AssetInfo
    {
        private UnityEngine.Object _object;
        public Type AssetType { get; set; }
        public string Path { get; set; }
        public int RefCount { get; set; }
        public bool IsLoaded { get { return _object != null; } }

        public UnityEngine.Object AssetObject
        {
            get
            {
                if (_object == null)
                    _ResourceLoad();
                return _object;
            }
        }

        public IEnumerator GetCoroutineObject(Action<UnityEngine.Object> _loaded)
        {
            while (true)
            {
                if (_object == null)
                {
                    //相当于暂停一帧
                    yield return null;
                    _ResourceLoad();
                    yield return null;
                }

                if (_loaded != null)
                {
                    _loaded(_object);
                }

                yield break;
            }
        }

        private void _ResourceLoad()
        {
            try
            {
                _object = Resources.Load(Path);
                if (_object == null)
                {
                    Debug.Log("  _ResourceLoad load failure path " + Path);
                }
            }
            catch (Exception e)
            {
                Debug.Log("  error  e :  " + e.ToString());
            }
        }

        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded)
        {
            return GetAsyncObject(_loaded, null);
        }
        public IEnumerator GetAsyncObject(Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            if (_object != null)
            {
                _loaded(_object);
                yield break;
            }
            ResourceRequest _resRequest = Resources.LoadAsync(Path);
            while (_resRequest.progress < 0.9)
            {
                if (_progress != null)
                {
                    _progress(_resRequest.progress);
                }
                yield return null;
            }

            while (!_resRequest.isDone)
            {
                if (_progress != null)
                {
                    _progress(_resRequest.progress);
                }
                yield return null;
            }
            //???
            _object = _resRequest.asset;
            if (_loaded != null)
            {
                _loaded(_object);
            }
            yield return _resRequest;
        }

    }

    public class ResManager : Singleton<ResManager>
    {
        private Dictionary<string, AssetInfo> dicAssetInfo = null;

        public override void Init()
        {
            dicAssetInfo = new Dictionary<string, AssetInfo>();
        }

        #region 实例化
        //同步实例
        public UnityEngine.Object LoadInstance(string _path)
        {
            UnityEngine.Object _obj = Load(_path);
            return Instantiate(_obj);
        }

        //协程实例
        public void LoadCorotineInstance(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadCoroutine(_path, (_obj) =>
            {
                Instantiate(_obj, _loaded);
            });
        }

        //异步实例
        public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadAsync(_path, (_obj) =>
            {
                Instantiate(_obj, _loaded);
            });
        }
        public void LoadAsyncInstance(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            LoadAsync(_path, (_obj) =>
            {
                Instantiate(_obj, _loaded);
            }, _progress);
        }

        private UnityEngine.Object Instantiate(UnityEngine.Object _obj)
        {
            return Instantiate(_obj,null);
        }

        private UnityEngine.Object Instantiate(UnityEngine.Object _obj, Action<UnityEngine.Object> _loaded)
        {
            UnityEngine.Object _retObj = null;
            if (_obj != null)
            {
                _retObj = MonoBehaviour.Instantiate(_obj);
                if (_retObj != null)
                {
                    if (_loaded != null)
                    {
                        _loaded(_retObj);
                    }
                    return _retObj;
                }
                else
                {
                    Debug.LogError(" 实例化 失败 ");
                }
            }
            else
            {
                Debug.LogError("传入参数为null");
            }
            return null;
        }

        #endregion

        #region 同步加载
        public UnityEngine.Object Load(string _path)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path);
            if (_assetInfo != null)
                return _assetInfo.AssetObject;
            return null;
        }

        #endregion

        #region 协程加载
        public void LoadCoroutine(string _path, Action<UnityEngine.Object> _loaded)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path, _loaded);
            if (_assetInfo != null)
            {
                CoroutineController.Instance.StartCoroutine(_assetInfo.GetCoroutineObject(_loaded));
            }
        }

        #endregion

        #region 异步加载

        public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded)
        {
            LoadAsync(_path, _loaded, null);
        }
        public void LoadAsync(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            AssetInfo _assetInfo = GetAssetInfo(_path, _loaded);
            if (_assetInfo != null)
            {
                CoroutineController.Instance.StartCoroutine(_assetInfo.GetAsyncObject(_loaded, _progress));
            }
        }

        #endregion

        public AssetInfo GetAssetInfo(string _path)
        {
            return GetAssetInfo(_path, null, null);
        }

        public AssetInfo GetAssetInfo(string _path, Action<UnityEngine.Object> _loaded)
        {
            return GetAssetInfo(_path, _loaded, null);
        }
        private AssetInfo GetAssetInfo(string _path, Action<UnityEngine.Object> _loaded, Action<float> _progress)
        {
            if (string.IsNullOrEmpty(_path))
            {
                Debug.LogError("  path is null");
                if (_loaded != null) _loaded(null);
            }

            AssetInfo _assetInfo = null;
            if (!dicAssetInfo.TryGetValue(_path, out _assetInfo))
            {
                _assetInfo = new AssetInfo();
                _assetInfo.Path = _path;
                dicAssetInfo.Add(_path, _assetInfo);
            }
            _assetInfo.RefCount++;
            return _assetInfo;
        }





    }
}

