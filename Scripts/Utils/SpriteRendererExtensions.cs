using UnityEngine;

namespace SMoonUniversalAsset
{
    public static class SpriteRendererExtensions
    {
        public static void SetOpacity(this SpriteRenderer spriteRenderer, float opacity)
        {
            if (spriteRenderer == null)
            {
                return;
            }
            Color color = spriteRenderer.color;
            color.a = opacity;
            spriteRenderer.color = color;
        }
    }
}