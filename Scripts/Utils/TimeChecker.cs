using System;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class TimeChecker
{
    public TimeChecker()
    {
        SetLastTimeInfinity();
    }
    public TimeChecker(float duration)
    {
        this.duration = duration;
        SetLastTimeInfinity();
    }
    public TimeChecker(float duration, bool waitingDuration)
    {
        this.duration = duration;
        if (waitingDuration)
        {
            UpdateTime();
            return;
        }
        SetLastTimeInfinity();
    }
    public float duration;
    protected float lastTime;

    public bool IsDurationEnd() => lastTime + duration <= GetTime();
    public void UpdateTime() => lastTime = GetTime();
    public void UpdateTime(float changedDuration)
    {
        duration = changedDuration;
        UpdateTime();
    }

    public float PercentageToEndDuration() => Mathf.Clamp01((GetTime() - lastTime) / duration);
    public float PercentageToStartDuration() => 1.0f - PercentageToEndDuration();
    public bool IsDurationPassPercentage(float percentage) => PercentageToEndDuration() > percentage;
    public void SetLastTimeInfinity() => lastTime = Mathf.NegativeInfinity;

    public bool IsUpdatingTime()
    {
        if (IsDurationEnd())
        {
            UpdateTime();
            return true;
        }
        return false;
    }
    public void TryUpdateOnDurationEnd(out bool isDurationEnd)
    {
        isDurationEnd = IsUpdatingTime();
    }

    public virtual float GetTime() => Time.time;
}