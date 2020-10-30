using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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

        public List<Projectile> projectiles;

        public Weapon(WeaponType weaponType, Character Character, Texture2D texture, int? ammo = null)
        {
            this.texture = texture;
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
            }
            projectiles.RemoveAll(a => a.DeathAnimationCompleted);
        }

        public void Fire()
        {
            Vector2 Direction = Vector2.One;
            projectiles.Add(new Projectile(this, Direction));
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
