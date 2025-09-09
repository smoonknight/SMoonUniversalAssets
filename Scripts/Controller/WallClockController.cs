using System;
using UnityEngine;

public class WallClockController : MonoBehaviour
{
    [SerializeField]
    private Transform hourNeedle;
    [SerializeField]
    private float hourRotateAdjust;
    [SerializeField]
    private Transform minutesNeedle;
    [SerializeField]
    private float minutesRotateAdjust;

    private const float HoursPerHalfDay = 12f;
    private const float MinutesPerHour = 60f;
    private const float fullRotate = 360f;

    public void UpdateClock(int hours, int minutes)
    {
        Debug.Log($"{HourRotate(hours, minutes)} : {MinuteRotate(minutes)}");
        hourNeedle.localRotation = Quaternion.Euler(0, HourRotate(hours, minutes), 0);
        minutesNeedle.localRotation = Quaternion.Euler(0, MinuteRotate(minutes), 0);
    }

    public float HourRotate(int hours, int minutes)
    {
        float hourNormalized = (hours % 12) + (minutes / MinutesPerHour);
        return (fullRotate * (hourNormalized / HoursPerHalfDay)) + hourRotateAdjust;
    }

    public float MinuteRotate(int minutes)
        => (fullRotate * (minutes / MinutesPerHour)) + minutesRotateAdjust;

}