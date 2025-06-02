public static class ArrayExtensions
{
    public static void EnsureSize<T>(this T[] array, int size, T defaultValue = default)
    {
        if (array.Length < size)
        {
            for (int i = array.Length; i < size; i++)
            {
                array[i] = defaultValue;
            }
        }
    }

    public static void SetAt<T>(this T[] array, int index, T value)
    {
        array.EnsureSize(index + 1);
        array[index] = value;
    }
}
