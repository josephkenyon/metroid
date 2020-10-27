using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.Assets.Samus.SamusAnimationSet;
using static Library.Assets.Samus.SamusCollisionBoxSet;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets.Samus
{
    public class Samus : Character
    {
        public override int MaxHealth => 100;

        public Samus(Texture2D spriteTexture)
        {
            this.spriteTexture = spriteTexture;
            SpriteNumber = new Vector2(10, 14);
            SpriteTileSize = 16;
            Direction = Direction.left;
            Position = new Vector2(tileSize * 3, tileSize * 3);
            Acceleration = new Vector2(0.025f * tileSize, 0.03f * tileSize);
            MaxVelocity = new Vector2(0.18f * tileSize, 0.005f * tileSize);
            SpriteSize = new Vector2(3, 4);
            animations = AnimationInitalizers.InitializeSamusAnimations(this, SamusAnimationProperties);
            SetCurrentAnimation(AnimationName.idle);
        }

        public override void Update(GameState gameState, GamePadState gamePadState)
        {
            this.gameState = gameState;
            this.gamePadState = gamePadState;
            float currentFloor = GetFloor();
            float currentCeiling = GetCeiling();
            float currentRightWall = GetRightWall();
            float currentLeftWall = GetLeftWall();

            HandleButtons(gamePadState);
            CurrentAnimation.Increment(gamePadState);

            Decelerate();
            ApplyGravity(GetFloor());

            Position += CurrentVelocity;

            CheckCollisions(currentFloor, currentCeiling, currentRightWall, currentLeftWall);

            if ( CurrentVelocity.Y > 0 && CurrentAnimation.Name != AnimationName.morphBall )
            {
                SetCurrentAnimation(AnimationName.falling);
            }

        }

        public void Decelerate()
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

                if ( MovingLeft )
                {
                    CurrentVelocity.X = CurrentVelocity.X > Acceleration.X
                        ? 0
                        : CurrentVelocity.X + (Acceleration.X * constant);
                }
                else if ( MovingRight )
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
