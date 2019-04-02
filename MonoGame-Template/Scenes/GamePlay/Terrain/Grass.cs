using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame_Template.Scenes.GamePlay.Terrain
{
    public class Grass: ICollider, ITerrain
    {
        private Body body;
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 MaxVelocity { get; set; }
        public Texture2D CurrentTexture { get; set; }

        public Grass(Vector2 position)
        {
            Position = position;
            body = GamePlay.World.CreateRectangle(64, 64, 1f, Position);
            body.BodyType = BodyType.Static;
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
