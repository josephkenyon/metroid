using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public class Weapon
    {
        public readonly WeaponProperties WeaponProperties;

        public Character Character;
        public int? ammo;
        public int lastFired = 0;
        public bool CanFire => lastFired >= WeaponProperties.weaponCooldown && (ammo == null || ammo > 0);

        public List<Projectile> projectiles;

        public Weapon(WeaponType weaponType, Character Character, int? ammo = null)
        {
            this.ammo = ammo;
            this.Character = Character;
            WeaponProperties = new WeaponProperties(weaponType);
            projectiles = new List<Projectile>();
        }

        public void Update()
        {
            lastFired++;
            foreach ( Projectile projectile in projectiles )
            {
                projectile.Update();
                if ( projectile.DeathAnimationCompleted )
                {
                    projectiles.Remove(projectile);
                }
            }
        }

        public void Fire()
        {
            Vector2 Direction = Vector2.One;
            projectiles.Add(new Projectile(WeaponProperties.weaponType, Direction));
            lastFired++;
            if (ammo != null )
            {
                ammo--;
            }
        }
    }
}
