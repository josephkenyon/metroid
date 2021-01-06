using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using static Library.Domain.Constants;
using static Library.Domain.Enums;
namespace Library.Assets
{
    public class WeaponProperties
    {
        public WeaponType weaponType;
        public int weaponCooldown;
        public int liveLoopIndex;
        public int liveFrames;
        public int deathFrames;
        public int? damageFrame;
        public int frameSkip;
        public float weaponPower;
        public float acceleration;
        public float weaponSpeed;
        public int? activeProjectilesAllowed;
        public float range;
        public int spriteTileSize = 16;
        public Vector2 SpriteSize = new Vector2(2, 2);
        public Point collisionBoxSize;
        public SoundEffect soundEffect;

        public WeaponProperties(WeaponType weaponType)
        {
            this.weaponType = weaponType;

            switch ( weaponType )
            {
                case WeaponType.Charge:
                    weaponPower = 2.5f;
                    weaponCooldown = 10;
                    weaponSpeed = 0.7f;
                    liveLoopIndex = 1;
                    liveFrames = 2;
                    range = 2000f;
                    acceleration = 1f;
                    deathFrames = 8;
                    frameSkip = 3;
                    damageFrame = null;
                    activeProjectilesAllowed = null;
                    collisionBoxSize = new Point(8, 6);
                    break;
                case WeaponType.Rocket:
                    weaponPower = 20f;
                    weaponCooldown = 30;
                    weaponSpeed = 0.15f;
                    acceleration = 1.05f;
                    liveLoopIndex = 0;
                    liveFrames = 1;
                    range = 2000f;
                    deathFrames = 11;
                    frameSkip = 3;
                    damageFrame = null;
                    activeProjectilesAllowed = null;
                    collisionBoxSize = new Point(16, 8);
                    break;
                case WeaponType.Bomb:
                    weaponPower = 8f;
                    weaponCooldown = 15;
                    weaponSpeed = 0f;
                    acceleration = 1.00f;
                    liveLoopIndex = 0;
                    liveFrames = 4;
                    range = 60f;
                    deathFrames = 11;
                    frameSkip = 3;
                    damageFrame = 7;
                    activeProjectilesAllowed = 4;
                    collisionBoxSize = new Point(48, 48);
                    break;
            }
        }
    }
}
