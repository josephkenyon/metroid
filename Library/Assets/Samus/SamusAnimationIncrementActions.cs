using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets.Samus
{
    public static class SamusAnimationIncrementActions
    {
        public static void IdleIncrement(Animation animation, Character character, GamePadState gamePadState)
        {
            float inputX = gamePadState.ThumbSticks.Right.X;
            float inputY = gamePadState.ThumbSticks.Right.Y;

            if ( Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.3f )
            {
                character.SetCurrentAnimation(AnimationName.running);
                return;
            }

            if ( gamePadState.ThumbSticks.Left.Y < -.7f && character.IsGrounded )
            {
                character.SetCurrentAnimation(AnimationName.crouchingIdle);
                return;
            }

            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingStandingDiagonalDown);
                }
                else if ( inputY > 0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingStandingDiagonalUp);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.aimingStandingStraight);
                }
            }
            else if ( Math.Abs(inputX) < 0.25 && inputY > 0.5 )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingStraightUp);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.idle);
            }
        }

        public static void RunningIncrement(Animation animation, Character character, GamePadState gamePadState)
        {
            float inputX = gamePadState.ThumbSticks.Right.X;
            float inputY = gamePadState.ThumbSticks.Right.Y;

            int newFrame = animation.AnimationType == AnimationType.runningType ? animation.CurrentFrame : 0;

            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.runningAimingDiagonalDown, newFrame);
                }
                else if ( inputY > 0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.runningAimingDiagonalUp, newFrame);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.runningAimingCenter, newFrame);
                }
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.running, newFrame);
            }

            if ( !character.AtMaxSpeedX() )
            {
                character.AccelerateX(gamePadState.ThumbSticks.Left.X);
            }
        }

        public static void JumpingIdleIncrement(Animation animation, Character character, GamePadState gamePadState)
        {
            character.InfluencePosition(new Vector2(gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));

            if ( animation.IsLooping == false )
            {
                character.AccelerateY();
            }

            if ( gamePadState.Buttons.A == ButtonState.Pressed && character.GetCurrentVelocity.Y > -0.2f * tileSize )
            {
                character.SetCurrentAnimation(AnimationName.jumpingSpinning);
            }
        }

        public static void JumpingSpinningIncrement(Animation animation, Character character, GamePadState gamePadState)
        {
            character.InfluencePosition(new Vector2(gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));

            if ( !character.AtMaxSpeedX((Direction)Math.Sign(gamePadState.ThumbSticks.Left.X)) )
            {
                character.AccelerateX((Direction)Math.Sign(gamePadState.ThumbSticks.Left.X), gamePadState.ThumbSticks.Left.X);
            }
        }

        public static void FallingIncrement(Animation animation, Character character, GamePadState gamePadState)
        {
            character.InfluencePosition(new Vector2(gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));
        }

        public static void CrouchingIncrement(Animation animation, Character character, GamePadState gamePadState)
        {
            float inputX = gamePadState.ThumbSticks.Right.X;
            float inputY = gamePadState.ThumbSticks.Right.Y;

            if ( gamePadState.ThumbSticks.Left.Y > 0 )
            {
                character.SetCurrentAnimation(AnimationName.standingUp);
            }

            if ( Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.7f )
            {
                if ( Math.Sign(gamePadState.ThumbSticks.Left.X) != (int)character.Direction )
                {
                    character.SetCurrentAnimation(AnimationName.turning);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.running);
                }
                return;
            }

            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingDiagonalDown);
                }
                else if ( inputY > 0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingDiagonalUp);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingStraight);
                }
            }
            else if ( Math.Abs(inputX) < 0.25 && inputY > 0.5 )
            {
                character.SetCurrentAnimation(AnimationName.aimingCrouchingStraightUp);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.crouchingIdle);
            }
        }
    }
}
