using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

using SMoonUniversalAsset;

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
        int minutes = TimeHelper.GetMinutesFromSeconds(timeInSeconds);
        int seconds = TimeHelper.GetSecondsFromSeconds(timeInSeconds);

        return string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public static string FullFormatTime(int timeInMinutes)
    {
        int days = TimeHelper.GetDaysFromMinutes(timeInMinutes);
        int hours = TimeHelper.GetHoursFromMinutes(timeInMinutes);
        int minutes = TimeHelper.GetMinutesFromMinutes(timeInMinutes);

        return string.Format("Day {0} {1:D2}:{2:D2}", days, hours, minutes);
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