using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual bool _destroyOnLoad => false;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
            if (!_destroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}