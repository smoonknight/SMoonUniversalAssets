using UnityEngine;

[System.Serializable]
public class UnscaleTimeChecker : TimeChecker
{
    public override float GetTime() => Time.unscaledTime;
}