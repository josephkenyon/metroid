using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public class TerrainBlock : GameObject
    {
        private Texture2D texture;
        public Vector2 SpriteLocation;
        public new Point CurrentQuadrant(GameProperties gameState) => Position.ToPoint();
        public int tileSize;
        public bool Impenetrable;
        public bool Background;

        public TerrainBlock()
        {

        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }
        public Texture2D Texture
            => texture;

        public TerrainBlock(Texture2D texture, Vector2 SpriteLocation, Vector2 Position, int SpriteTileSize, bool Impenetrable, int tileSize, bool Background = false)
        {
            this.tileSize = tileSize;
            this.texture = texture;
            this.SpriteLocation = SpriteLocation;
            this.SpriteTileSize = SpriteTileSize;
            this.Impenetrable = Impenetrable;
            this.Background = Background;
            this.Position = Position;
        }

        public override void Draw(SpriteBatch spriteBatch, GameProperties gameState)
        {
            Rectangle drawRectangle = new Rectangle(
                location: (SpriteLocation * SpriteTileSize).ToPoint(),
                size: (Vector2.One * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: texture,
                position: (Position * gameState.tileSize) - gameState.CameraLocation,
                sourceRectangle: drawRectangle,
                color: Background ? gameState.CurrentLevel.TintColor * 0.65f : gameState.CurrentLevel.TintColor,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: gameState.tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }

        public override Rectangle GetCollisionBox(GameProperties gameState)
        {
            return new Rectangle((Position * gameState.tileSize).ToPoint(), (Vector2.One * gameState.tileSize).ToPoint());
        }
    }
}
