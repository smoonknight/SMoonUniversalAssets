namespace SMoonUniversalAsset
{
    public static class EaseFunctions
    {
        public static float EaseInQuad(float t) => t * t;

        public static float EaseOutQuad(float t) => t * (2 - t);

        public static float EaseInOutQuad(float t) =>
            t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;

        public static float EaseInCubic(float t) => t * t * t;

        public static float EaseOutCubic(float t)
        {
            t -= 1f;
            return t * t * t + 1f;
        }

        public static float EaseInOutCubic(float t)
        {
            return t < 0.5f
                ? 4f * t * t * t
                : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;
        }

        public static float EaseOutBounce(float t)
        {
            if (t < 1f / 2.75f)
                return 7.5625f * t * t;
            else if (t < 2f / 2.75f)
            {
                t -= 1.5f / 2.75f;
                return 7.5625f * t * t + 0.75f;
            }
            else if (t < 2.5f / 2.75f)
            {
                t -= 2.25f / 2.75f;
                return 7.5625f * t * t + 0.9375f;
            }
            else
            {
                t -= 2.625f / 2.75f;
                return 7.5625f * t * t + 0.984375f;
            }
        }
    }

}