using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public abstract class Character : AnimateObject
    {
        public abstract int MaxHealth { get; }
        public bool Alive { get; private set; }
        public int NumJumps { get; set; }

        public bool IsGrounded => GetCollisionBox().Bottom == GetFloor();
        public bool IsNotCrouching => CurrentAnimation.AnimationType != AnimationType.crouchingType;

        public Vector2 SpriteNumber { get; protected set; }
        public Animation CurrentAnimation => animations[(int)CurrentAnimationIndex];
        public GamePadState gamePadState;
        public GameState gameState;

        protected double health;
        protected Animation[] animations;
        protected AnimationName CurrentAnimationIndex;

        public bool AtMaxSpeedX(float constant = 1f)
        {
            return CurrentVelocity.X * (int)Direction >= MaxVelocity.X * constant;
        }

        public void StopX()
        {
            CurrentVelocity.X *= 0f;

        }

        public bool AtMaxSpeedX(Direction overrideDirection)
        {
            return CurrentVelocity.X * (int)overrideDirection >= MaxVelocity.X;
        }

        public void AccelerateX(Direction overrideDirection, float constant = 1)
        {
            CurrentVelocity.X = CurrentVelocity.X + (Acceleration.X * (int)overrideDirection * Math.Abs(constant));
        }

        public void AccelerateX(float directionConstant = 1f)
        {
            CurrentVelocity.X = CurrentVelocity.X + (Acceleration.X * directionConstant);
        }

        public void AccelerateY(float constant = 1f)
        {
            CurrentVelocity.Y = CurrentVelocity.Y - (Acceleration.Y * constant);
        }

        public void InfluencePosition(Vector2 velocity)
        {
            Position += velocity;
        }

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }

        public void SetCurrentAnimation(AnimationName animationName, int startingFrame = 0, bool finalFrame = false)
        {
            if ( CurrentAnimation.IsActionable(animations[(int)animationName].AnimationType) && CurrentAnimation.Name != animationName || animationName == AnimationName.landing )
            {
                OverrideCurrentAnimation(animationName, startingFrame, finalFrame);
                if ( CurrentAnimation.AnimationType == AnimationType.jumpingType )
                {
                    NumJumps--;
                }
            }
        }

        public void OverrideCurrentAnimation(AnimationName animationName, int startingFrame, bool finalFrame)
        {
            float oldBottom = GetFloor();
            float oldWidth = GetCollisionBox().Width;

            CurrentAnimation.Reset();
            CurrentAnimationIndex = animationName;

            if ( oldWidth != GetCollisionBox().Width )
            {
                InfluencePosition(new Vector2((GetCollisionBox().Width - oldWidth) / 2, 0f));
            }

            float newFloor = GetFloor();
            if ( oldBottom != newFloor && Math.Abs(oldBottom - newFloor) <= tileSize && GetCurrentVelocity.Y > 0f && oldBottom < newFloor )
            {
                InfluencePosition(new Vector2(0f, oldBottom - GetCollisionBox().Bottom));
            }


            CurrentAnimation.Reset(startingFrame, finalFrame);
        }

        public void TakeDamage(double amount)
        {
            health -= amount;
            if ( health < 0 )
            {
                Alive = false;
            }
        }

        public void Heal(double amount)
        {
            health += amount;
            if ( health > MaxHealth )
            {
                health = MaxHealth;
            }
        }

        public float GetFloor()
        {
            Rectangle collisionBox = GetCollisionBox();
            int bottom = collisionBox.Bottom;
            int center = collisionBox.Center.X / tileSize;

            IEnumerable<TerrainBlock> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                   where block.CurrentQuadrant.X >= center - 1
                                                         && block.CurrentQuadrant.X <= center + 1
                                                   select block;

            candidates = candidates.Where(b => b.Position.X < collisionBox.Right && b.Position.X + tileSize > collisionBox.Left).Where(b => b.Position.Y >= bottom);

            return (from block in candidates select (float?)block.Position.Y).Min() ?? 999999f;
        }

        public float GetCeiling()
        {
            Rectangle collisionBox = GetCollisionBox();
            int top = collisionBox.Top;
            int center = collisionBox.Center.X / tileSize;

            IEnumerable<TerrainBlock> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                   where block.CurrentQuadrant.X >= center - 1
                                                         && block.CurrentQuadrant.X <= center + 1
                                                   select block;

            candidates = candidates.Where(
                b => b.Position.X < collisionBox.Right && b.Position.X + tileSize > collisionBox.Left)
                .Where(b => b.Position.Y + tileSize <= top);

            return (from block in candidates select (float?)block.Position.Y + tileSize).Max() ?? -9999999f;
        }

        public float GetLeftWall()
        {
            Rectangle collisionBox = GetCollisionBox();
            int left = collisionBox.Left;
            int center = collisionBox.Center.Y / tileSize;

            IEnumerable<TerrainBlock> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                   where block.CurrentQuadrant.Y >= center - 2
                                                         && block.CurrentQuadrant.Y <= center + 2
                                                   select block;

            candidates = candidates.Where(b => b.Position.Y < collisionBox.Bottom && b.Position.Y + tileSize > collisionBox.Top).Where(b => b.Position.X <= left);

            return (from block in candidates select (float?)block.Position.X + tileSize).Max() ?? -9999999f;
        }


        public float GetRightWall()
        {
            Rectangle collisionBox = GetCollisionBox();
            int right = collisionBox.Right;
            int center = collisionBox.Center.Y / tileSize;

            IEnumerable<TerrainBlock> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                   where block.CurrentQuadrant.Y >= center - 2
                                                         && block.CurrentQuadrant.Y <= center + 2
                                                   select block;

            candidates = candidates.Where(b => b.Position.Y < collisionBox.Bottom && b.Position.Y + tileSize > collisionBox.Top).Where(b => b.Position.X >= right);

            return (from block in candidates select (float?)block.Position.X).Min() ?? 9999999f;
        }

        public void CheckCollisions(float currentFloor, float currentCeiling, float currentRightWall, float currentLeftWall)
        {

            if ( GetCollisionBox().Right > currentRightWall )
            {
                Position = new Vector2(Position.X + (currentRightWall - GetCollisionBox().Right), Position.Y);
            }

            if ( GetCollisionBox().Left < currentLeftWall )
            {
                Position = new Vector2(Position.X + (currentLeftWall - GetCollisionBox().Left), Position.Y);
            }

            if ( GetCollisionBox().Bottom > currentFloor )
            {
                if ( NumJumps < 2 && IsNotCrouching && CurrentAnimation.Name != AnimationName.morphBall && CurrentAnimation.Name != AnimationName.turning )
                {
                    NumJumps = 2;
                    SetCurrentAnimation(AnimationName.landing);
                }
                Position = new Vector2(Position.X, currentFloor - GetCollisionBox().Bottom + Position.Y);
                CurrentVelocity.Y = 0;
                NumJumps = 2;
            }

            if ( GetCollisionBox().Top < currentCeiling )
            {
                SetCurrentAnimation(AnimationName.falling);
                Position = new Vector2(Position.X, currentCeiling + (Position.Y - GetCollisionBox().Top));
                CurrentVelocity.Y = 0;
            }
        }

        public void ApplyGravity(float floor)
        {
            if ( CurrentAnimation.IsLooping || CurrentAnimation.Name != AnimationName.jumpingIdle )
            {
                if ( GetCollisionBox().Bottom != floor )
                {
                    CurrentVelocity.Y += gravity;
                    if ( NumJumps == 2 )
                    {
                        NumJumps--;
                    }
                }
            }
        }

        public abstract void Update(GameState gameState, GamePadState gamePadState);

        public abstract void HandleButtons(GamePadState gamePadState);
    }
}
