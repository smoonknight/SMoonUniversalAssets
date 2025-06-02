using System;
using SMoonUniversalAsset;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonView : TextViewBase, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private TextMeshProUGUI secondaryLabelText;

    private Func<string> secondaryTextFunc;
    private UnityAction<Vector2, bool> onHoverAction;

    public string Text => labelText.text;

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(Action);
    }
    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(Action);
    }

    void Action()
    {
        action?.Invoke();
        SetSecondaryText(secondaryTextFunc?.Invoke());
    }

    public void ChangeInteractable(bool condition) => button.interactable = condition;

    UnityAction action;
    public void Initialize(string text, UnityAction buttonAction)
    {
        ChangeText(text);
        Initialize(buttonAction);
    }

    public void Initialize(UnityAction buttonAction)
    {
        action = buttonAction;
    }

    public void Clear()
    {
        action = null;
        labelText.text = "";
    }

    public void ChangeButtonColor(Color color)
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = color;
        colorBlock.pressedColor = color;
        colorBlock.highlightedColor = color;
        colorBlock.selectedColor = color;
        button.colors = colorBlock;
    }

    public void ChangeButtonOpacity(float percentage)
    {
        button.image.SetOpacity(percentage);
        Color labelTextColor = labelText.color;
        labelTextColor.a = percentage;
        labelText.color = labelTextColor;
    }

    public bool TryInitializeTranslator(StringId stringId, UnityAction buttonAction)
    {
        Initialize(buttonAction);
        return TryInitializeTranslator(stringId);
    }

    public void SetSecondaryFunc(Func<string> secondaryTextFunc, bool callImmidietly = true)
    {
        this.secondaryTextFunc = secondaryTextFunc;
        if (callImmidietly)
            SetSecondaryText(secondaryTextFunc?.Invoke());
    }

    public void SetSecondaryText(string text)
    {
        if (secondaryLabelText == null)
            return;

        secondaryLabelText.text = text;
    }

    public void SetHoverAction(UnityAction<Vector2, bool> onHoverAction)
    {
        this.onHoverAction = onHoverAction;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onHoverAction?.Invoke(eventData.position, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onHoverAction?.Invoke(eventData.position, false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        onHoverAction?.Invoke(eventData.position, true);
    }
}