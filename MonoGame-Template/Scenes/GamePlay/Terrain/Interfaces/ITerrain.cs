using Microsoft.Xna.Framework.Content;
using MonoGame_Template.Common.Interfaces;

namespace MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces
{
    public interface ITerrain : ICollider
    {
        void LoadContent(ContentManager content);
    }
}
