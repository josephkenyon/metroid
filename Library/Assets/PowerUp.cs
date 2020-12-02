using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public class PowerUp : GameObject
    {
        public PowerUpSpawner PowerUpSpawner;
        public PowerUpProperties PowerUpProperties;
        private int bounce = 0;
        private bool bounceIncreasing = true;

        public PowerUp(PowerUpSpawner powerUpSpawner)
        {
            PowerUpSpawner = powerUpSpawner;
            PowerUpProperties = new PowerUpProperties(powerUpSpawner.PowerUpType);
            SpriteTileSize = PowerUpProperties.spriteTileSize;
            Position = powerUpSpawner.Location.ToVector2() * tileSize;
        }

        public void Update()
        {
            if (bounce == 0) {
                bounceIncreasing = true;
            } else if (bounce == 10) {
                bounceIncreasing = false;
            }

            bounce = bounceIncreasing ? bounce + 1 : bounce - 1;
        }

        public override void Draw(SpriteBatch spriteBatch, GameProperties gameState)
        {
            Rectangle drawRectangle = new Rectangle(
                location: (PowerUpProperties.DrawCoordinates.ToVector2() * PowerUpProperties.spriteTileSize).ToPoint(),
                size: (PowerUpProperties.SpriteSize * PowerUpProperties.spriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: PowerUpSpawner.GetTexture(),
                position: Position - gameState.CameraLocation - new Vector2(0f, bounce / 2),
                sourceRectangle:
                drawRectangle,
                color: gameState.CurrentLevel.TintColor,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }

        public override Rectangle GetCollisionBox() => new Rectangle(Position.ToPoint(), (Vector2.One * tileSize).ToPoint());
    }
}
