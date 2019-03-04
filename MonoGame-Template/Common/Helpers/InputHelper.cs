using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Template.Common.Helpers
{
    public static class InputHelper
    {
        private static readonly Dictionary<Keys, double> OldGameTimeByKey = new Dictionary<Keys, double>();
        private const double AcceptedTimeDifference = 0.2;

        public static bool IsKeyPressed(this KeyboardState keyboardState, Keys pressedKey)
        {
            var newGameTime = Main.GameTime.TotalGameTime.TotalSeconds;

            bool result = keyboardState.IsKeyDown(pressedKey) && (newGameTime - GetOldGameTime(pressedKey) > AcceptedTimeDifference);

            if (!result)
            {
                return false;
            }

            SetOldGameTime(pressedKey, newGameTime);

            return true;
        }

        private static double GetOldGameTime(Keys pressedKey)
        {
            double oldGameTime = 0;

            if (OldGameTimeByKey.ContainsKey(pressedKey))
            {
                oldGameTime = OldGameTimeByKey[pressedKey];
            }

            return oldGameTime;
        }

        private static void SetOldGameTime(Keys pressedKey, double newGameTime)
        {
            if (OldGameTimeByKey.ContainsKey(pressedKey))
            {
                OldGameTimeByKey[pressedKey] = newGameTime;
            }
            else
            {
                OldGameTimeByKey.Add(pressedKey, newGameTime);
            }
        }
    }
}
