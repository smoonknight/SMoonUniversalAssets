using System;
using System.Collections.Generic;
using System.Linq;

namespace SMoonUniversalAsset
{
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

        public static void EnsureSize<T>(this List<T> list, int size, Func<T> factory)
        {
            if (list.Count < size)
            {
                for (int i = list.Count; i < size; i++)
                {
                    list.Add(factory());
                }
            }
        }


        public static void SetAt<T>(this List<T> list, int index, T value)
        {
            list.EnsureSize(index + 1);
            list[index] = value;
        }

        public static void AddOrUpdate<T>(this List<T> list, T data, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            if (index == -1)
            {
                list.Add(data);
            }
            else
            {
                list[index] = data;
            }
        }
    }
}
