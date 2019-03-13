﻿using System;
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
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 MaxVelocity { get; set; }
        public Texture2D CurrentTexture { get; set; }
        public bool IsGrounded { get; set; }


        private int _currentFrame;
        private readonly List<Texture2D> _idle;
        private List<Texture2D> _walk;

        private double _oldGameTime;
        private bool _faceRight;
        private readonly float _movementSpeed;
        private readonly int _jumpForce;
        private PlayerState _playerState;
        private TimeSpan lastJumpTime;


        public Player()
        {
            _idle = new List<Texture2D>();
            _walk = new List<Texture2D>();

            Position = new Vector2(32, 32);
            //Position = new Vector2(32, Main.WindowHeight - 32);
            _faceRight = true;

            _movementSpeed =  0.05f;
            _jumpForce = 1;
            MaxVelocity = new Vector2(-4, -10);
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

            if(keyboardState.IsKeyDown(Keys.Up) && IsGrounded && Math.Abs((lastJumpTime - gameTime.TotalGameTime).TotalSeconds) > 0.25)
            {
                lastJumpTime = gameTime.TotalGameTime;
                velocityY -= _jumpForce * deltaTime;
                IsGrounded = false;
            }
            else
            {
                IsGrounded = false;
            }

            Velocity = new Vector2(velocityX, velocityY);
            var clampedVelocity = Vector2.Clamp(Velocity, MaxVelocity, new Vector2(Math.Abs(MaxVelocity.X), Math.Abs(MaxVelocity.Y)));
            
            _playerState = velocityX != 0 
                ? PlayerState.Walk 
                : PlayerState.Idle;

            Position += clampedVelocity;
            

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
            var collisionType = this.GetCollisionType(collider);

            var velocity = new Vector2(Velocity.X, Velocity.Y);
            var position = new Vector2(Position.X, Position.Y);

            if (collisionType == CollisionSide.Top)
            {
                IsGrounded = true;
                velocity.Y = 0;
                position.Y = collider.Position.Y - collider.CurrentTexture.Height;
            }

            if (collisionType == CollisionSide.Bottom)
            {
                velocity.Y = 0;
                position.Y = collider.GetRect().Bottom + collider.GetRect().Height;
            }

            if (collisionType == CollisionSide.Right)
            {
                velocity.X = 0;
                position.X = collider.GetRect().Right;
            }

            if (collisionType == CollisionSide.Left)
            {
                velocity.X = 0;
                position.X = collider.GetRect().Left - CurrentTexture.Width;
            }
            
            var clampedVelocity = Vector2.Clamp(velocity, MaxVelocity, new Vector2(Math.Abs(MaxVelocity.X), Math.Abs(MaxVelocity.Y)));

            Velocity = clampedVelocity;
            Position = position;
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
