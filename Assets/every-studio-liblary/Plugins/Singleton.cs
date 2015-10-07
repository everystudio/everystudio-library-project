using UnityEngine;
 

//Unity用Singletonクラス 参考URL http://caitsithware.com/wordpress/archives/118から少し修正使用
public abstract class Singleton<T>  : MonoBehaviour where T : Singleton<T> 
{
    static T m_Instance = null;
    static bool isShuttingDown = false;
 
    public static T instance {
        get {
            if (isShuttingDown) {
                return null;
            }

            if (m_Instance != null) {
                return m_Instance;
            }
 
            System.Type type = typeof(T);
 
            T instance = GameObject.FindObjectOfType(type) as T;
 
            if (instance == null) {
                string typeName = type.ToString();
 
                GameObject gameObject = new GameObject(typeName, type);
                instance = gameObject.GetComponent<T>();
 
                if (instance == null) {
                    Debug.LogError("Problem during the creation of " + typeName, gameObject);
                } else {
                    Initialize(instance);
                }
            } else {
                Initialize(instance);
            }
            return m_Instance;
        }
    }
 
    static void Initialize(T instance)
    {
        if (m_Instance == null) {
            m_Instance = instance;
 
            m_Instance.OnInitialize();
        } else if (m_Instance != instance) {
            DestroyImmediate(instance.gameObject);
        }
    }
 
    static void Destroyed(T instance)
    {
        if (m_Instance == instance) {
            m_Instance.OnFinalize();
 
            m_Instance = null;
        }
    }
 
    public virtual void OnInitialize()
    {
    }
    public virtual void OnFinalize()
    {
    }
 
    public virtual void Awake()
    {
        Initialize(this as T);
    }
 
    void OnDestroy()
    {
        Destroyed(this as T);
    }
 
    void OnApplicationQuit()
    {
        isShuttingDown = true;
        Destroyed(this as T);
    }
}