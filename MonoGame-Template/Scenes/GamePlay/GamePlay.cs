using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.Interfaces;
using MonoGame_Template.Scenes.GamePlay.Terrain.Enums;
using MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame_Template.Scenes.GamePlay
{
    public class GamePlay : IScene
    {
        private Player.Player _player;
        private readonly Camera2D _camera;
        
        private ITerrain[][] _tiles;
        public static TerrainType[][] TilemapEnum;
        public static World World = new World(Vector2.Zero);

        public GamePlay()
        {
            Initialize();
            LoadContent(Main.ContentManager);
            _camera = new Camera2D();
        }

        public static int TileSize = 64;

        public void Initialize()
        {
            _player = new Player.Player();

            int[][] tilemap = {
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                new [] {1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
                new [] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

            TilemapEnum = tilemap.ToEnum();
            _tiles = TileMapManager.Generate(TilemapEnum, TileSize);
        }

        public void LoadContent(ContentManager content)
        {
            _player.LoadContent(content);

            TileMapManager.LoadContent(content);

            foreach (var tile in _tiles.SelectMany(x => x.Select(y => y)))
            {
                tile.LoadContent(content);
            }
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyPressed(Keys.Escape))
            {
                Main.CurrentScene = new Menu.Menu();
                Main.CurrentScene.Initialize();
                Main.CurrentScene.LoadContent(Main.ContentManager);
            }

            World.Step(deltaTime);

            _player.Update(gameTime);

            _camera.Position = _player.Body.Position.X*64 > Main.WindowWidth / 2 
                ? new Vector2(_player.Body.Position.X*64, Main.WindowHeight / 2) 
                : new Vector2(Main.WindowWidth / 2, Main.WindowHeight / 2);
        }

        public void Draw(GameTime gameTime)
        {
            Main.Graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            Main.SpriteBatch.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                _camera.GetTransformation(Main.Graphics.GraphicsDevice));

            // Draw Terrain
            TileMapManager.Draw(Main.SpriteBatch);

            foreach (var tile in _tiles.SelectMany(x => x.Select(y => y)))
            {
                Main.SpriteBatch.Draw(tile.CurrentTexture, tile.Body.Position * 64, Color.White);
            }

            _player.Draw(gameTime);

            Main.SpriteBatch.End();
        }
    }
}
