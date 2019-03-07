using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Template.Common.Scenes.GamePlay.Player
{
    public class Player
    {
        public Vector2 Position;
        public Vector2 Velocity;

        private int _currentFrame;
        private readonly List<Texture2D> _idle;
        private List<Texture2D> _walk;

        private double _oldGameTime;
        private bool _faceRight;
        private readonly float _movementSpeed;
        private readonly Vector2 _maxSpeed;
        private PlayerState _playerState;
        

        public Player()
        {
            _idle = new List<Texture2D>();
            _walk = new List<Texture2D>();

            Position = new Vector2(32, Main.WindowHeight - 128);
            _faceRight = true;

            _movementSpeed =  0.05f;
            _maxSpeed = new Vector2(-2, 0);
            _playerState = PlayerState.Idle;
        }

        public void LoadContent(ContentManager content)
        {
            _idle.Add(content.Load<Texture2D>("images/player__Idle_0"));
            _idle.Add(content.Load<Texture2D>("images/player__Idle_1"));

            _walk.Add(content.Load<Texture2D>("images/player__Walk_0"));
            _walk.Add(content.Load<Texture2D>("images/player__Walk_1"));
            _walk.Add(content.Load<Texture2D>("images/player__Walk_2"));
            _walk.Add(content.Load<Texture2D>("images/player__Walk_3"));
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds;
            var keyboardState = Keyboard.GetState();

            
            if (keyboardState.IsKeyDown(Keys.Right) && Position.X < Main.WindowWidth - 32)
            {
                if (Velocity.X < 0)
                {
                    Velocity.X = 0;
                    _currentFrame = 0;
                }

                Velocity.X += _movementSpeed * deltaTime;
                _faceRight = true;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (Velocity.X > 0)
                {
                    Velocity.X = 0;
                    _currentFrame = 0;
                }

                Velocity.X -= _movementSpeed * deltaTime;
                _faceRight = false;
            }

            if (!keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                Velocity.X = 0;
            }


            var clampedVelocity = Vector2.Clamp(Velocity, _maxSpeed, new Vector2(Math.Abs(_maxSpeed.X), 0));
            
            if (Velocity.X != 0)
            {
                _playerState = PlayerState.Walk;
            }
            else
            {
                _playerState = PlayerState.Idle;
            }

            Position += clampedVelocity;

            if (Position.X < 0)
            {
                Position.X = 0;
            }
            else if (Position.X  > Main.WindowWidth - 64)
            {
                Position.X = Main.WindowWidth - 64;
            }
        }

        public void Draw(GameTime gameTime)
        {
            Texture2D texture2D = null;

            // TODO sortir dans un animationManager
            switch (_playerState)
            {
                case PlayerState.Idle:
                    texture2D = Animate(_idle, gameTime);
                    break;
                case PlayerState.Walk:
                    texture2D = Animate(_walk, gameTime);
                    break;
            }

            if (texture2D != null)
            {
                Main.SpriteBatch.Draw(
                    texture: texture2D,
                    position: Position,
                    sourceRectangle: null,
                    color: Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1,
                    effects: _faceRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    layerDepth: 1);
            }
        }

        private Texture2D Animate(List<Texture2D> textureList, GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - _oldGameTime > 0.3)
            {
                _currentFrame = _currentFrame == 0 ? 1 : 0;

                _oldGameTime = gameTime.TotalGameTime.TotalSeconds;
            }

            return textureList[_currentFrame];
        }
    }
}
