using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Library.Domain
{
    public static class Constants
    {
        public const int rocketAmmoSize = 5;
        public const int animationFrameSkip = 3;
        public const float gravity = 0.0084f;
        public static List<Color> backgroundColors = new List<Color>
            {
                Color.CornflowerBlue,
                Color.Crimson,
                Color.BlanchedAlmond,
                Color.Black,
                Color.BlueViolet,
                Color.DarkGreen,
                Color.DarkSlateGray
            };
        public static List<string> levelNames = new List<string>
            {
                "battlefield",
                "nightfall"
            };
    }
}
