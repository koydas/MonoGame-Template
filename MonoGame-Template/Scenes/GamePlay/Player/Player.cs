using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Enums;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Interfaces;
using MonoGame_Template.Common.Scenes.GamePlay.Player;
using MonoGame_Template.Scenes.GamePlay.Player.Helpers;
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
        internal Direction Direction = Direction.Right;
        
        private PlayerState _playerState = PlayerState.Idle;

        internal float TeleportOpacity = 0;

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
            var deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            if(this.Teleport(deltaTime))
                return;

            this.ApplyGravity();
            this.Move();
            this.Jump();
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
                SpriteEffects effect;
                switch (Direction)
                {
                    case Direction.Left:
                        effect = SpriteEffects.FlipHorizontally;
                        break;
                    case Direction.Right:
                        effect = SpriteEffects.None;
                        break;
                    default:
                        throw new System.Exception("Unsupported Direction");
                }

                Main.SpriteBatch.Draw(
                    texture: texture2D,
                    position: Body.Position * 64,
                    sourceRectangle: null,
                    color: Color.White * TeleportOpacity,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: 1,
                    effects: effect,
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
