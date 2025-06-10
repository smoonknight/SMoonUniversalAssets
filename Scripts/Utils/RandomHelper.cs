using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public static T GetRandom<T>(this List<T> list)
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static T GetRandomWithExcept<T>(this List<T> list, T except)
    {
        if (except == null)
        {
            return list.GetRandom();
        }
        List<T> values = list.Where(v => !v.Equals(except)).ToList();
        return values.GetRandom();
    }

    public static T GetRandomEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }

    public static T GetRandomEnumWithExcept<T>(T except) where T : Enum
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>().Where(v => !v.Equals(except)).ToList();
        if (values.Count == 0) throw new InvalidOperationException("No other enum values available.");
        return values[UnityEngine.Random.Range(0, values.Count)];
    }

    public static void Randomize<T>(this List<T> list)
    {
        var rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}