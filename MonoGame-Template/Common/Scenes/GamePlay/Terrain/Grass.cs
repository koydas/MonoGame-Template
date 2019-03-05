using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Template.Common.Scenes.GamePlay.Terrain
{
    public class Grass
    {
        public Texture2D Texture;
        public Vector2 Position { get; set; }

        public Grass(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("images/grass");
        }
    }
}
