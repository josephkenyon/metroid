using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public class TerrainBlock : GameObject
    {
        private readonly Texture2D texture;
        private readonly Vector2 SpriteLocation;
        private readonly Vector2 Size;
        public readonly bool Impenetrable;

        public TerrainBlock(Texture2D texture, Vector2 SpriteLocation, Vector2 Position, int SpriteTileSize, bool Impenetrable)
        {
            Size = Vector2.One * SpriteTileSize * tileSize;
            this.texture = texture;
            this.SpriteLocation = SpriteLocation;
            this.Position = Position;
            this.SpriteTileSize = SpriteTileSize;
            this.Impenetrable = Impenetrable;
        }

        public override void Draw(SpriteBatch spriteBatch, GameState gameState)
        {
            Rectangle drawRectangle = new Rectangle(
                location: (SpriteLocation * SpriteTileSize).ToPoint(),
                size: (Vector2.One * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: texture,
                position: Position - gameState.CameraLocation,
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

        public override Rectangle GetCollisionBox()
        {
            throw new System.NotImplementedException();
        }
    }
}
