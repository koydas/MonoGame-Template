using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Template.Common.Helpers
{
    public static class TextureHelper
    {
        public static Color GetPixel(this Color[] colors, int x, int y, int width)
        {
            return colors[x + (y * width)];
        }
        public static Color[] GetPixels(this Texture2D texture)
        {
            var colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            return colors;
        }
    }
}
