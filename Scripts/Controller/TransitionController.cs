using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public List<TransitionAnimationType> transitionAnimationTypes;

    public void Change(float percentage)
    {
        foreach (var transitionAnimationType in transitionAnimationTypes)
        {
            switch (transitionAnimationType)
            {

                case TransitionAnimationType.AlphaCanvasGroup: canvasGroup.alpha = percentage; break;
                case TransitionAnimationType.Scale: transform.localScale = Vector3.one * percentage; break;
            }
        }
    }
}

public enum TransitionAnimationType
{
    AlphaCanvasGroup,
    Scale
}