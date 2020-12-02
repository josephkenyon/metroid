using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public abstract class GameObject
    {
        public Vector2 Position { get; set; }
        public Point CurrentQuadrant => (Position / tileSize).ToPoint();
        public int SpriteTileSize { get; set; }
        public abstract Rectangle GetCollisionBox();
        public abstract void Draw(SpriteBatch spriteBatch, GameProperties gameState);
    }
}
