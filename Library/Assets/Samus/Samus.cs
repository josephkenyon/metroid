using Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static Library.Domain.Enums;
using static Library.Domain.Constants;
using static Library.Assets.Samus.SamusAnimationSet;
using Library.Domain;

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
            Position = new Vector2(200f, 200f);
            Acceleration = new Vector2(1.1f, 2.0f);
            MaxVelocity = new Vector2(11f, 0.3f);
            Size = new Vector2(3, 4);
            animations = AnimationInitalizers.InitializeSamusAnimations(this, SamusAnimationProperties);
            SetCurrentAnimation(AnimationName.idle);
        }

        public override void Update(GamePadState gamePadState)
        {
            HandleMovementX(gamePadState.ThumbSticks.Left);
            HandleMovementY(gamePadState.ThumbSticks.Left);
            HandleButtons(gamePadState);
            CurrentAnimation.IncrementFrame();

            if (CurrentAnimation.AnimationType == AnimationType.runningType)
            {
                if (!AtMaxSpeedX())
                {
                    AccelerateX(gamePadState.ThumbSticks.Left.X);
                }
            }
            else if (CurrentAnimation.Name == AnimationName.jumpingSpinning)
            {
                if (!AtMaxSpeedX((Direction)Math.Sign(gamePadState.ThumbSticks.Left.X)))
                {
                    AccelerateX((Direction)Math.Sign(gamePadState.ThumbSticks.Left.X), gamePadState.ThumbSticks.Left.X);
                }
            }
            else
            {
                float constant = CurrentAnimation.Name == AnimationName.jumpingIdle? 0.6f : 1f;

                if (MovingLeft)
                {
                    CurrentVelocity.X = CurrentVelocity.X > Acceleration.X
                        ? 0
                        : CurrentVelocity.X + (Acceleration.X * constant);
                }
                else if (MovingRight)
                {
                    CurrentVelocity.X = CurrentVelocity.X < Acceleration.X
                        ? 0
                        : CurrentVelocity.X - (Acceleration.X * constant);
                }
            }

            if (CurrentAnimation.Name == AnimationName.jumpingIdle && CurrentAnimation.IsLooping == false)
            {
                AccelerateY();
            }
            else
            {
                if (Position.Y != floor)
                {
                    CurrentVelocity.Y += gravity;
                }
            }

            Position += CurrentVelocity;

            if (CurrentAnimation.AnimationType == AnimationType.jumpingType || CurrentAnimation.AnimationType == AnimationType.fallingType)
            {
                Position = new Vector2(Position.X + gamePadState.ThumbSticks.Left.X * 6f, Position.Y);
            }

            if (Position.Y >= floor)
            {
                if (NumJumps < 2)
                {
                    NumJumps = 2;
                    OverrideCurrentAnimation(AnimationName.landing);
                }
                Position = new Vector2(Position.X, floor);
                CurrentVelocity.Y = 0;
            }

            if (CurrentVelocity.Y > 0)
            {
                SetCurrentAnimation(AnimationName.falling);
            }

        }


        public override void HandleMovementX(Vector2 directionalInput)
        {
            if (IsGrounded && IsNotCrouching)
            {
                if (directionalInput.X == 0)
                {
                    SetCurrentAnimation(AnimationName.idle);
                }
                else if (Math.Sign((int)Direction) != Math.Sign(directionalInput.X))
                {
                    Turn();
                }
                else
                {
                    SetCurrentAnimation(AnimationName.running);
                }
            }
        }

        public override void HandleMovementY(Vector2 directionalInput)
        {
            if (directionalInput.Y < -.7f && IsGrounded)
            {
                SetCurrentAnimation(AnimationName.crouchingIdle);
            }
            else if (directionalInput.Y > 0 && CurrentAnimation.AnimationType == AnimationType.crouchingType && IsGrounded)
            {
                SetCurrentAnimation(AnimationName.standingUp);
            }
        }

        public override void HandleButtons(GamePadState gamePadState)
        {
            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                Jump();
            }
        }
    }
}
