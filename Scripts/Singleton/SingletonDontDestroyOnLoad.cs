using UnityEngine;

public class SingletonWithDontDestroyOnLoad<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(gameObject);
    }
}