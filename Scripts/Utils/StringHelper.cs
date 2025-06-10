using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace SMoonUniversalAsset
{
    public static class StringHelper
    {
        public const string pattern = @"\[translateId:(\d+)\]";

        public static string GenerateUid() => GenerateRandomString(24);
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string randomString = "";

            for (int i = 0; i < length; i++)
            {
                randomString += chars[Random.Range(0, chars.Length)];
            }

            return randomString;
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        public static string FullFormatTime(int timeInMinutes)
        {
            int days = Mathf.FloorToInt(timeInMinutes / (60 * 24));
            int hours = Mathf.FloorToInt(timeInMinutes / 60f % 24);
            int minutes = Mathf.FloorToInt(timeInMinutes % 60f);

            return string.Format("{0} {1:D2}:{2:D2}", days, hours, minutes);
        }

        public static string FormatTime12H(int timeInMinutes)
        {
            int hours24 = Mathf.FloorToInt(timeInMinutes / 60f % 24);
            int minutes = Mathf.FloorToInt(timeInMinutes % 60f);

            int hours12 = hours24 % 12;
            hours12 = (hours12 == 0) ? 12 : hours12;
            string period = hours24 >= 12 ? "PM" : "AM";

            return string.Format("{0:D2}:{1:D2} {2}", hours12, minutes, period);
        }


        public static int GetTranslatedId(string originalString)
        {
            Match match = Regex.Match(originalString, pattern);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }

            return -1;
        }

        public static List<string> ExtractSentencesWithBraces(string input)
        {
            var list = new List<string>();
            var regex = new Regex(@"\{[^}]*\}");
            var matches = regex.Matches(input);

            foreach (Match match in matches)
            {
                list.Add(match.Value);
            }

            return list;
        }

    }
}