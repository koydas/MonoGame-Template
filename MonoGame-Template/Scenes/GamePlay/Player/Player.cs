using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.GamePlay.Player;
using MonoGame_Template.Common.Scenes.GamePlay.Terrain;

namespace MonoGame_Template.Scenes.GamePlay.Player
{
    public class Player: ICollider, IGravity
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D CurrentTexture { get; set; }
        public bool IsGrounded { get; set; }


        private int _currentFrame;
        private readonly List<Texture2D> _idle;
        private List<Texture2D> _walk;

        private double _oldGameTime;
        private bool _faceRight;
        private readonly float _movementSpeed;
        private readonly Vector2 _maxSpeed;
        private readonly int _jumpForce;
        private PlayerState _playerState;
        

        public Player()
        {
            _idle = new List<Texture2D>();
            _walk = new List<Texture2D>();

            Position = new Vector2(32, Main.WindowHeight - 32);
            _faceRight = true;

            _movementSpeed =  0.05f;
            _jumpForce = 1;
            _maxSpeed = new Vector2(-4, -10);
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

            var velocityX = Velocity.X;
            var velocityY = Velocity.Y;

            if (keyboardState.IsKeyDown(Keys.Right) && Position.X < Main.WindowWidth - 32)
            {
                if (velocityX < 0)
                {
                    velocityX = 0;
                    _currentFrame = 0;
                }

                velocityX += _movementSpeed * deltaTime;
                _faceRight = true;
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (Velocity.X > 0)
                {
                    velocityX = 0;
                    _currentFrame = 0;
                }

                velocityX -= _movementSpeed * deltaTime;
                _faceRight = false;
            }

            if (!keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                velocityX = 0;
            }

            if (keyboardState.IsKeyPressed(Keys.Up) && IsGrounded)
            {
                
                velocityY -= _jumpForce * deltaTime;
                IsGrounded = false;
            }
            
            var clampedVelocity = Vector2.Clamp(Velocity, _maxSpeed, new Vector2(Math.Abs(_maxSpeed.X), Math.Abs(_maxSpeed.Y)));
            
            _playerState = velocityX != 0 
                ? PlayerState.Walk 
                : PlayerState.Idle;

            Position += clampedVelocity;
            Velocity = new Vector2(velocityX, velocityY);

            if (Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
            }
            else if (Position.X  > Main.WindowWidth - 64)
            {
                Position = new Vector2(Main.WindowWidth - 64, Position.Y);
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

        public void OnCollision(ICollider collider)
        {
                // TODO : Gérer les collisions sur X   
                Velocity = new Vector2(Velocity.X, 0);
                Position = new Vector2(Position.X, collider.Position.Y - collider.CurrentTexture.Height);
                IsGrounded = true;
            
        }

        private Texture2D Animate(List<Texture2D> textureList, GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds - _oldGameTime > 0.3)
            {
                _currentFrame = _currentFrame == 0 ? 1 : 0;

                _oldGameTime = gameTime.TotalGameTime.TotalSeconds;
            }

            CurrentTexture = textureList[_currentFrame];

            return CurrentTexture;
        }

        
    }
}
