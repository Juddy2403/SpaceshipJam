using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance || _applicationQuitting) return _instance;
            _instance = FindFirstObjectByType<T>();
            if (_instance) return _instance;
            GameObject newInstance = new GameObject($"Singleton_{typeof(T).Name}");
            _instance = newInstance.AddComponent<T>();
            return _instance;
        }
    }

    public static bool Exists => _instance != null;

    private static bool _applicationQuitting = false;
    protected virtual void OnApplicationQuit()
    {
        _applicationQuitting = true;
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        _instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this as T)
        {
            _instance = null;
        }
    }
}
