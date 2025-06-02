using System;
using UnityEngine;

[System.Serializable]
public class GameObjectsModifier
{
    [SerializeField]
    private GameObject[] enableGameObjects;
    [SerializeField]
    private GameObject[] disableGameObjects;
    [SerializeField]
    private bool disableOnAwake = true;


    public void DisableGameObjects()
    {
        SetConditionGameObjects(false, disableGameObjects);
    }

    public void SetIfDisableOnAwake()
    {
        if (disableOnAwake)
        {
            SetConditionGameObjects(false, enableGameObjects);
        }
    }

    public void EnableGameObjects()
    {
        SetConditionGameObjects(true, enableGameObjects);
    }

    void SetConditionGameObjects(bool condition, GameObject[] gameObjects)
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(condition);
        }
    }
}