using Library.State;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public class Weapon
    {
        public readonly WeaponProperties WeaponProperties;

        public Texture2D texture;
        public Character Character;
        public int? ammo;
        public int lastFired = 0;
        public bool CanFire => lastFired >= WeaponProperties.weaponCooldown && (ammo == null || ammo > 0);

        public readonly SortedDictionary<WeaponType, SoundEffect> WeaponFireSounds;
        public readonly SortedDictionary<WeaponType, SoundEffect> WeaponExplosionSounds;

        public List<Projectile> projectiles;

        public Weapon(WeaponType weaponType, Character Character, Texture2D texture, SortedDictionary<WeaponType, SoundEffect> WeaponFireSounds, SortedDictionary<WeaponType, SoundEffect> WeaponExplosionSounds, int? ammo = null)
        {
            this.texture = texture;
            this.ammo = ammo;
            this.Character = Character;
            WeaponProperties = new WeaponProperties(weaponType);
            this.WeaponFireSounds = WeaponFireSounds;
            this.WeaponExplosionSounds = WeaponExplosionSounds;
            projectiles = new List<Projectile>();
        }

        public void Update()
        {
            lastFired++;
            foreach ( Projectile projectile in projectiles )
            {
                projectile.Update();
            }
            projectiles.RemoveAll(a => a.DeathAnimationCompleted);
        }

        public void Fire()
        {
            projectiles.Add(new Projectile(this));
            WeaponFireSounds[WeaponProperties.weaponType]?.Play(0.5f * soundLevel, 0, 0);
            lastFired = 0;
            if ( ammo != null )
            {
                ammo--;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameState gameState)
        {
            foreach ( Projectile projectile in projectiles )
            {
                projectile.Draw(spriteBatch, gameState);
            }
        }
    }
}
