using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Common.Interfaces;

namespace MonoGame_Template.Common.Scenes.GamePlay.Terrain
{
    public class Grass: ICollider
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D CurrentTexture { get; set; }

        public Grass(Vector2 position)
        {
            Position = position;
        }

        public void LoadContent(ContentManager content)
        {
            CurrentTexture = content.Load<Texture2D>("images/grass");
        }

        public void OnCollision(ICollider mainCollider)
        {
            // We do nothing here
        }
    }
}
