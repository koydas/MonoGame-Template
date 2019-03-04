using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Template.Common.Helpers;
using MonoGame_Template.Common.Scenes.Interfaces;
using MonoGame_Template.Common.Scenes.Menu.Enums;

namespace MonoGame_Template.Common.Scenes.Menu
{
    public class Menu : IScene
    {
        private MenuItem _selectedMenuItem;
        private SpriteFont _font;
        private Vector2 _menuPosition;

        public void Initialize()
        {
            _selectedMenuItem = MenuItem.Play;

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
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyPressed(Keys.Down) || keyboardState.IsKeyPressed(Keys.Up))
            {
                _selectedMenuItem = _selectedMenuItem == MenuItem.Play
                    ? MenuItem.Quit
                    : MenuItem.Play;
            }

            if (keyboardState.IsKeyPressed(Keys.Enter))
            {
                switch (_selectedMenuItem)
                {
                    case MenuItem.Play:
                        Main.CurrentScene = new GamePlay();
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
