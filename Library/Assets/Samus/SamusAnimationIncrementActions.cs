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
        public static void IdleIncrement(Animation animation, Character character)
        {
            float inputX = character.gamePadState.ThumbSticks.Right.X;
            float inputY = character.gamePadState.ThumbSticks.Right.Y;

            if ( Math.Abs(character.gamePadState.ThumbSticks.Left.X) > 0.3f )
            {
                character.SetCurrentAnimation(AnimationName.running);
                return;
            }

            if ( character.gamePadState.ThumbSticks.Left.Y < -.7f && character.IsGrounded )
            {
                character.SetCurrentAnimation(AnimationName.crouchingIdle);
                return;
            }

            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingStandingDiagonalDown, finalFrame: true);
                }
                else if ( inputY > 0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingStandingDiagonalUp, finalFrame: true);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.aimingStandingStraight, finalFrame: true);
                }
            }
            else if ( Math.Abs(inputX) < 0.25 && inputY > 0.5 )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingStraightUp, finalFrame: true);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.idle);
            }
        }

        public static void RunningIncrement(Animation animation, Character character)
        {
            float directionalInputX = character.gamePadState.ThumbSticks.Left.X;

            if ( directionalInputX == 0 )
            {
                // if not moving set animation to idle
                character.SetCurrentAnimation(AnimationName.idle);
                return;
            }
            else if ( Math.Sign((int)character.Direction) != Math.Sign(directionalInputX) )
            {
                // if input is not the direction currently running in, turn
                character.SetCurrentAnimation(AnimationName.turning);
                character.CurrentAnimation.Direction = (Direction)((int)character.Direction * -1);
            }

            int newFrame = animation.AnimationType == AnimationType.runningType ? animation.CurrentFrame : 0;

            // allow character to aim
            if ( Math.Abs(character.gamePadState.ThumbSticks.Right.X) > 0.3 )
            {
                if ( character.gamePadState.ThumbSticks.Right.Y < -0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.runningAimingDiagonalDown, newFrame);
                }
                else if ( character.gamePadState.ThumbSticks.Right.Y > 0.1 )
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

            // if not at max speed, accelerate
            if ( !character.AtMaxSpeedX() )
            {
                character.AccelerateX(character.gamePadState.ThumbSticks.Left.X);
            }
        }

        public static void JumpingIdleIncrement(Animation animation, Character character)
        {
            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));

            if ( animation.IsLooping == false )
            {
                character.AccelerateY();
            }

            if ( character.gamePadState.Buttons.A == ButtonState.Pressed && character.GetCurrentVelocity.Y > -0.2f * tileSize && character.NumJumps > 0 )
            {
                character.SetCurrentAnimation(AnimationName.jumpingSpinning);
            }
        }

        public static void MorphBallIncrement(Animation animation, Character character)
        {
            float directionalInputX = character.gamePadState.ThumbSticks.Left.X;

            if ( character.gamePadState.ThumbSticks.Left.Y > 0.7f && Math.Abs(character.GetCeiling() - character.GetCollisionBox().Top) > tileSize / 2)
            {
                character.SetCurrentAnimation(AnimationName.standingUp);
                return;
            }

            // if input is not the direction currently running in, stop
            if ( directionalInputX != 0 && Math.Sign((int)character.Direction) != Math.Sign(directionalInputX) )
            {
                character.StopX();
                character.SetDirection((Direction)((int)character.Direction * -1));
            }

            // if not at max speed, accelerate
            if ( !character.AtMaxSpeedX() )
            {
                character.AccelerateX(character.gamePadState.ThumbSticks.Left.X * 1.2f);
            }
        }

        public static void JumpingSpinningIncrement(Animation animation, Character character)
        {
            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));

            if ( !character.AtMaxSpeedX((Direction)Math.Sign(character.gamePadState.ThumbSticks.Left.X)) )
            {
                character.AccelerateX((Direction)Math.Sign(character.gamePadState.ThumbSticks.Left.X), character.gamePadState.ThumbSticks.Left.X);
            }
        }

        public static void FallingIncrement(Animation animation, Character character)
        {
            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));

            if ( character.gamePadState.Buttons.A == ButtonState.Pressed && character.NumJumps > 0 )
            {
                character.SetCurrentAnimation(AnimationName.jumpingSpinning);
                return;
            }
        }

        public static void CrouchingIncrement(Animation animation, Character character)
        {
            float inputX = character.gamePadState.ThumbSticks.Right.X;
            float inputY = character.gamePadState.ThumbSticks.Right.Y;

            if ( character.gamePadState.ThumbSticks.Left.Y > 0 )
            {
                character.SetCurrentAnimation(AnimationName.standingUp);
                return;
            }
            else if ( character.gamePadState.ThumbSticks.Left.Y < -0.8f )
            {
                character.SetCurrentAnimation(AnimationName.morphBall);
                return;
            }

            if ( Math.Abs(character.gamePadState.ThumbSticks.Left.X) > 0.7f )
            {
                if ( Math.Sign(character.gamePadState.ThumbSticks.Left.X) != (int)character.Direction )
                {
                    character.SetCurrentAnimation(AnimationName.turning);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.standingUp);
                }
                return;
            }

            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingDiagonalDown, finalFrame: true);
                }
                else if ( inputY > 0.1 )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingDiagonalUp, finalFrame: true);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingStraight, finalFrame: true);
                }
            }
            else if ( Math.Abs(inputX) < 0.25 && inputY > 0.5 )
            {
                character.SetCurrentAnimation(AnimationName.aimingCrouchingStraightUp, finalFrame: true);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.crouchingIdle, finalFrame: true);
            }
        }
    }
}
