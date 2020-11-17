using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public class Projectile : AnimateObject
    {
        public readonly WeaponProperties WeaponType;
        public readonly Weapon Weapon;
        public Vector2 StartingCoordinates;
        public int CurrentFrameIndex = 0;
        public int CurrentFrame => CurrentFrameIndex * WeaponType.frameSkip;
        internal int LiveFinalFrame => (WeaponType.liveFrames * WeaponType.frameSkip) - 1;
        internal int DeathFinalFrame => (WeaponType.deathFrames * WeaponType.frameSkip);
        internal bool LiveAnimationCompleted => CurrentFrameIndex >= LiveFinalFrame;
        public bool DeathAnimationCompleted => CurrentFrameIndex >= DeathFinalFrame;
        public bool Dead { get; private set; }
        public bool AtMaxRange => Math.Abs((StartingCoordinates - Position).Length()) > WeaponType.range;
        public new Vector2 Direction;
        public Vector2 velocity;

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
                if ( AtMaxRange )
                {
                    Dead = true;
                    if ( Weapon.WeaponExplosionSounds.ContainsKey(WeaponType.weaponType) )
                        Weapon.WeaponExplosionSounds[WeaponType.weaponType].Play(0.5f * soundLevel, 0, 0);
                }
            }
            else if ( !DeathAnimationCompleted )
            {
                CurrentFrameIndex += 1;
                if ( !Dead )
                {
                    Position += velocity;
                    velocity *= WeaponType.acceleration;
                }
            }
        }

        public Point DrawCoordinates => new Point((int)(CurrentFrameIndex / WeaponType.frameSkip * SpriteTileSize * SpriteSize.Y), (int)((int)WeaponType.weaponType * SpriteTileSize * SpriteSize.Y));

        public override void Draw(SpriteBatch spriteBatch, GameState gameState)
        {
            Rectangle drawRectangle = new Rectangle(
                location: DrawCoordinates,
                size: (SpriteSize * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: Position - gameState.CameraLocation,
                sourceRectangle:
                drawRectangle,
                color: Color.White,
                rotation: (float)Math.Atan2(Direction.Y, Direction.X),
                origin: new Vector2(SpriteSize.X * SpriteTileSize / 2, SpriteSize.Y * SpriteTileSize / 2),
                scale: tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }

        public Projectile(Weapon weapon)
        {
            Vector2 gunLocation = weapon.Character.CurrentAnimation.GunLocation;
            gunLocation.X *= (int)weapon.Character.Direction;

            Vector2 gunDirection = weapon.Character.CurrentAnimation.GunDirection;
            gunDirection.X *= (int)weapon.Character.Direction;

            SpriteTileSize = 16;
            spriteTexture = weapon.texture;
            Position = weapon.Character.Position + (gunLocation * tileSize / SpriteTileSize);
            StartingCoordinates = Position;
            SpriteSize = weapon.WeaponProperties.SpriteSize;
            Direction = gunDirection;
            Weapon = weapon;
            WeaponType = new WeaponProperties(weapon.WeaponProperties.weaponType);
            velocity = Direction * WeaponType.weaponSpeed;
        }
    }
}
