using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Helpers.Enum;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.GamePlay.Player;

namespace MonoGame_Template.Scenes.GamePlay.Player
{
    public class Player: ICollider, IGravity
    {
        public Vector2 Position { get; set; } = new Vector2(32, 32);
        public Vector2 Velocity { get; set; }
        public Vector2 MaxVelocity { get; set; } = new Vector2(-4, -10);
        public Texture2D CurrentTexture { get; set; }
        public bool IsGrounded { get; set; }


        private int _currentFrame;
        private List<Texture2D> _idle = new List<Texture2D>();
        private List<Texture2D> _walk = new List<Texture2D>();

        private double _oldGameTime;
        private bool _faceRight = true;
        private const float _movementSpeed = 0.05f;
        private const int _jumpForce = 10;
        private PlayerState _playerState = PlayerState.Idle;
        private TimeSpan lastJumpTime;

        public void LoadContent(ContentManager content)
        {
            _idle.Add(content.Load<Texture2D>("images/player__Idle_0"));
            _idle.Add(content.Load<Texture2D>("images/player__Idle_1"));

            _walk.Add(content.Load<Texture2D>("images/player__Walk_0"));
            _walk.Add(content.Load<Texture2D>("images/player__Walk_1"));
            _walk.Add(content.Load<Texture2D>("images/player__Walk_2"));
            _walk.Add(content.Load<Texture2D>("images/player__Walk_3"));
        }

        public void Update(GameTime gameTime, List<ICollider> colliders)
        {
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            ApplyGravity(colliders, deltaTime);
            Move(colliders, deltaTime);
            Jump(colliders, gameTime, deltaTime);
            
            var clampedVelocity = Vector2.Clamp(Velocity, MaxVelocity, new Vector2(Math.Abs(MaxVelocity.X), Math.Abs(MaxVelocity.Y)));
            Position += clampedVelocity;
        }

        private void Jump(List<ICollider> colliders, GameTime gameTime, int deltaTime)
        {
            var keyboardState = Keyboard.GetState();

            float velocityY = Velocity.Y;
            if (keyboardState.IsKeyDown(Keys.Up) && IsGrounded && Math.Abs((lastJumpTime - gameTime.TotalGameTime).TotalSeconds) > 0.25)
            {
                velocityY -= _jumpForce * deltaTime;
                var movement = new Vector2(0, velocityY);

                if (this.MovementAllowed(movement, colliders, out var lastAllowedPosition))
                {
                    lastJumpTime = gameTime.TotalGameTime;

                    IsGrounded = false;

                    Velocity += movement;
                }
                else
                {
                    Velocity = new Vector2(Velocity.X, 0);
                }
            }
            else
            {
                IsGrounded = false;
            }

        }

        private void Move(List<ICollider> colliders, int deltaTime)
        {
            var keyboardState = Keyboard.GetState();

            var velocityX = Velocity.X;

            bool moving = false;

            if (keyboardState.IsKeyDown(Keys.Right) && Position.X < Main.WindowWidth - 32)
            {
                velocityX += _movementSpeed * deltaTime;
                if (this.MovementAllowed(new Vector2(velocityX, 0), colliders, out var collisionRectangle))
                {
                    _faceRight = true;
                    moving = true;
                }
                else
                {
                    velocityX = 0;

                }
            }
            
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                velocityX -= _movementSpeed * deltaTime;

                if (this.MovementAllowed(new Vector2(velocityX, 0), colliders, out var collisionRectangle))
                {
                    _faceRight = false;
                    moving = true;
                }
                else
                {
                    velocityX = 0;
                }
            }

            if (moving && !(keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Left)))
            {
                Velocity += new Vector2(velocityX, 0);
            }
            else
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }

        private void ApplyGravity(List<ICollider> colliders, int deltaTime)
        {
            var gravityVelocity = new Vector2(0, GamePlay.GravityForce);

            if (this.MovementAllowed(gravityVelocity, colliders, out var lastAllowedValue))
            {
                Velocity += gravityVelocity * deltaTime;
            }
            else
            {
                Velocity = new Vector2(Velocity.X, 0);

                if (lastAllowedValue != null)
                {
                    Position = new Vector2(Position.X, lastAllowedValue.Value.Top - 64); // todo : on devrait utiliser la hauteur du collider
                }

                IsGrounded = true;
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

            CurrentTexture = textureList[_currentFrame];

            return CurrentTexture;
        }
    }
}
