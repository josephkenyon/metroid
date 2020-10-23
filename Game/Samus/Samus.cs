using Library;
using Microsoft.Xna.Framework.Graphics;
using static Library.Enums;

namespace Game1
{
    internal class Samus
    {
        private readonly Texture2D spriteTexture;
        private readonly Animation[] animations;

        private Animations CurrentAnimation { get; set; }
        public Samus(Texture2D spriteTexture)
        {
            this.spriteTexture = spriteTexture;
            animations = new Animation[]
            {

            };
        }
    }
}
