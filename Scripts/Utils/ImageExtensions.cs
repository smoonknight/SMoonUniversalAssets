using UnityEngine;
using UnityEngine.UI;

namespace SMoonUniversalAsset
{
    public static class ImageExtensions
    {
        public static void SetOpacity(this Image image, float opacity)
        {
            Color color = image.color;
            color.a = opacity;
            image.color = color;
        }
    }
}