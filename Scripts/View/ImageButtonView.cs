using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageButtonView : ButtonView
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private CanvasGroup canvasGroup;

    public CanvasGroup GetCanvasGroup() => canvasGroup;

    public void Initialize(Sprite sprite, string text, string secondaryText, UnityAction buttonAction)
    {
        Initialize(text, secondaryText, buttonAction);
        image.sprite = sprite;
    }
}