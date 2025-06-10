using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class SpriteOrderResetter : EditorWindow
{
    private GameObject rootObject;
    private int modifiedCount = 0;

    [MenuItem("Tools/Reset SpriteRenderer Sorting Orders")]
    public static void ShowWindow()
    {
        GetWindow<SpriteOrderResetter>("Reset Sorting Orders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Reset SpriteRenderer Order In Layer", EditorStyles.boldLabel);
        rootObject = (GameObject)EditorGUILayout.ObjectField("Root GameObject", rootObject, typeof(GameObject), true);

        if (GUILayout.Button("Reset Orders") && rootObject != null)
        {
            ResetSortingOrders();
            Debug.Log($"[SpriteOrderResetter] Modified {modifiedCount} SpriteRenderers.");
        }
    }

    private void ResetSortingOrders()
    {
        SpriteRenderer[] allRenderers = rootObject.GetComponentsInChildren<SpriteRenderer>(true);

        // Group by sortingOrder, lalu urutkan
        var grouped = allRenderers
            .GroupBy(sr => sr.sortingOrder)
            .OrderBy(g => g.Key)
            .ToList();

        int currentOrder = 0;
        modifiedCount = 0;

        foreach (var group in grouped)
        {
            foreach (var sr in group)
            {
                Undo.RecordObject(sr, "Reset Sorting Order");
                sr.sortingOrder = currentOrder;
                modifiedCount++;
            }
            currentOrder++;
        }
    }
}
