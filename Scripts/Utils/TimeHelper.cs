using System;
using UnityEngine;

namespace SMoonUniversalAsset
{
    public static class TimeHelper
    {
        private const int SecondsPerMinute = 60;
        private const int MinutesPerHour = 60;
        private const int HoursPerDay = 24;
        private const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;
        private const int SecondsPerDay = SecondsPerHour * HoursPerDay;

        public static int GetDaysFromMinutes(int timeInMinutes) => timeInMinutes / (MinutesPerHour * HoursPerDay);
        public static int GetHoursFromMinutes(int timeInMinutes) => timeInMinutes / MinutesPerHour % HoursPerDay;
        public static int GetMinutesFromMinutes(int timeInMinutes) => timeInMinutes % MinutesPerHour;
        public static int GetSecondsFromMinutes(int timeInMinutes) => timeInMinutes * SecondsPerMinute;

        public static int GetDaysFromSeconds(float timeInSeconds) => Mathf.FloorToInt(timeInSeconds / SecondsPerDay);
        public static int GetHoursFromSeconds(float timeInSeconds) => Mathf.FloorToInt(timeInSeconds % SecondsPerDay / SecondsPerHour);
        public static int GetMinutesFromSeconds(float timeInSeconds) => Mathf.FloorToInt(timeInSeconds % SecondsPerHour / SecondsPerMinute);
        public static int GetSecondsFromSeconds(float timeInSeconds) => Mathf.FloorToInt(timeInSeconds % SecondsPerMinute);
    }
}