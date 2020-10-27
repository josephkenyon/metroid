using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public abstract class GameObject
    {
        public Vector2 Position { get; protected set; }
        public Point CurrentQuadrant => (Position / tileSize).ToPoint();
        public int SpriteTileSize { get; protected set; }
        public abstract Rectangle GetCollisionBox();
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
