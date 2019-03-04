using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Scenes;
using MonoGame_Template.Common.Scenes.Interfaces;
using MonoGame_Template.Common.Scenes.Menu;

namespace MonoGame_Template
{
    public class Main : Game
    {
        public static Main Self;

        public static GraphicsDeviceManager Graphics;
        public static SpriteBatch SpriteBatch;

        public static int WindowHeight { get; set; }
        public static int WindowWidth { get; set; }

        public static IScene CurrentScene;
        public static GameTime GameTime;

        public Main()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Self = this;
        }

        protected override void Initialize()
        {
            WindowWidth = GraphicsDevice.PresentationParameters.Bounds.Width;
            WindowHeight = GraphicsDevice.PresentationParameters.Bounds.Height;

            CurrentScene = new Menu();
            CurrentScene.Initialize();

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            CurrentScene.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            CurrentScene.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Main.GameTime = gameTime;

            CurrentScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            CurrentScene.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}