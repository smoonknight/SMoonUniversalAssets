using UnityEngine;

public abstract class ViewBase : MonoBehaviour
{
    public RectTransform rectTransform => transform as RectTransform;
    protected virtual void OnDestroy()
    {
        LeanTween.cancel(rectTransform);
    }
}