using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Scenes.Interfaces;
using MonoGame_Template.Common.Scenes.Menu.Enums;
using MonoGame_Template.Scenes.Platform;

namespace MonoGame_Template.Scenes.Menu
{
    public class Menu : IScene
    {
        private MenuItem _selectedMenuItem;
        private SpriteFont _font;
        private Vector2 _menuPosition;

        public void Initialize()
        {
            _selectedMenuItem = MenuItem.Platform;

            _menuPosition = new Vector2
            {
                X = Main.WindowWidth / 10,
                Y = Main.WindowHeight / 10
            };
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Arial");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyPressed(Keys.Escape))
            {
                Main.Self.Exit();
            }

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyPressed(Keys.Down) || keyboardState.IsKeyPressed(Keys.Up))
            {
                var index = (int) _selectedMenuItem;

                if (keyboardState.IsKeyDown(Keys.Down))
                    index++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    index--;

                if ((int)MenuItem.Quit < index)
                {
                    index = 0;
                }

                if (index < 0)
                {
                    index = (int) MenuItem.Quit;
                }

                _selectedMenuItem = (MenuItem) index;
            }

            if (keyboardState.IsKeyPressed(Keys.Enter))
            {
                switch (_selectedMenuItem)
                {
                    case MenuItem.Platform:
                        Main.CurrentScene = new Platform.Platform();
                        break;
                    case MenuItem.Zombies:
                        break;
                    case MenuItem.Quit:
                        Main.Self.Exit();
                        break;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            Main.SpriteBatch.Begin();

            Vector2 menuItemPosition = _menuPosition;
            foreach (var menuItem in Enum.GetValues(typeof(MenuItem)))
            {
                Color color = Color.LightSlateGray;
                if (_selectedMenuItem == (MenuItem)menuItem)
                {
                    color = Color.White;
                }

                Main.SpriteBatch.DrawString(_font, menuItem.ToString(), menuItemPosition, color);
                menuItemPosition.Y += _font.LineSpacing;
            }

            Main.SpriteBatch.End();
        }
    }
}
