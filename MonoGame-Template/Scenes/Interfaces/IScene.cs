using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGame_Template.Common.Scenes.Interfaces
{
    public interface IScene
    {
        void Initialize();
        void LoadContent(ContentManager content);
        void UnloadContent();

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}
