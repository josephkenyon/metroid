using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Constants;

namespace Library.Assets
{
    internal class TerrainBlock : GameObject
    {
        private readonly Texture2D texture;
        private readonly Vector2 spriteLocation;
        private readonly Vector2 Size;
        private readonly int spriteTileSize;
        public readonly bool impenetrable;

        public TerrainBlock(Texture2D texture, Vector2 spriteLocation, Vector2 Size, Vector2 Position, int spriteTileSize, bool impenetrable)
        {
            this.texture = texture;
            this.spriteLocation = spriteLocation;
            this.Size = Size;
            this.Position = Position;
            this.spriteTileSize = spriteTileSize;
            this.impenetrable = impenetrable;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRectangle = new Rectangle(
                location: spriteLocation.ToPoint(),
                size: Size.ToPoint()
            );
            spriteBatch.Draw(
                texture: texture,
                position: Position,
                sourceRectangle:
                drawRectangle,
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: tileSize / spriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
    }
}
