using System.Collections.Generic;
using UnityEngine;

public class BootManager : SingletonWithDontDestroyOnLoad<BootManager>
{
    [SerializeField]
    private List<SingletonBootProperty> singletonBootProperties;

    public Singleton<T> Recall<T>(SingletonBootType singletonBootType, Singleton<T> overrideInstance) where T : MonoBehaviour
    {
        var singletonBootProperty = singletonBootProperties.Find(find => find.singletonBootType == singletonBootType);
        if (singletonBootProperty.monoBehaviour == null)
        {
            overrideInstance = null;
        }
        return ReturnRefreshedSingletonValidation(singletonBootProperty.monoBehaviour as Singleton<T>, overrideInstance);
    }

    Singleton<T> ReturnRefreshedSingletonValidation<T>(Singleton<T> monoBehaviour, Singleton<T> overrideInstance) where T : MonoBehaviour
    {
        overrideInstance?.Destroy();
        return Instantiate(monoBehaviour);
    }
}

[System.Serializable]
public struct SingletonBootProperty
{
    public MonoBehaviour monoBehaviour;
    public SingletonBootType singletonBootType;
}

public enum SingletonBootType
{
    GameManager
}