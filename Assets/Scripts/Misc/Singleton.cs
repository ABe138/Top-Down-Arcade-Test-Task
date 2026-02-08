using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (Quitting) return null;
            return instance;
        }
    }

    private static bool Quitting = false;

    protected abstract bool Persistent { get; }

    /// <summary>
    /// Make sure to call base.Awake() in override if you need awake.
    /// </summary>
    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    private void InitializeSingleton()
    {
        if (!Application.isPlaying) return;

        transform.SetParent(null);

        if (instance == null)
        {
            instance = this as T;
            if (Persistent) DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        Quitting = true;    //needed to counter Zombie objects issue
    }
}