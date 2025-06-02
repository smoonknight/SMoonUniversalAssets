using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class TargetLayoutFitter : MonoBehaviour
{
    [SerializeField]
    private RectTransform targetTransform;
    [SerializeField]
    private bool useWidth = true;
    [SerializeField]
    private bool useHeight = true;
    [SerializeField]
    private bool usePadding;
    [SerializeField]
    private Padding padding;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    private void OnEnable()
    {
        UpdateSize();
    }

    private void Update()
    {
        UpdateSize();
    }

    private void UpdateSize()
    {
        if (targetTransform == null) return;

        Vector2 targetSize = targetTransform.sizeDelta;

        if (usePadding)
        {
            targetSize.x += padding.left + padding.right;
            targetSize.y += padding.top + padding.bottom;
        }

        Vector2 newSize = rectTransform.sizeDelta;

        if (useWidth) newSize.x = targetSize.x;
        if (useHeight) newSize.y = targetSize.y;

        rectTransform.sizeDelta = newSize;
    }
}

