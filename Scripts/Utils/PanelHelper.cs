using UnityEngine;

public static class PanelHelper
{
    public static T GetFirstComponentInChildren<T>(GameObject panel) where T : Component
    {
        return FindFirstComponentRecursive<T>(panel.transform);
    }

    private static T FindFirstComponentRecursive<T>(Transform parent) where T : Component
    {
        foreach (Transform child in parent)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                return component;
            }

            T foundInChild = FindFirstComponentRecursive<T>(child);
            if (foundInChild != null)
            {
                return foundInChild;
            }
        }
        return null;
    }
}
