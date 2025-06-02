using TMPro;
using UnityEngine;

public abstract class TextMeshProUGUIOnTranslateBase : TextMeshProUGUI
{
    public bool translateOnAwake = true;

    protected override void OnEnable()
    {
        base.OnEnable();
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        if (translateOnAwake)
        {
            Translate();
        }
    }

    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        if (translateOnAwake)
        {
            Translate();
        }
    }

    public abstract void Translate();
}