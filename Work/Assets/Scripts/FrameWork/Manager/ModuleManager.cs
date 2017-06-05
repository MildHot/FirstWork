using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace FrameWork
{
    public class ModuleManager : Singleton<ModuleManager>
    {
        private Dictionary<string, BaseModule> dicModules = null;

        public override void Init()
        {
            dicModules = new Dictionary<string, BaseModule>();
        }

        #region 注册 --添加要注册的模块

        public void RegisterLoadingModule()
        {
            LoadModule(typeof(LoadingModule));
        }


        public void RegisterAllModule()
        {
           // LoadModule(typeof(LoaginModule));
        }


        private void LoadModule(Type moduleType)
        {
            BaseModule bm = System.Activator.CreateInstance(moduleType) as BaseModule;
            bm.Load();
        }

        #endregion


        #region 得到模块
        public BaseModule Get(string key)
        {
            if (dicModules.ContainsKey(key))
            {
                return dicModules[key];
            }
            return null;
        }

        public T Get<T>() where T : BaseModule
        {
            Type t = typeof(T);
            if (dicModules.ContainsKey(t.ToString()))
            {
                return dicModules[t.ToString()] as T;
            }
            return null;
        }

        #endregion

        #region 注册模块
        public void Register(BaseModule module)
        {
            Type t = module.GetType();
            Register(t.ToString(), module);
        }

        public void Register(string key, BaseModule module)
        {
            if (!dicModules.ContainsKey(key))
            {
                dicModules.Add(key, module);
            }
        }

        #endregion 

        #region 注销模块
        public void UnRegister(BaseModule module)
        {
            Type t = module.GetType();
            UnRegister(t.ToString());
        }

        public void UnRegister(string key)
        {
            if (dicModules.ContainsKey(key))
            {
                BaseModule module = dicModules[key];
                module.Release();
                dicModules.Remove(key);
                module = null;
            }
        }


        public void UnRegisterAll()
        {
            List<string> _keyList = new List<string>(dicModules.Keys);
            for (int i = 0; i < _keyList .Count ; i++)
            {
                UnRegister(_keyList[i]);
            }
            dicModules.Clear();
        }

        #endregion
    }
}