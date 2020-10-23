using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Enums;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public abstract class AnimateObject : GameObject
    {
        protected Texture2D spriteTexture;
        protected Vector2 Acceleration;
        protected Vector2 MaxVelocity;
        protected Vector2 CurrentVelocity;

        protected Animation animation;
        public Vector2 GetCurrentVelocity => CurrentVelocity;
        public Direction Direction { get; protected set; }

        public Vector2 SpriteSize { get; protected set; }
        public bool MovingLeft => CurrentVelocity.X < 0;
        public bool MovingRight => CurrentVelocity.X > 0;

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRectangle = new Rectangle(
                location: animation.GetDrawCoordinates(Direction).ToPoint(),
                size: (SpriteSize * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: Position,
                sourceRectangle:
                drawRectangle,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
    }
}
