using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
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
        public float acceleration;
        public float weaponSpeed;
        public float range;
        public int spriteTileSize = 16;
        public Vector2 SpriteSize = new Vector2(2, 2);
        public Vector2 collisionBoxSize;
        public SoundEffect soundEffect;

        public WeaponProperties(WeaponType weaponType)
        {
            this.weaponType = weaponType;

            switch ( weaponType )
            {
                case WeaponType.Charge:
                    weaponPower = 7f;
                    weaponCooldown = 10;
                    weaponSpeed = 0.7f * tileSize;
                    liveLoopIndex = 1;
                    liveFrames = 2;
                    range = 900f;
                    acceleration = 1f;
                    deathFrames = 6;
                    frameSkip = 3;
                    break;
                case WeaponType.Rocket:
                    weaponPower = 18f;
                    weaponCooldown = 30;
                    weaponSpeed = 0.15f * tileSize;
                    acceleration = 1.05f;
                    liveLoopIndex = 0;
                    liveFrames = 1;
                    range = 1200f;
                    deathFrames = 10;
                    frameSkip = 3;
                    break;
            }
        }
    }
}
