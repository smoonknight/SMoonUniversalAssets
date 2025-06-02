using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class AutoSizeLayout : MonoBehaviour
{
    public enum States
    {
        VerticalTop,
        VerticalCenter,
        VerticalBottom,
        HorizontalLeft,
        HorizontalCenter,
        HorizontalRight,
        Custom
    }


    public Vector2 customAnchorMin;
    public Vector2 customAnchorMax;
    public Vector2 customPivot;

    public bool isLoopUpdate;
    public bool dontTouchChildren;
    public States typeLayout;
    public bool isResizeSelf = true;
    public bool isUseAdditionalPadding = false;
    public Padding padding;
    public float addHeight;
    public float addWidth;
    public float spacing;
    public int repeatFrames = 2;
    public bool isHaveMinSizeX, isHaveMinSizeY, isHaveMaxSizeX, isHaveMaxSizeY;
    public Vector2 minSize, maxSize;
    public RectTransform minSizeTargetRect;
    public bool isInverted;

    private CancellationTokenSource cancellationTokenSource;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (isLoopUpdate)
            UpdateLayout(false);
    }

    private void OnDisable()
    {
        cancellationTokenSource?.Cancel();
    }

    public void UpdateLayout(bool isRepeat = true, bool isRecursive = false)
    {
        UpdateAllRect(isRecursive);

        if (isRepeat && gameObject.activeInHierarchy)
        {
            UpdateRepeate(isRecursive);
        }
    }

    private void UpdateAllRect(bool isRecursive)
    {
        if (minSizeTargetRect != null)
            minSize = minSizeTargetRect.rect.size;

        float sizeTotal = CalculateLayout(isRecursive);

        if (isResizeSelf)
        {
            rectTransform.sizeDelta = ApplySizeConstraints(rectTransform.sizeDelta, sizeTotal);
        }
    }

    private float CalculateLayout(bool isRecursive)
    {
        float sizeTotal = typeLayout switch
        {
            States.VerticalTop => padding.top,
            States.VerticalBottom => padding.bottom,
            States.HorizontalLeft => padding.left,
            States.HorizontalRight => padding.right,
            _ => 0
        };

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!ShouldIncludeChild(child)) continue;

            RectTransform rect = child.GetComponent<RectTransform>();
            if (isRecursive && rect.TryGetComponent(out AutoSizeLayout childLayout))
                childLayout.UpdateLayout(isRecursive: true);

            if (!dontTouchChildren)
                AdjustChildPosition(rect, ref sizeTotal);
        }

        return FinalizeSize(sizeTotal);
    }

    private bool ShouldIncludeChild(Transform child)
    {
        bool tagCondition = isInverted ? child.CompareTag("NotInLayout") : !child.CompareTag("NotInLayout");
        return child.gameObject.activeSelf && tagCondition;
    }

    private void AdjustChildPosition(RectTransform rect, ref float sizeTotal)
    {
        Vector2 offset = typeLayout switch
        {
            States.VerticalTop => new Vector2(0, -sizeTotal),
            States.VerticalBottom => new Vector2(0, sizeTotal),
            States.HorizontalLeft => new Vector2(sizeTotal, 0),
            States.HorizontalRight => new Vector2(-sizeTotal, 0),
            _ => rect.anchoredPosition
        };

        if (typeLayout is States.VerticalTop or States.VerticalBottom)
        {
            rect.anchorMax = new Vector2(0.5f, typeLayout == States.VerticalTop ? 1 : 0);
            rect.anchorMin = rect.anchorMax;
            rect.pivot = new Vector2(rect.pivot.x, typeLayout == States.VerticalTop ? 1 : 0);
        }
        else if (typeLayout is States.HorizontalLeft or States.HorizontalRight)
        {
            rect.anchorMax = new Vector2(typeLayout == States.HorizontalLeft ? 0 : 1, 0.5f);
            rect.anchorMin = rect.anchorMax;
            rect.pivot = new Vector2(typeLayout == States.HorizontalLeft ? 0 : 1, rect.pivot.y);
        }
        else if (typeLayout is States.Custom)
        {
            rect.anchorMax = customAnchorMax;
            rect.anchorMin = customAnchorMin;
            rect.pivot = customPivot;
        }

        rect.anchoredPosition = offset + (isUseAdditionalPadding
            ? new Vector2(padding.left - padding.right, padding.bottom - padding.top)
            : Vector2.zero);

        sizeTotal += typeLayout is States.VerticalTop or States.VerticalBottom
            ? rect.sizeDelta.y * rect.localScale.y + spacing
            : rect.sizeDelta.x * rect.localScale.x + spacing;
    }

    private float FinalizeSize(float sizeTotal)
    {
        sizeTotal -= spacing;
        return typeLayout switch
        {
            States.VerticalTop => sizeTotal + padding.bottom,
            States.VerticalBottom => sizeTotal + padding.top,
            States.HorizontalLeft => sizeTotal + padding.right,
            States.HorizontalRight => sizeTotal + padding.left,
            _ => sizeTotal
        };
    }

    private Vector2 ApplySizeConstraints(Vector2 currentSize, float sizeTotal)
    {
        return typeLayout switch
        {
            States.VerticalTop or States.VerticalBottom or States.VerticalCenter or States.Custom => new Vector2(
                currentSize.x,
                Mathf.Clamp(sizeTotal + addHeight, isHaveMinSizeY ? minSize.y + addHeight : float.MinValue, isHaveMaxSizeY ? maxSize.y : float.MaxValue)
            ),
            _ => new Vector2(
                Mathf.Clamp(sizeTotal, isHaveMinSizeX ? minSize.x : float.MinValue, isHaveMaxSizeX ? maxSize.x : float.MaxValue),
                currentSize.y
            )
        };
    }

    private async void UpdateRepeate(bool isRecursive)
    {
        for (int i = 0; i < repeatFrames; i++)
        {
            UpdateAllRect(isRecursive);
            await UniTask.Yield();
        }
    }
}
