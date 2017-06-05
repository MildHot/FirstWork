using UnityEngine;
using System.Collections;
namespace FrameWork
{
    public abstract class Singleton<T> where T : class, new()
    {
        private static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        protected Singleton()
        {
            if(_instance!=null)
                Debug.LogError(" Singleton is not null = "+(typeof(T).ToString()));
            Init();
        }

        public virtual void Init() { }
    }

}
