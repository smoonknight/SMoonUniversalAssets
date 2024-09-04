using UnityEngine;

public class TimeWatcher
{
    float startTime;
    float endTime = 0;

    float GetTime => endTime - startTime;
    public TimeWatcher()
    {
        ResetTime();
    }

    public float GetTimePassed()
    {
        SnapTime();
        return GetTime;
    }

    public void ResetTime()
    {
        startTime = Time.time;
        endTime = Time.time;
    }
    public void SnapTime() => endTime = Time.time;

    public bool IsTimePassedReachTarget(float target) => GetTimePassed() < target;

    public string GetStringFormatter() => StringHelper.FormatTime(GetTimePassed());
}