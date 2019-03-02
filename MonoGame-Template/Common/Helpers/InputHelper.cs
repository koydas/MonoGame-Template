using Microsoft.Xna.Framework.Input;

namespace MonoGame_Template.Common.Helpers
{
    public static class InputHelper
    {
        private static KeyboardState _oldKeyboardState;

        public static bool IsKeyPressed(this KeyboardState keyboardState, Keys key)
        {
            bool result = _oldKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key);

            _oldKeyboardState = keyboardState;

            return result;
        }
    }
}
