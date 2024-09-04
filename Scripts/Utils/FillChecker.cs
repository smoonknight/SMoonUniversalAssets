using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class FillChecker
{

    public FillChecker()
    {

    }
    public FillChecker(float fillRequired, bool isFill = false)
    {
        this.fillRequired = fillRequired;
        if (isFill == true)
        {
            fillAmount = fillRequired;
        }
    }

    public float fillAmount = 0;
    public float fillRequired;

    public void ResetFill() => fillAmount = 0;
    public void ReduceFill(float amount) => ChangeFill(-amount);
    public void AddFill() => AddFill(1);
    public void AddFill(float amount) => ChangeFill(amount);
    public void ChangeFill(float fill) => fillAmount = Mathf.Clamp(fillAmount + fill, 0, fillRequired);
    public bool PushFill()
    {
        return PushFill(1);
    }
    public bool PushFill(float amount)
    {
        if (IsFillComplete())
        {
            ResetFill();
            return true;
        }
        AddFill(amount);
        return false;
    }
    public float FillPercentage() => Mathf.Clamp01(fillAmount / fillRequired);
    public bool IsNotFilled() => fillAmount != 0;
    public bool IsFillComplete() => fillAmount >= fillRequired;
}