using UnityEngine;
using System.Collections;
/// <summary>
/// 不销毁单例
/// </summary>
public abstract class DDOLSingleton<T> : MonoBehaviour where T : DDOLSingleton<T>
{
    protected static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("DDOLGameObject");
                if (go == null)
                {
                    go = new GameObject("DDOLGameObject");
                    DontDestroyOnLoad(go);
                }
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }


    private void OnApplicationQuit()
    {
        _instance = null;
    }
}
