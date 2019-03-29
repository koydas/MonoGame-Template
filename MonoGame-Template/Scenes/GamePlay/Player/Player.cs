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
        //private TimeSpan lastJumpTime;

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
            
            MoveLogic(colliders, deltaTime);
            ApplyGravity(colliders, deltaTime);
            Jump(colliders, gameTime, deltaTime);
            

            var clampedVelocity = Vector2.Clamp(Velocity, MaxVelocity, new Vector2(Math.Abs(MaxVelocity.X), Math.Abs(MaxVelocity.Y)));
            Position += clampedVelocity;
        }

        private void Jump(List<ICollider> colliders, GameTime gameTime, int deltaTime)
        {
            var keyboardState = Keyboard.GetState();

            if (IsGrounded && keyboardState.IsKeyPressed(Keys.Up))
            {
                var movement = new Vector2(Velocity.X, -_jumpForce*deltaTime);
        
                // Jump
                Velocity += movement;
                IsGrounded = false;                
            }

            // If we hit something
            if (!this.MovementAllowed(Vector2.Zero, colliders, out var collisionRectangle) && collisionRectangle != null && CollisionManager.IsVertical(collisionRectangle.Value) && collisionRectangle.Value.Bottom > Position.Y)
            {
                Velocity = new Vector2(Velocity.X, 0);
                Position = new Vector2(Position.X, collisionRectangle.Value.Bottom);
            }
        }

        private void MoveLogic(List<ICollider> colliders, int deltaTime)
        {
            if (Move(Keys.Right, colliders) == Move(Keys.Left, colliders))
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }

        private bool Move(Keys keypressed, List<ICollider> colliders)
        {
            Vector2 movement;
            bool insideScreen;
            switch (keypressed)
            {
                case Keys.Right:
                    movement = new Vector2(1, 0);
                    insideScreen = Position.X < Main.WindowWidth - CurrentTexture.Width;
                    break;
                case Keys.Left:
                    movement = new Vector2(-1, 0);
                    insideScreen = Position.X > 0;
                    break;
                default:
                    throw new NotImplementedException("Unsupported key");
            }
            
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(keypressed) && insideScreen)
            {
                if (this.MovementAllowed(movement, colliders, out var collisionRectangle))
                {
                    Velocity += movement;
                    _faceRight = Velocity.X >= 0;

                    return true;
                }

                if (collisionRectangle != null && !CollisionManager.IsVertical(collisionRectangle.Value))
                {
                    var x = Position.X;
                    switch (keypressed)
                    {
                        case Keys.Right:
                            x = collisionRectangle.Value.Left - CurrentTexture.Width;
                            break;
                        case Keys.Left:
                            x = collisionRectangle.Value.Right;
                            break;
                    }

                    Position = new Vector2(x, Position.Y);
                }
            }
            
            return false;
        }

        private void ApplyGravity(List<ICollider> colliders, int deltaTime)
        {
            var gravityVelocity = new Vector2(0, GamePlay.GravityForce);

            if (this.MovementAllowed(gravityVelocity, colliders, out var collisionRectangle))
            {
                Velocity += gravityVelocity * deltaTime;
                IsGrounded = false;
            }
            else if (collisionRectangle != null && CollisionManager.IsVertical(collisionRectangle.Value))
            {
                IsGrounded = true;
                Velocity = new Vector2(Velocity.X, 0);
                Position = new Vector2(Position.X, collisionRectangle.Value.Top - CurrentTexture.Height);
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
