using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Template.Scenes.GamePlay.Terrain;
using MonoGame_Template.Scenes.GamePlay.Terrain.Enums;
using MonoGame_Template.Scenes.GamePlay.Terrain.Interfaces;

namespace MonoGame_Template.Common.Helpers
{
    public static class TileMapManager
    {
        public static TerrainType[][] ToEnum(this int[][] tileMap)
        {
            var list = new List<TerrainType[]>();

            foreach (var row in tileMap)
            {
                var secondList = new List<TerrainType>();
                foreach (var column in row)
                {
                    secondList.Add((TerrainType)column);
                }

                list.Add(secondList.ToArray());
            }

            return list.ToArray();
        }

        public static ITerrain[][] Generate(TerrainType[][] tileMap, int tileSize)
        {
            var list = new List<ITerrain[]>();

            var tilePosition = new Vector2(0, 0);

            foreach (var row in tileMap)
            {
                var secondList = new List<ITerrain>();
                tilePosition.X = 0;
                foreach (var column in row)
                {
                    if (column != TerrainType.Nothing)
                    {
                        switch (column)
                        {
                            case TerrainType.Grass:
                                secondList.Add(new Grass(tilePosition));
                                break;
                            case TerrainType.Sun:
                                secondList.Add(new Sun(tilePosition));
                                break;
                            default:
                                throw new NotImplementedException($"{column} is not supported.");
                        }
                    }

                    tilePosition.X += tileSize;
                }

                tilePosition.Y += tileSize;

                list.Add(secondList.ToArray());
            }

            return list.ToArray();

            //_grassList = new List<Grass>();

            //var tileWidth = 32;
            //var numberOfTiles = Main.WindowWidth / tileWidth;
            //var grassPosition = new Vector2(0, Main.WindowHeight - tileWidth*2);

            //for (int i = 0; i < numberOfTiles; i++)
            //{
            //    var newGrass = new Grass(grassPosition);

            //    _grassList.Add(newGrass);
            //    grassPosition.X += 32;
            //}
        }

        public static void LoadContent(ContentManager contentManager)
        {

        }

        public static void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
