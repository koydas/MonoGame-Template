using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Template.Common.Scenes.GamePlay.Player
{
    public class Player
    {
        private int _idleCurrent;
        private readonly List<Texture2D> _idle;
        private double oldgametime;

        public Player()
        {
            _idle = new List<Texture2D>();
        }

        public void LoadContent(ContentManager content)
        {
            _idle.Add(content.Load<Texture2D>("images/player__Idle_0"));
            _idle.Add(content.Load<Texture2D>("images/player__Idle_1"));
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - oldgametime > 0.3)
            {
                _idleCurrent = _idleCurrent == 0 ? 1 : 0;

                oldgametime = gameTime.TotalGameTime.TotalSeconds;
            }

            Main.SpriteBatch.Draw(_idle[_idleCurrent], Vector2.One, Color.White);
        }
    }
}
