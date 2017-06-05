//==================================================================================
/// ElementsMapper
/// @bill
//==================================================================================

using UnityEngine;
using System.Collections.Generic;
using System;
public class ElementsMapper : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectPair
    {
        public string Name;
        public GameObject GameObject;
    }
    [SerializeField]
    public List<GameObject> Panels = new List<GameObject>(0);
    public int panelCount
    {
        get
        {
            return Panels.Count;
        }
    }
    [SerializeField]
    public List<ObjectPair> Elements = new List<ObjectPair>(0);

    private bool inited = false;
    private Dictionary<string, GameObject> _elements = new Dictionary<string, GameObject>();

    public GameObject GetPanel(int index)
    {
        if (index >= Panels.Count)
        {
            return null;
        }
        return Panels[index];
    }

    public GameObject GetElement(string name)
    {
        if (!inited)
            Init();

        GameObject obj = null;
        if (!_elements.TryGetValue(name, out obj))
        {
            return null;
        }
        return obj;
    }

    public T GetElement<T>(string name) where T : UnityEngine.Component
    {
        GameObject obj = GetElement(name);
        if (obj == null)
            return null;

        T component = obj.GetComponent<T>();
        return component;
    }

    public Component GetElement(Type type, string name)
    {
        GameObject obj = GetElement(name);
        if (obj == null)
            return null;

        Component component = obj.GetComponent(type);
        return component;
    }

    private void Init()
    {
        for (int i = 0; i < Elements.Count; i++)
        {
            ObjectPair p = Elements[i];
            if (_elements.ContainsKey(p.Name))
                Debug.LogError("In prefab [" + gameObject.name + "] elements mapper has duplicate name [" + p.Name + "]");

            _elements[p.Name] = p.GameObject;
        }
#if !UNITY_EDITOR
            Elements.Clear();
#endif
        inited = true;
    }
}