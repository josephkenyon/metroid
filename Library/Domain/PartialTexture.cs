using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{
    public class PartialTexture
    {
        public readonly Rectangle textureLocation;
        public readonly Texture2D sourceTexture;
        public PartialTexture(Texture2D sourceTexture, Rectangle? textureLocation = null)
        {
            this.sourceTexture = sourceTexture;
            this.textureLocation = textureLocation ?? sourceTexture.Bounds;
        }

    }
}
