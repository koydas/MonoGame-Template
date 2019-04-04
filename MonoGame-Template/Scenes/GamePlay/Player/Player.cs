using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.GamePlay.Player;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGame_Template.Scenes.GamePlay.Player
{
    public class Player : ICollider
    {
        public Body Body { get; set; }
        public Texture2D CurrentTexture { get; set; }

        public bool IsGrounded { get; set; }

        private int _currentFrame;
        private readonly List<Texture2D> _idle = new List<Texture2D>();
        private readonly List<Texture2D> _walk = new List<Texture2D>();

        private double _oldGameTime;
        private bool _faceRight = true;
        private const float MovementSpeed = 0.0005f;
        private const float JumpForce = 0.003f;
        private PlayerState _playerState = PlayerState.Idle;
        
        public Player()
        {
            Body = GamePlay.World.CreateRectangle(1, 1, 1f, new Vector2(0, 0));
            Body.SetFriction(0.2f);
            Body.SetRestitution(0.2f);
            Body.BodyType = BodyType.Dynamic;

            Body.OnCollision += (sender, other, contact) => IsGrounded = true;
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
            Body.ApplyForce(Vector2.UnitY * 0.0005f);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (Body.LinearVelocity.X < MovementSpeed)
                {
                    Body.ApplyForce(Vector2.UnitX * MovementSpeed);
                    _faceRight = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (Body.LinearVelocity.X > -MovementSpeed)
                {
                    Body.ApplyForce(Vector2.UnitX * -MovementSpeed);
                    _faceRight = false;
                }
            }

            if (IsGrounded && Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (Body.LinearVelocity.Y > -JumpForce)
                {
                    Body.ApplyForce(Vector2.UnitY * -JumpForce);
                }

                IsGrounded = false;
            }

            // Avoid falling out of the screen
            if (Body.Position.X.ToDisplayUnit() < 0)
            {
                Body.Position = new Vector2(0, Body.Position.Y);
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
                    position: Body.Position * 64,
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
