using UnityEngine;
using UnityEngine.UI;

public class ImageMultiLabelView : MultiLabelView
{
    [SerializeField]
    private Image image;

    public void Initialize(Sprite sprite, params string[] texts)
    {
        Initialize(texts);
        image.sprite = sprite;
    }
}