using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Assets
{
    public abstract class GameObject
    {
        public Vector2 Position { get; protected set; }
        public Vector2 Size { get; protected set; }
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
