using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Template.Scenes.GamePlay.Decor
{
    public class Sun : IDecor
    {
        public Vector2 Position { get; set; }
        public Texture2D CurrentTexture { get; set; }

        public Sun(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager content)
        {
            CurrentTexture = content.Load<Texture2D>("images/sun");
        }
    }
}
