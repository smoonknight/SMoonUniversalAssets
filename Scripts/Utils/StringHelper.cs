using UnityEngine;

public static class StringHelper
{
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
}