using Microsoft.Xna.Framework;
using static Library.Domain.Enums;

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

        public Projectile(WeaponType weaponType, Vector2 Direction)
        {
            this.Direction = Direction;
            WeaponType = new WeaponProperties(weaponType);
        }
    }
}
