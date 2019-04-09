using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Scenes.Platform.Terrain.Interfaces;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame_Template.Scenes.Platform.Terrain
{
    public class Grass: ITerrain
    {
        public Body Body { get; set; }
        public Texture2D CurrentTexture { get; set; }

        public Grass(Vector2 position)
        {
            Body = Platform.World.CreateRectangle(1, 1, 1f, position / 64);
        }

        public void LoadContent(ContentManager content)
        {
            CurrentTexture = content.Load<Texture2D>("images/grass");
        }
    }
}
