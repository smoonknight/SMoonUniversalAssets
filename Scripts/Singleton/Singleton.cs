using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance => instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            OnNullSetup();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnNullSetup()
    {
        instance = this as T;
    }

    public void Destroy()
    {
        if (instance == null)
        {
            return;
        }
        Destroy(gameObject);
        instance = null;
    }
}