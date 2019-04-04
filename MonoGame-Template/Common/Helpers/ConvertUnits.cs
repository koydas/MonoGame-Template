using System;
using Microsoft.Xna.Framework;

namespace MonoGame_Template.Common.Helpers
{
    public static class ConvertUnits
    {
        private static int? _ratio;

        public static int Ratio
        {
            get
            {
                if (_ratio == null)
                    throw new Exception("Ratio must be set before using it.");

                return _ratio.Value;
            }
            set
            {
                if (_ratio == null)
                    _ratio = value;
                else
                    throw new Exception("Can be set only once.");
            }
        }

        public static Vector2 ToSimUnits(this Vector2 displayUnits)
        {
            return displayUnits / Ratio;
        }

        public static Vector2 ToDisplayUnit(this Vector2 displayUnits)
        {
            return displayUnits * Ratio;
        }

        public static float ToSimUnits(this float displayUnit)
        {
            return displayUnit / Ratio;
        }

        public static float ToDisplayUnit(this float displayUnit)
        {
            return displayUnit * Ratio;
        }
    }
}
