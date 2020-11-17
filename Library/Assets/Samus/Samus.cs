using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public List<Weapon> Weapons { get; set; }
        public override int MaxHealth => 100;
        public bool Dead => CurrentAnimation.AnimationType == AnimationType.deadType;

        public Samus(Texture2D spriteTexture, Texture2D weaponTexture, SortedDictionary<WeaponType, SoundEffect> WeaponFireSounds, SortedDictionary<WeaponType, SoundEffect> WeaponExplosionSounds)
        {
            this.spriteTexture = spriteTexture;
            Weapons = new List<Weapon>()
            {
                new Weapon(WeaponType.Charge, this, weaponTexture, WeaponFireSounds, WeaponExplosionSounds),
                new Weapon(WeaponType.Rocket, this, weaponTexture, WeaponFireSounds, WeaponExplosionSounds)

            };
            SpriteNumber = new Vector2(10, 14);
            SpriteTileSize = 16;
            Direction = Direction.left;
            Position = new Vector2(tileSize * 3, tileSize * 3);
            Acceleration = new Vector2(0.025f * tileSize, 0.032f * tileSize);
            MaxVelocity = new Vector2(0.18f * tileSize, 0.005f * tileSize);
            SpriteSize = new Vector2(3, 4);
            animations = AnimationInitalizers.InitializeSamusAnimations(this, SamusAnimationProperties);
            SetCurrentAnimation(AnimationName.idle);
        }

        public override void Update(GameState gameState, GamePadState gamePadState)
        {
            if ( Dead ) return;
            this.gameState = gameState;
            this.gamePadState = gamePadState;
            float currentFloor = GetFloor();
            float currentCeiling = GetCeiling();
            float currentRightWall = GetRightWall();
            float currentLeftWall = GetLeftWall();

            CurrentAnimation.Increment(gamePadState);
            HandleButtons(gamePadState);

            Decelerate(gamePadState);
            ApplyGravity(GetFloor());

            Position += CurrentVelocity;

            CheckCollisions(currentFloor, currentCeiling, currentRightWall, currentLeftWall);

            if ( CurrentVelocity.Y > 0 && CurrentAnimation.Name != AnimationName.morphBall && CurrentAnimation.Name != AnimationName.turning && CurrentAnimation.AnimationType != AnimationType.jumpingShootingType )
            {
                SetCurrentAnimation(AnimationName.falling);
            }

            foreach ( Weapon weapon in Weapons )
            {
                weapon.Update();
            }

        }

        public void Decelerate(GamePadState gamePadState)
        {
            if ( CurrentAnimation.AnimationType != AnimationType.runningType )
            {
                float constant;
                switch ( CurrentAnimation.Name )
                {
                    case AnimationName.jumpingIdle:
                        constant = 0.6f;
                        break;
                    case AnimationName.jumpingSpinning:
                        constant = 0.2f;
                        break;
                    default:
                        constant = 1f;
                        break;
                }

                if ( MovingLeft && !(CurrentAnimation.Name == AnimationName.morphBall && gamePadState.ThumbSticks.Left.X != 0) )
                {
                    CurrentVelocity.X = CurrentVelocity.X > Acceleration.X
                        ? 0
                        : CurrentVelocity.X + (Acceleration.X * constant);
                }
                else if ( MovingRight && !(CurrentAnimation.Name == AnimationName.morphBall && gamePadState.ThumbSticks.Left.X != 0) )
                {
                    CurrentVelocity.X = CurrentVelocity.X < Acceleration.X
                        ? 0
                        : CurrentVelocity.X - (Acceleration.X * constant);
                }
            }
        }

        public override void HandleButtons(GamePadState gamePadState)
        {
            if ( gamePadState.Buttons.A == ButtonState.Pressed && IsGrounded && NumJumps == 2 && Math.Abs(GetCeiling() - GetCollisionBox().Top) > tileSize / 2 )
            {
                SetCurrentAnimation(AnimationName.jumpingIdle);
            }

            if ( CurrentAnimation.Name.ToString().Contains("iming") )
            {
                if ( gamePadState.Triggers.Right == 1.0f && Weapons[1].CanFire )
                    Weapons[1].Fire();
                else if ( gamePadState.Triggers.Left == 1.0f && Weapons[0].CanFire )
                    Weapons[0].Fire();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameState gameState)
        {
            foreach ( Weapon weapon in Weapons )
            {
                weapon.Draw(spriteBatch, gameState);
            }

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
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
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
