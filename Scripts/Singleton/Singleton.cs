using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance => instance;

    protected void Awake()
    {
        if (instance == null)
        {
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnAwake()
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