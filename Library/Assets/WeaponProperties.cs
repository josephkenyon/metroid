using Microsoft.Xna.Framework;
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
        public int frameSkip;
        public float weaponPower;
        public float weaponSpeed;
        public Vector2 collisionBoxSize;

        public WeaponProperties(WeaponType weaponType)
        {
            this.weaponType = weaponType;

            switch ( weaponType )
            {
                case WeaponType.Charge:
                    weaponPower = 7f;
                    weaponCooldown = 10;
                    weaponSpeed = 0.5f * tileSize;
                    liveLoopIndex = 1;
                    liveFrames = 2;
                    deathFrames = 6;
                    frameSkip = 3;
                    break;
                case WeaponType.Missile:
                    weaponPower = 18f;
                    weaponCooldown = 30;
                    weaponSpeed = 0.4f * tileSize;
                    liveLoopIndex = 0;
                    liveFrames = 1;
                    deathFrames = 10;
                    frameSkip = 3;
                    break;
            }
        }
    }
}
