using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T:MonoBehaviour
{
    protected virtual void Awake()
    {
        T[] managers = FindObjectsOfType<T>();
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public static T Get()
    {
        var tag = typeof(T).Name;
        GameObject managerObject = GameObject.FindWithTag(tag);
        if (managerObject != null)
        {
            return managerObject.GetComponent<T>();
        }
        GameObject go = new(tag);
        go.tag = tag;
        return go.AddComponent<T>();
    }
    
}