using UnityEngine;

public class SingletonWithDontDestroyOnLoad<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void OnNullSetup()
    {
        base.OnNullSetup();
        DontDestroyOnLoad(gameObject);
    }
}