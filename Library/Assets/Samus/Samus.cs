using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static Library.Assets.Samus.SamusAnimationSet;
using static Library.Assets.Samus.SamusCollisionBoxSet;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets.Samus
{
    public class Samus : Character
    {
        public PlayerIndex playerIndex;
        public override int MaxHealth => 100;
        public Texture2D whiteTexture;
        public bool Dead => CurrentAnimation.AnimationType == AnimationType.deadType;

        public Samus(PlayerIndex index, SamusColor color, Point spawnLocation, ContentManager Content, CharacterSounds characterSounds)
        {
            Texture2D weaponTexture = Content.Load<Texture2D>("Sprites\\weapons");
            Health = MaxHealth;
            playerIndex = index;
            this.characterSounds = characterSounds;

            spriteTexture = Content.Load<Texture2D>("Sprites\\Samus\\samus" + color.ToString());
            whiteTexture = Content.Load<Texture2D>("Sprites\\blankWhite");

            SortedDictionary<WeaponType, SoundEffect> WeaponFireSounds = new SortedDictionary<WeaponType, SoundEffect>
            {
                { WeaponType.Charge, Content.Load<SoundEffect>("Sound\\chargeFire") },
                { WeaponType.Rocket, Content.Load<SoundEffect>("Sound\\rocketFire") }
            };

            SortedDictionary<WeaponType, SoundEffect> WeaponExplosionSounds = new SortedDictionary<WeaponType, SoundEffect>
            {
                { WeaponType.Rocket, Content.Load<SoundEffect>("Sound\\rocketExplosion") },
            };

            SoundEffect WeaponCollisionSound = Content.Load<SoundEffect>("Sound\\rockHit");

            Weapons = new List<Weapon>()
            {
                new Weapon(WeaponType.Charge, this, weaponTexture, WeaponFireSounds, WeaponExplosionSounds, WeaponCollisionSound, playerIndex: index),
                new Weapon(WeaponType.Rocket, this, weaponTexture, WeaponFireSounds, WeaponExplosionSounds, WeaponCollisionSound, ammo: rocketAmmoSize, playerIndex: index)

            };

            SpriteNumber = new Vector2(10, 14);
            SpriteTileSize = 16;
            Direction = Direction.left;
            Position = spawnLocation.ToVector2() * tileSize;
            Acceleration = new Vector2(0.025f * tileSize, 0.032f * tileSize);
            MaxVelocity = new Vector2(0.18f * tileSize, 0.005f * tileSize);
            SpriteSize = new Vector2(3, 4);
            animations = AnimationInitalizers.InitializeSamusAnimations(this, SamusAnimationProperties);
            SetCurrentAnimation(AnimationName.idle);
        }

        public override void Update(GameProperties gameState, GamePadState gamePadState)
        {
            if (HitTimer > 0)
                HitTimer--;
            if (Dead) return;
            Health += 0.01f;
            this.gameState = gameState;
            this.gamePadState = gamePadState;
            float currentFloor = GetFloor();
            float currentCeiling = GetCeiling();
            float currentRightWall = GetRightWall();
            float currentLeftWall = GetLeftWall();

            CurrentAnimation.Increment();
            HandleButtons(gamePadState);

            Decelerate(gamePadState);
            ApplyGravity(currentFloor);

            Position += CurrentVelocity;

            CheckCollisions(currentFloor, currentCeiling, currentRightWall, currentLeftWall);

            if (CurrentVelocity.Y > 0 && CurrentAnimation.Name != AnimationName.morphBall && CurrentAnimation.AnimationType != AnimationType.crouchingType && CurrentAnimation.Name != AnimationName.turning && CurrentAnimation.AnimationType != AnimationType.jumpingShootingType)
                SetCurrentAnimation(AnimationName.falling);

            foreach (Weapon weapon in Weapons)
                weapon.Update(gameState);

        }

        public void Decelerate(GamePadState gamePadState)
        {
            if (CurrentAnimation.AnimationType != AnimationType.runningType)
            {
                var constant = CurrentAnimation.Name switch
                {
                    AnimationName.jumpingIdle => 0.6f,
                    AnimationName.jumpingSpinning => 0.2f,
                    _ => 1f,
                };
                constant *= gameState.CurrentLevel.Gravity / gravity;
                if (MovingLeft && !(CurrentAnimation.Name == AnimationName.morphBall && gamePadState.ThumbSticks.Left.X != 0))
                {
                    CurrentVelocity.X = CurrentVelocity.X > Acceleration.X
                        ? 0
                        : CurrentVelocity.X + (Acceleration.X * constant);
                }
                else if (MovingRight && !(CurrentAnimation.Name == AnimationName.morphBall && gamePadState.ThumbSticks.Left.X != 0))
                {
                    CurrentVelocity.X = CurrentVelocity.X < Acceleration.X
                        ? 0
                        : CurrentVelocity.X - (Acceleration.X * constant);
                }
            }
        }

        public override void HandleButtons(GamePadState gamePadState)
        {
            if (gamePadState.Buttons.A == ButtonState.Pressed && IsGrounded && NumJumps == 2 && Math.Abs(GetCeiling() - GetCollisionBox().Top) > tileSize / 2)
                SetCurrentAnimation(AnimationName.jumpingIdle);

            if (CurrentAnimation.Name.ToString().Contains("iming"))
                if (gamePadState.Triggers.Right == 1.0f && Weapons[1].CanFire)
                    Weapons[1].Fire();
                else if (gamePadState.Triggers.Left == 1.0f && Weapons[0].CanFire)
                    Weapons[0].Fire();
        }

        public override void Draw(SpriteBatch spriteBatch, GameProperties gameState)
        {
            Vector2 position = new Vector2(
                Position.X - (SpriteSize.X * SpriteTileSize * tileSize / SpriteTileSize / 2),
                Position.Y - (SpriteSize.Y * SpriteTileSize * tileSize / SpriteTileSize)
            );
            Rectangle drawRectangle = new Rectangle(
                location: CurrentAnimation.GetDrawCoordinates(Direction).ToPoint(),
                size: (SpriteSize * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: position - gameState.CameraLocation,
                sourceRectangle:
                drawRectangle,
                color: new Color(gameState.CurrentLevel.TintColor.R,
                            (int)((255 - HitTimer * 255f / 30f) * gameState.CurrentLevel.TintColor.G / 255f),
                            (int)(gameState.CurrentLevel.TintColor.B - HitTimer * gameState.CurrentLevel.TintColor.B / 30f)),
                rotation: 0f,
                origin: Vector2.Zero,
                scale: tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            var collisionBox = GetCollisionBox();
            var position = collisionBox.Location;
            position.Y = (int)(Position.Y - tileSize * 3.1);
            var size = new Vector2(tileSize * 1.4f * (float)Health / 100f, tileSize / 6);
            var blackSize = new Vector2(tileSize * 1.4f - size.X, tileSize / 6);
            position.X -= (int)(tileSize * 0.3f);

            spriteBatch.Draw(whiteTexture, new Rectangle(position, size.ToPoint()), Color.Red);
            spriteBatch.Draw(whiteTexture, new Rectangle(new Point(position.X + (int)size.X, position.Y), blackSize.ToPoint()), Color.Black);
        }

        public void DrawWeapons(SpriteBatch spriteBatch, GameProperties gameState)
        {
            foreach (Weapon weapon in Weapons)
                weapon.Draw(spriteBatch, gameState);
        }

        public override Rectangle GetCollisionBox()
        {
            Rectangle collisionBox = SamusCollisionBoxes.ContainsKey(CurrentAnimation.Name)
                ? SamusCollisionBoxes[CurrentAnimation.Name]
                : DefaultCollisionBox;

            collisionBox.Width = collisionBox.Width * tileSize / SpriteTileSize;
            collisionBox.Height = collisionBox.Height * tileSize / SpriteTileSize;
            collisionBox.X = (int)(Position.X - (collisionBox.Width / 2) + (collisionBox.X * tileSize / SpriteTileSize));
            collisionBox.Y = (int)(Position.Y - collisionBox.Height + (collisionBox.Y * tileSize / SpriteTileSize));

            collisionBox.Y = (int)(collisionBox.Y + (collisionBox.Height * 0.03));
            collisionBox.Height = (int)(collisionBox.Height * 0.9);

            return collisionBox;
        }
    }
}
