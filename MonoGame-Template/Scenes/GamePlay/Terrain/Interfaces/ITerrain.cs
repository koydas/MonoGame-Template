using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces
{
    public interface ITerrain
    {
        Vector2 Position { get; set; }
        Texture2D CurrentTexture { get; set; }

        void LoadContent(ContentManager content);
    }
}
