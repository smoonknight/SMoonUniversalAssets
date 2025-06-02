public static class NumberExtensions
{
    public static bool IsBetween(this float value, float minValue, float maxValue)
    {
        return value >= minValue && value <= maxValue;
    }

    public static bool IsBetween(this float value, float minValue, float maxValue, float tolerance) => IsBetween(value, minValue - tolerance, maxValue + tolerance);

    public static bool IsBetween(this int value, int minValue, int maxValue)
    {
        return minValue <= maxValue
                ? value >= minValue && value <= maxValue
                : value >= minValue || value <= maxValue;
    }
}
