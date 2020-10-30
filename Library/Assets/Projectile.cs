using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Library.Domain.Enums;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public class Projectile : AnimateObject
    {
        public readonly WeaponProperties WeaponType;
        public int CurrentFrameIndex = 0;
        public int CurrentFrame => CurrentFrameIndex * WeaponType.frameSkip;
        internal int LiveFinalFrame => (WeaponType.liveFrames * WeaponType.frameSkip) - 1;
        internal int DeathFinalFrame => (WeaponType.deathFrames * WeaponType.frameSkip);
        internal bool LiveAnimationCompleted => CurrentFrameIndex >= WeaponType.liveFrames;
        public bool DeathAnimationCompleted => CurrentFrameIndex >= WeaponType.deathFrames;
        public bool Dead { get; private set; }
        public new Vector2 Direction;

        public override Rectangle GetCollisionBox()
        {
            return new Rectangle(
                (int)(Position.X - WeaponType.collisionBoxSize.X / 2),
                (int)(Position.Y - WeaponType.collisionBoxSize.Y / 2),
                (int)WeaponType.collisionBoxSize.X,
                (int)WeaponType.collisionBoxSize.Y
            );
        }

        public void Update()
        {
            if ( !Dead && LiveAnimationCompleted )
            {
                CurrentFrameIndex = WeaponType.liveLoopIndex * WeaponType.frameSkip;
                Dead = true;
            }
            else if ( !DeathAnimationCompleted )
            {
                CurrentFrameIndex += 1;
                Position += Direction * WeaponType.weaponSpeed;
            }
        }

        public Point DrawCoordinates => new Point((int)(CurrentFrameIndex / WeaponType.frameSkip * SpriteTileSize * SpriteSize.Y), (int)((int)WeaponType.weaponType * SpriteTileSize * SpriteSize.Y));

        public override void Draw(SpriteBatch spriteBatch, GameState gameState)
        {
            Vector2 position = new Vector2(
                Position.X - (SpriteSize.X * tileSize / 2),
                Position.Y - (SpriteSize.Y * tileSize / 2)
                );
            Rectangle drawRectangle = new Rectangle(
                location: DrawCoordinates,
                size: (SpriteSize * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: position - gameState.CameraLocation,
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

        public Projectile(Weapon weapon, Vector2 Direction)
        {
            spriteTexture = weapon.texture;
            Position = weapon.Character.Position;
            SpriteSize = weapon.WeaponProperties.SpriteSize;
            this.Direction = Direction;
            SpriteTileSize = 16;
            WeaponType = new WeaponProperties(weapon.WeaponProperties.weaponType);
        }
    }
}
