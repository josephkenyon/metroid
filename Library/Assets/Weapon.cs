using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public class Weapon
    {
        public WeaponProperties WeaponProperties { get; private set; }

        public Texture2D texture;
        public Character Character;
        public int? ammo;
        public int lastFired = 0;
        public readonly PlayerIndex? playerIndex;
        public bool CanFire => lastFired >= WeaponProperties.weaponCooldown;

        public readonly SoundEffect WeaponCollisionSound;
        public readonly SortedDictionary<WeaponType, SoundEffect> WeaponFireSounds;
        public readonly SortedDictionary<WeaponType, SoundEffect> WeaponExplosionSounds;

        public List<Projectile> projectiles;

        public Weapon(WeaponType weaponType, Character Character, Texture2D texture, SortedDictionary<WeaponType, SoundEffect> WeaponFireSounds, SortedDictionary<WeaponType, SoundEffect> WeaponExplosionSounds, SoundEffect WeaponCollisionSound, int? ammo = null, PlayerIndex? playerIndex = null)
        {
            this.texture = texture;
            this.ammo = ammo;
            this.Character = Character;
            this.playerIndex = playerIndex;
            WeaponProperties = new WeaponProperties(weaponType);
            this.WeaponCollisionSound = WeaponCollisionSound;
            this.WeaponFireSounds = WeaponFireSounds;
            this.WeaponExplosionSounds = WeaponExplosionSounds;
            projectiles = new List<Projectile>();
        }

        public void Update(GameProperties gameState)
        {
            lastFired++;
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameState);
            }
            projectiles.RemoveAll(a => a.DeathAnimationCompleted);
        }

        public void AddAmmo(int howMuch)
        {
            if (ammo == null)
                throw new System.Exception("This gun doesn't have ammo.");
            else
            {
                ammo += howMuch;
            }
        }

        public void Fire()
        {
            Character.characterStats.shotsFired++;
            if ((ammo == null || ammo > 0))
            {
                projectiles.Add(new Projectile(this));
                WeaponFireSounds[WeaponProperties.weaponType]?.Play(0.5f * soundLevel, 0, 0);
                lastFired = 0;
                if (ammo != null)
                {
                    ammo--;
                }
            }
            else {
                Character.characterSounds.emptyGunSound.Play(0.5f * soundLevel, 0, 0);
                lastFired = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameProperties gameState)
        {
            foreach (Projectile projectile in projectiles)
            {
                projectile.Draw(spriteBatch, gameState);
            }
        }
    }
}
