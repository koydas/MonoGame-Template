using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Common.Scenes.GamePlay.Terrain;
using MonoGame_Template.Common.Scenes.Interfaces;

namespace MonoGame_Template.Common.Scenes.GamePlay
{
    public class GamePlay : IScene
    {
        public List<Texture2D> idle;

        private Player.Player _player;
        private List<Grass> _grassList;

        public GamePlay()
        {
            Initialize();
            LoadContent(Main.ContentManager);
        }

        public void Initialize()
        {
            _player = new Player.Player();
            _grassList = new List<Grass>();

            var tileWidth = 32;
            var numberOfTiles = Main.WindowWidth / tileWidth;
            var grassPosition = new Vector2(0, Main.WindowHeight - tileWidth*2);

            for (int i = 0; i < numberOfTiles; i++)
            {
                var newGrass = new Grass(grassPosition);
                
                _grassList.Add(newGrass);

                grassPosition.X += 32;
            }
        }

        public void LoadContent(ContentManager content)
        {
            _player.LoadContent(content);

            foreach (var grass in _grassList)
            {
                grass.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Main.Graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            Main.SpriteBatch.Begin();

            _player.Draw(gameTime);

            // Draw Terrain

            foreach (var grass in _grassList)
            {
                Main.SpriteBatch.Draw(grass.Texture, grass.Position, Color.White);
            }
            

            Main.SpriteBatch.End();
        }
    }
}
