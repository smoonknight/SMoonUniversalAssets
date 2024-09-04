using System.Collections.Generic;

public static class ListExtensions
{
    public static void EnsureSize<T>(this List<T> list, int size, T defaultValue = default)
    {
        if (list.Count < size)
        {
            for (int i = list.Count; i < size; i++)
            {
                list.Add(defaultValue);
            }
        }
    }

    public static void SetAt<T>(this List<T> list, int index, T value)
    {
        list.EnsureSize(index + 1);
        list[index] = value;
    }
}
