using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RandomHelper
{
    public static int Range(int minimum, int maximum) => Random.Range(minimum, maximum);
    public static int Range(int maximumRange) => Random.Range(0, maximumRange);
    public static float Range(float minimum, float maximum) => Random.Range(minimum, maximum);
    public static bool IsRateUp() => IsRateUp(5);
    public static bool IsRateUp(float minimumRate) => Random.Range(0, 100f) < minimumRate;
    public static T[] RandomArray<T>(T[] array)
    {
        System.Random random = new();
        var enumerable = array.OrderBy(x => random.Next());
        return enumerable.ToArray();
    }
    public static List<T> RandomList<T>(List<T> list)
    {
        System.Random random = new();
        var enumerable = list.OrderBy(x => random.Next());
        return enumerable.ToList();
    }


    public static T GetRandomElement<T>(List<T> list)
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
    public static T GetRandomElement<T>(T[] list)
    {
        int randomIndex = Random.Range(0, list.Length);
        return list[randomIndex];
    }
}