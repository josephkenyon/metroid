using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using static Library.Domain.Constants;
using static Library.Domain.Enums;
namespace Library.Assets
{
    public class PowerUpProperties
    {
        public PowerUpType PowerUpType;
        public int spriteTileSize = 16;
        public Vector2 SpriteSize = new Vector2(1, 1);
        public SoundEffect soundEffect;
        public Point DrawCoordinates;
        public Action<Character> ExecutePickedUpAction { get; private set; }

        public PowerUpProperties(PowerUpType powerUpType)
        {
            PowerUpType = powerUpType;

            switch (powerUpType)
            {
                case PowerUpType.Health:
                    ExecutePickedUpAction = HealthPickUp;
                    DrawCoordinates = new Point(0, 0);
                    break;
                case PowerUpType.RocketAmmo:
                    ExecutePickedUpAction = RocketAmmoPickUp;
                    DrawCoordinates = new Point(1, 0);
                    break;
            }
        }

        public static void HealthPickUp(Character character)
        {
            var previousHealth = character.Health;
            int healAmount = 35;
            if (character.Health + healAmount > character.MaxHealth)
            {
                character.Heal(character.MaxHealth - character.Health);
            }
            else
                character.Heal(healAmount);

            character.characterStats.hitPointsHealed += character.Health - previousHealth;

            character.characterSounds.healthUpSound.Play(1.2f * character.gameState.soundLevel, 0, 0);
        }

        public static void RocketAmmoPickUp(Character character)
        {
            character.Weapons.Find(w => w.WeaponProperties.weaponType == WeaponType.Rocket).ammo = rocketAmmoSize;
            character.characterSounds.ammoCollectSound.Play(1.2f * character.gameState.soundLevel, 0, 0);
        }
    }
}
