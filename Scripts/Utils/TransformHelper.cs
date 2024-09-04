using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    public static void DestroyChilds(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void DestroyChildsImmediately(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.DestroyImmediate(child.gameObject, true);
        }
    }

    // IEnumerator UpdateLayout(RectTransform rectTransform)
    // {
    //     int updateCount = 0;

    //     while (updateCount < 10)
    //     {
    //         yield return null;
    //         rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + 0.1f, rectTransform.sizeDelta.y);
    //         rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - 0.1f, rectTransform.sizeDelta.y);
    //         rectTransform.ForceUpdateRectTransforms();
    //         updateCount += 1;
    //     }
    // }

    public static List<T> GetComponentsRecursively<T>(List<Collider> colliders) where T : Component
    {
        List<T> componentsList = new List<T>();

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<T>(out var currentComponent))
            {
                componentsList.Add(currentComponent);
            }
        }

        return componentsList;
    }

    public static List<T> GetComponentsRecursively<T>(Transform currentTransform) where T : Component
    {
        List<T> componentsList = new List<T>();

        void GetComponents(Transform transform)
        {
            if (transform.TryGetComponent<T>(out var currentComponent))
            {
                componentsList.Add(currentComponent);
            }

            foreach (Transform child in transform)
            {
                GetComponents(child);
            }
        }

        GetComponents(currentTransform);

        return componentsList;
    }

    public static bool CompareLayermaskWithLayerIndex(LayerMask layerMask, int index) => layerMask == (layerMask | (1 << index));
}