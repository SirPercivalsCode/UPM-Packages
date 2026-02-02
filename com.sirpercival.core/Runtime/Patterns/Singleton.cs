using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _instanceLock = new object();
    protected static bool _quitting = false;

    public static T Instance
    {
        get
        {
            lock (_instanceLock)
            {
                if (_instance == null && !_quitting) _instance = FindFirstObjectByType<T>();
                return _instance;
            }
        }
    }

    [Header("Singleton")]
    [SerializeField] protected bool dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if (_instance == null) _instance = gameObject.GetComponent<T>();
        else if (_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            throw new System.Exception(string.Format("Instance of {0} already exists, removing {1}", GetType().FullName, ToString()));
        }

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayeModeStateChanged;
#endif
        // _quitting = false;
        if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
    }

#if UNITY_EDITOR
    private static void OnPlayeModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode) _quitting = false;
        if (state == PlayModeStateChange.ExitingEditMode) EditorApplication.playModeStateChanged -= OnPlayeModeStateChanged;
    }
#endif

    protected virtual void OnApplicationQuit()
    {
        _quitting = true;
    }
}