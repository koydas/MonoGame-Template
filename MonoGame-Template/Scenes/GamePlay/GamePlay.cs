using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.Interfaces;
using MonoGame_Template.Scenes.GamePlay.Terrain.Enums;
using MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces;

namespace MonoGame_Template.Scenes.GamePlay
{
    public class GamePlay : IScene
    {
        public List<Texture2D> idle;
        public readonly static float GravityForce = 1;
        private Player.Player _player;

        private List<ICollider> _colliders;
        private ITerrain[][] _tiles;
        public static TerrainType[][] TilemapEnum;

        public GamePlay()
        {
            Initialize();
            LoadContent(Main.ContentManager);
        }

        public static int TileSize = 64;

        public void Initialize()
        {
            _colliders = new List<ICollider>();
            _player = new Player.Player();

            int[][] tilemap = {
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                new [] {1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
                new [] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

            TilemapEnum = tilemap.ToEnum();
            _tiles = TileMapManager.Generate(TilemapEnum, TileSize);


            var tilesFlatList = _tiles
                .SelectMany(x => x.Select(y => y))
                .Where(x => x is ICollider)
                .Cast<ICollider>()
                .ToArray();

            _colliders.Add(_player);
            _colliders.AddRange(tilesFlatList);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyPressed(Keys.Escape))
            {
                Main.CurrentScene = new Menu.Menu();
                Main.CurrentScene.Initialize();
                Main.CurrentScene.LoadContent(Main.ContentManager);
            }

            //foreach (ICollider collider in _colliders)
            //{
            //    var newVelocity = new Vector2(collider.Velocity.X, collider.Velocity.Y);
            //    if (collider is IGravity gravity && !gravity.IsGrounded)
            //    {
            //        newVelocity.Y = collider.Velocity.Y + 1;

            //        if (collider.MovementAllowed(newVelocity, _colliders))
            //        {
            //            collider.Velocity = newVelocity;
            //        }
            //        else
            //        {
            //            collider.Velocity = new Vector2();
            //        }
            //    }
            //}

            _player.Update(gameTime,_colliders);

            
        }

        public void Draw(GameTime gameTime)
        {
            Main.Graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            Main.SpriteBatch.Begin();
            
            // Draw Terrain
            TileMapManager.Draw(Main.SpriteBatch);

            foreach (var tile in _tiles.SelectMany(x => x.Select(y => y)))
            {
                Main.SpriteBatch.Draw(tile.CurrentTexture, tile.Position, Color.White);
            }

            _player.Draw(gameTime);

            Main.SpriteBatch.End();
        }
    }
}
