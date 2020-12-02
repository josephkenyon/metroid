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

            if ( Math.Abs(inputX) < 0.25 && inputY > 0.5 )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingStraightUp, finalFrame: true);
            }
            else if ( (inputY < -0.15 && Math.Sign(inputX) == Math.Sign((int)character.Direction)) || inputY == -1 )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingDiagonalDown, finalFrame: true);
            }
            else if ( inputY > 0.15 && Math.Sign(inputX) == Math.Sign((int)character.Direction) )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingDiagonalUp, finalFrame: true);
            }
            else if ( Math.Sign(inputX) == Math.Sign((int)character.Direction) )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingStraight, finalFrame: true);
            }
            else if ( character.gamePadState.Triggers.Right > 0 || character.gamePadState.Triggers.Left > 0 )
            {
                character.SetCurrentAnimation(AnimationName.aimingStandingStraight);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.idle);
            }


        }

        public static void RunningIncrement(Animation animation, Character character)
        {
            float directionalInputX = character.gamePadState.ThumbSticks.Left.X;
            float inputX = character.gamePadState.ThumbSticks.Right.X;

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
            if ((character.gamePadState.ThumbSticks.Right.Y < -0.15 && Math.Sign(inputX) == Math.Sign((int)character.Direction)) || character.gamePadState.ThumbSticks.Right.Y == -1 )
            {
                character.SetCurrentAnimation(AnimationName.runningAimingDiagonalDown, newFrame);
            }
            else if ( character.gamePadState.ThumbSticks.Right.Y > 0.15 && Math.Sign(inputX) == Math.Sign((int)character.Direction) )
            {
                character.SetCurrentAnimation(AnimationName.runningAimingDiagonalUp, newFrame);
            }
            else if ( Math.Sign(inputX) == Math.Sign((int)character.Direction) )
            {
                character.SetCurrentAnimation(AnimationName.runningAimingCenter, newFrame);
            }
            else if ( character.gamePadState.Triggers.Right > 0 || character.gamePadState.Triggers.Left > 0 )
            {
                character.SetCurrentAnimation(AnimationName.runningAimingCenter);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.running, newFrame);
            }

            // if not at max speed, accelerate
            if ( !character.AtMaxSpeedX() )
            {
                character.AccelerateX(character.gamePadState.ThumbSticks.Left.X * character.gameState.CurrentLevel.Gravity / gravity);
            }
        }

        public static void JumpingIdleIncrement(Animation animation, Character character)
        {
            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.12f * tileSize, 0));

            if ( animation.IsLooping == false )
            {
                character.AccelerateY();
            }

            if ( character.gamePadState.Buttons.A == ButtonState.Pressed && character.GetCurrentVelocity.Y > -0.5f * tileSize && character.NumJumps > 0 )
            {
                character.SetCurrentAnimation(AnimationName.jumpingSpinning);
            }
            else if ( character.gamePadState.Triggers.Right > 0 || character.gamePadState.Triggers.Left > 0 )
            {
                character.SetCurrentAnimation(AnimationName.jumpingAimingStraight);
            }

            AimingMidAir(animation, character);

        }

        public static void JumpingAimingIncrement(Animation animation, Character character)
        {
            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));
            AimingMidAir(animation, character);
        }

        public static void MorphBallIncrement(Animation animation, Character character)
        {
            float directionalInputX = character.gamePadState.ThumbSticks.Left.X;

            if ( character.gamePadState.ThumbSticks.Left.Y > 0.7f && Math.Abs(character.GetCeiling() - character.GetCollisionBox().Top) > tileSize / 2 )
            {
                character.SetCurrentAnimation(AnimationName.crouchingIdle, finalFrame: true);
                return;
            }

            if ( character.gamePadState.Buttons.A == ButtonState.Pressed )
            {
                if ( character.IsGrounded )
                {
                    if ( Math.Abs(character.GetCeiling() - character.GetCollisionBox().Top) > tileSize / 2 )
                    {
                        character.NumJumps = 2;
                        character.SetCurrentAnimation(AnimationName.jumpingIdle);
                    }
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.jumpingSpinning);
                }
                return;
            }

            // if input is not the direction currently running in, stop
            int test = Math.Sign(directionalInputX);
            int test2 = Math.Sign((int)character.Direction);
            if ( directionalInputX != 0 && Math.Sign((int)character.Direction) != Math.Sign(directionalInputX) )
            {
                character.StopX();
                character.SetDirection((Direction)((int)character.Direction * -1));
            }

            // if not at max speed, accelerate
            if ( !character.AtMaxSpeedX(0.85f) )
            {
                character.AccelerateX(character.gamePadState.ThumbSticks.Left.X * 1.2f);
            }
        }

        public static void JumpingSpinningIncrement(Animation animation, Character character)
        {
            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.15f * tileSize, 0));
        }

        public static void FallingIncrement(Animation animation, Character character)
        {
            float inputX = character.gamePadState.ThumbSticks.Right.X;
            float inputY = character.gamePadState.ThumbSticks.Right.Y;

            character.InfluencePosition(new Vector2(character.gamePadState.ThumbSticks.Left.X * 0.08f * tileSize, 0));

            if ( character.gamePadState.Buttons.A == ButtonState.Pressed && character.NumJumps > 0 )
            {
                character.SetCurrentAnimation(AnimationName.jumpingSpinning);
                return;
            }
            else if ( character.gamePadState.Triggers.Right > 0 || character.gamePadState.Triggers.Left > 0 )
            {
                character.SetCurrentAnimation(AnimationName.jumpingAimingStraight);
            }

            AimingMidAir(animation, character);
        }

        internal static void AimingMidAir(Animation animation, Character character)
        {

            float inputX = character.gamePadState.ThumbSticks.Right.X;
            float inputY = character.gamePadState.ThumbSticks.Right.Y;

            if (Math.Sign(inputX) != Math.Sign((int)character.Direction) && Math.Abs(inputX) > 0.5f) {
                character.SetDirection((Direction)(-(int)character.Direction));
            }

            // aiming while mid air
            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.15 && Math.Sign(inputX) == Math.Sign((int)character.Direction) )
                {
                    character.SetCurrentAnimation(AnimationName.jumpingAimingDiagonalDown, finalFrame: true);
                }
                else if ( inputY > 0.15 && Math.Sign(inputX) == Math.Sign((int)character.Direction) )
                {
                    character.SetCurrentAnimation(AnimationName.jumpingAimingDiagonalUp, finalFrame: true);
                }
                else if ( Math.Sign(inputX) == Math.Sign((int)character.Direction) )
                {
                    character.SetCurrentAnimation(AnimationName.jumpingAimingStraight, finalFrame: true);
                }
            }
            else if ( Math.Abs(inputX) < 0.25 )
            {
                if ( inputY > 0.5 )
                {
                    character.SetCurrentAnimation(AnimationName.jumpingAimingStraightUp, finalFrame: true);
                }
                else if ( inputY < -0.5 )
                {
                    character.SetCurrentAnimation(AnimationName.jumpingAimingStraightDown, finalFrame: true);
                }
            }
        }

        public static void CrouchingIncrement(Animation animation, Character character)
        {
            float inputX = character.gamePadState.ThumbSticks.Right.X;
            float inputY = character.gamePadState.ThumbSticks.Right.Y;

            if ( character.gamePadState.ThumbSticks.Left.Y > 0 && Math.Abs(character.GetCeiling() - character.GetCollisionBox().Top) > tileSize / 2)
            {
                character.SetCurrentAnimation(AnimationName.standingUp);
                return;
            }
            else if ( character.gamePadState.ThumbSticks.Left.Y < -0.8f )
            {
                character.SetCurrentAnimation(AnimationName.morphBall);
                return;
            }

            if ( Math.Abs(character.gamePadState.ThumbSticks.Left.X) > 0.7f && Math.Abs(character.GetCeiling() - character.GetCollisionBox().Top) > tileSize / 2)
            {
                if ( Math.Sign(character.gamePadState.ThumbSticks.Left.X) != (int)character.Direction )
                {
                    character.SetCurrentAnimation(AnimationName.turning);
                    character.CurrentAnimation.Direction = (Direction)((int)character.Direction * -1);
                }
                else
                {
                    character.SetCurrentAnimation(AnimationName.standingUp);
                }
                return;
            }

            if ( Math.Abs(inputX) > 0.3 )
            {
                if ( inputY < -0.1 && Math.Sign(inputX) == Math.Sign((int)character.Direction) )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingDiagonalDown, finalFrame: true);
                }
                else if ( inputY > 0.1 && Math.Sign(inputX) == Math.Sign((int)character.Direction) )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingDiagonalUp, finalFrame: true);
                }
                else if ( Math.Sign(inputX) == Math.Sign((int)character.Direction) )
                {
                    character.SetCurrentAnimation(AnimationName.aimingCrouchingStraight, finalFrame: true);
                }
            }
            else if ( Math.Abs(inputX) < 0.25 && inputY > 0.5 )
            {
                character.SetCurrentAnimation(AnimationName.aimingCrouchingStraightUp, finalFrame: true);
            }
            else if ( character.gamePadState.Triggers.Right > 0 || character.gamePadState.Triggers.Left > 0 )
            {
                character.SetCurrentAnimation(AnimationName.aimingCrouchingStraight);
            }
            else
            {
                character.SetCurrentAnimation(AnimationName.crouchingIdle, finalFrame: true);
            }
        }
    }
}
