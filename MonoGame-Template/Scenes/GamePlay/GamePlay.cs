using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.GamePlay.Terrain;
using MonoGame_Template.Common.Scenes.Interfaces;
using MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces;

namespace MonoGame_Template.Scenes.GamePlay
{
    public class GamePlay : IScene
    {
        public List<Texture2D> idle;

        private MonoGame_Template.Scenes.GamePlay.Player.Player _player;

        private List<ICollider> colliders;
        private ITerrain[][] _tiles;

        public GamePlay()
        {
            Initialize();
            LoadContent(Main.ContentManager);
        }

        public void Initialize()
        {
            colliders = new List<ICollider>();
            _player = new MonoGame_Template.Scenes.GamePlay.Player.Player();

            int[][] tilemap = {
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new [] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                new [] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
            };

            var tilemapenum = tilemap.ToEnum();
            _tiles = TileMapManager.Generate(tilemapenum);


            var tilesFlatList = _tiles
                .SelectMany(x => x.Select(y => y))
                .Where(x => x is ICollider)
                .Cast<ICollider>()
                .ToArray();

            colliders.Add(_player);
            colliders.AddRange(tilesFlatList);
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
            _player.Update(gameTime);

            foreach (ICollider collider in colliders)
            {
                if (collider is IGravity gravity && !gravity.IsGrounded)
                {
                    var velocityY = collider.Velocity.Y + 1;
                    collider.Velocity = new Vector2(collider.Velocity.X, velocityY);
                }

                collider.IsColliding(colliders);
            }
        }

        public void Draw(GameTime gameTime)
        {
            Main.Graphics.GraphicsDevice.Clear(Color.LightSkyBlue);

            Main.SpriteBatch.Begin();

            _player.Draw(gameTime);

            // Draw Terrain
            TileMapManager.Draw(Main.SpriteBatch);

            foreach (var tile in _tiles.SelectMany(x => x.Select(y => y)))
            {
                Main.SpriteBatch.Draw(tile.CurrentTexture, tile.Position, Color.White);
            }

            Main.SpriteBatch.End();
        }
    }
}
