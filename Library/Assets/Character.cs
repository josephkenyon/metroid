using System;
using System.Linq;
using Library.State;
using Library.Domain;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using static Library.Domain.Enums;
using static Library.Domain.Constants;

namespace Library.Assets
{
    public abstract class Character : AnimateObject
    {
        public abstract int MaxHealth { get; }
        public int HitTimer { get; protected set; }
        public int NumJumps { get; set; }
        public List<Weapon> Weapons { get; set; }
        public bool Alive => Health > 0;

        public bool IsGrounded => GetCollisionBox(gameState).Bottom == GetFloor();
        public bool IsNotCrouching => CurrentAnimation.AnimationType != AnimationType.crouchingType;

        public Vector2 SpriteNumber { get; protected set; }
        public Animation CurrentAnimation => animations[(int)CurrentAnimationIndex];
        public GamePadState gamePadState;
        public GameProperties gameState;
        public CharacterSounds characterSounds;
        public CharacterStats characterStats = new CharacterStats();

        public float Health { get; protected set; }
        protected Animation[] animations;
        protected AnimationName CurrentAnimationIndex;

        public bool AtMaxSpeedX(float constant = 1f)
            => CurrentVelocity.X * (int)Direction >= MaxVelocity.X * gameState.CurrentLevel.Gravity / gravity * constant;

        public bool AtMaxSpeedX(Direction overrideDirection)
            => CurrentVelocity.X * (int)overrideDirection >= MaxVelocity.X;

        public void AccelerateX(Direction overrideDirection, float constant = 1)
        {
            CurrentVelocity.X += (Acceleration.X * (int)overrideDirection * Math.Abs(constant));
        }

        public void AccelerateX(float directionConstant = 1f)
        {
            CurrentVelocity.X += (Acceleration.X * directionConstant);
        }

        public void AccelerateY(float constant = 0.75f)
        {
            var gravityConstant = gravity == gameState.CurrentLevel.Gravity ? 1f : ((gravity - gameState.CurrentLevel.Gravity) * 130);
            CurrentVelocity.Y -= Acceleration.Y * constant * gravityConstant;
        }

        public void SetVelocityY(float constant)
        {
            var gravityConstant = gravity == gameState.CurrentLevel.Gravity ? 1f : ((gravity - gameState.CurrentLevel.Gravity) * 130);
            CurrentVelocity.Y = constant * gravityConstant;
        }

        public void SetVelocityX(float constant)
        {
            var gravityConstant = gravity == gameState.CurrentLevel.Gravity ? 1f : ((gravity - gameState.CurrentLevel.Gravity) * 130);
            CurrentVelocity.X = constant * gravityConstant;
        }

        public void SetVelocity(Vector2 velocity)
        {
            var gravityConstant = gravity == gameState.CurrentLevel.Gravity ? 1f : ((gravity - gameState.CurrentLevel.Gravity) * 130);
            CurrentVelocity = velocity * gravityConstant;
        }

        public void StopX()
        {
            CurrentVelocity.X *= 0f;
        }

        public void InfluencePosition(Vector2 velocity)
        {
            Position += velocity;
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }

        public void SetCurrentAnimation(AnimationName animationName, int startingFrame = 0, bool finalFrame = false)
        {
            if (CurrentAnimation.IsActionable(animations[(int)animationName].AnimationType) && CurrentAnimation.Name != animationName || animationName == AnimationName.landing)
            {
                OverrideCurrentAnimation(animationName, startingFrame, finalFrame);
                if (CurrentAnimation.AnimationType == AnimationType.jumpingType)
                {
                    characterSounds.jumpVoiceSounds[new Random().Next(1, 3)].Play(0.7f * gameState.soundLevel, 0, 0);
                    NumJumps--;
                }
            }
        }

        public void OverrideCurrentAnimation(AnimationName animationName, int startingFrame, bool finalFrame)
        {
            if (gameState.GameOver) return;

            float oldBottom = GetFloor();
            float oldCeiling = GetCeiling();
            float oldWidth = GetCollisionBox(gameState).Width;
            float oldHeight = GetCollisionBox(gameState).Height;

            CurrentAnimation.Reset();
            CurrentAnimationIndex = animationName;

            // if (oldWidth != GetCollisionBox().Width)
            //   InfluencePosition(new Vector2((GetCollisionBox().Width - oldWidth) / 2, 0f));

            float newFloor = GetFloor();
            float newCeiling = GetCeiling();
            if (oldBottom != newFloor && Math.Abs(oldBottom - newFloor) <= gameState.tileSize && GetCurrentVelocity.Y > 0f && oldBottom < newFloor)
                InfluencePosition(new Vector2(0f, oldBottom - GetCollisionBox(gameState).Bottom - 1));
            //SetCurrentAnimation(AnimationName.crouchingIdle, finalFrame: true);

            if (oldCeiling != newCeiling && Math.Abs(oldCeiling - newCeiling) <= gameState.tileSize && oldCeiling > newCeiling)
                InfluencePosition(new Vector2(0f, oldCeiling - GetCollisionBox(gameState).Top + 1));
            //SetCurrentAnimation(AnimationName.crouchingIdle, finalFrame: true);

            CurrentAnimation.Reset(startingFrame, finalFrame);
        }

        public void TakeDamage(float amount)
        {
            Health -= amount;
            HitTimer = 30;
            if (Health <= 0)
            {
                OverrideCurrentAnimation(AnimationName.dead, 0, false);
                Health = 0;
                characterSounds.deathSound.Play(0.7f * gameState.soundLevel, 0, 0);
                if (gameState.players.Where(p => p.Value.Alive).Count() == 1)
                {
                    gameState.GameOver = true;
                }
            }
            else
            {
                characterSounds.hurtSound.Play(0.35f * gameState.soundLevel, 0, 0);
            }
        }

        public void Heal(float amount)
        {
            Health += amount;
            if (Health > MaxHealth)
                Health = MaxHealth;
        }

        public float GetFloor()
        {
            Rectangle collisionBox = GetCollisionBox(gameState);
            int bottom = collisionBox.Bottom;
            int center = collisionBox.Center.X / gameState.tileSize;

            IEnumerable<GameObject> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                 where block.Impenetrable
                                                       && block.CurrentQuadrant(gameState).X >= center - 1
                                                       && block.CurrentQuadrant(gameState).X <= center + 1
                                                 select block;

            candidates = candidates.Where(
                b => (b.Position * gameState.tileSize).X < collisionBox.Right && (b.Position * gameState.tileSize).X + gameState.tileSize > collisionBox.Left)
                .Where(b => (b.Position * gameState.tileSize).Y >= bottom);

            return (from block in candidates select (float?)(block.Position * gameState.tileSize).Y).Min() ?? 3000f;
        }

        public float GetCeiling()
        {
            Rectangle collisionBox = GetCollisionBox(gameState);
            int top = collisionBox.Top;
            int center = collisionBox.Center.X / gameState.tileSize;

            IEnumerable<GameObject> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                 where block.Impenetrable
                                                       && block.CurrentQuadrant(gameState).X >= center - 1
                                                       && block.CurrentQuadrant(gameState).X <= center + 1
                                                 select block;

            candidates = candidates.Where(
                b => (b.Position * gameState.tileSize).X < collisionBox.Right && (b.Position * gameState.tileSize).X + gameState.tileSize > collisionBox.Left)
                .Where(b => (b.Position * gameState.tileSize).Y + gameState.tileSize <= top);

            return (from block in candidates select (float?)(block.Position * gameState.tileSize).Y + gameState.tileSize).Max() ?? -9999999f;
        }

        public float GetLeftWall()
        {
            Rectangle collisionBox = GetCollisionBox(gameState);
            int left = collisionBox.Left;
            int center = collisionBox.Center.Y / gameState.tileSize;

            IEnumerable<GameObject> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                 where block.Impenetrable
                                                       && block.CurrentQuadrant(gameState).Y >= center - 2
                                                       && block.CurrentQuadrant(gameState).Y <= center + 2
                                                 select block;

            candidates = candidates.Where(
                b => (b.Position * gameState.tileSize).Y < collisionBox.Bottom && (b.Position * gameState.tileSize).Y + gameState.tileSize > collisionBox.Top)
                .Where(b => (b.Position * gameState.tileSize).X <= left);

            return (from block in candidates select (float?)(block.Position * gameState.tileSize).X + gameState.tileSize).Max() ?? -9999999f;
        }

        public float GetRightWall()
        {
            Rectangle collisionBox = GetCollisionBox(gameState);
            int right = collisionBox.Right;
            int center = collisionBox.Center.Y / gameState.tileSize;

            IEnumerable<GameObject> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                 where block.Impenetrable
                                                       && block.CurrentQuadrant(gameState).Y >= center - 2
                                                       && block.CurrentQuadrant(gameState).Y <= center + 2
                                                 select block;

            candidates = candidates.Where(
                b => (b.Position * gameState.tileSize).Y < collisionBox.Bottom && (b.Position * gameState.tileSize).Y + gameState.tileSize > collisionBox.Top)
                .Where(b => (b.Position * gameState.tileSize).X >= right);

            return (from block in candidates select (float?)(block.Position * gameState.tileSize).X).Min() ?? 9999999f;
        }

        public Vector2 GetIntersections(Rectangle collisionBox)
        {
            int right = collisionBox.Right;
            int center = collisionBox.Center.Y / gameState.tileSize;

            var candidate = gameState.CurrentLevel.BlockMap.Values.Where(b => b.GetCollisionBox(gameState).Intersects(collisionBox) && b.Impenetrable).DefaultIfEmpty(new TerrainBlock()).First();

            if (candidate.SpriteTileSize == 0) return Vector2.Zero;

            var blockCollisionBox = candidate.GetCollisionBox(gameState);

            List<int> options = new List<int>{
                Math.Abs(collisionBox.Left - blockCollisionBox.Right),
                Math.Abs(blockCollisionBox.Left - collisionBox.Right),
                Math.Abs(collisionBox.Top - blockCollisionBox.Bottom),
                Math.Abs(blockCollisionBox.Top - collisionBox.Bottom),
            };

            if (options[0] == options.Min() && collisionBox.Left - blockCollisionBox.Right < 0)
                return new Vector2(-(collisionBox.Left - blockCollisionBox.Right), 0f);
            else if (options[1] == options.Min() && blockCollisionBox.Left - collisionBox.Right < 0)
                return new Vector2(-(blockCollisionBox.Left - collisionBox.Right), 0f);
            else if (options[2] == options.Min() && collisionBox.Top - blockCollisionBox.Bottom < 0)
                return new Vector2(0f, -(collisionBox.Top - blockCollisionBox.Bottom));
            else if (options[3] == options.Min() && blockCollisionBox.Top - collisionBox.Bottom < 0)
                return new Vector2(0f, -(blockCollisionBox.Top - collisionBox.Bottom));

            return Vector2.Zero;
        }

        public void CheckCollisions(float currentFloor, float currentCeiling, float currentRightWall, float currentLeftWall)
        {
            var collisionBox = GetCollisionBox(gameState);
            foreach (PowerUpSpawner powerUpSpawner in gameState.CurrentLevel.PowerUpSpawners)
            {
                var powerUp = powerUpSpawner.GetPowerUp();
                if (powerUp != null && collisionBox.Intersects(powerUp.GetCollisionBox(gameState)))
                {
                    powerUp.PowerUpProperties.ExecutePickedUpAction.Invoke(this);
                    powerUpSpawner.PowerUpCollected();
                }
            }

            if (collisionBox.Right > currentRightWall)
            {
                Position = new Vector2(Position.X + (currentRightWall - collisionBox.Right), Position.Y);
                collisionBox.X += (int)(currentRightWall - collisionBox.Right);
            }

            if (collisionBox.Left < currentLeftWall)
            {
                Position = new Vector2(Position.X + (currentLeftWall - collisionBox.Left), Position.Y);
                collisionBox.X += (int)(currentLeftWall - collisionBox.Left);
            }

            if (collisionBox.Bottom > currentFloor)
            {
                if (NumJumps < 2 && IsNotCrouching && CurrentAnimation.Name != AnimationName.morphBall && CurrentAnimation.Name != AnimationName.turning)
                {
                    NumJumps = 2;
                    SetCurrentAnimation(AnimationName.landing);
                }
                Position = new Vector2(Position.X, currentFloor - collisionBox.Bottom + Position.Y);
                collisionBox.Y += (int)(currentFloor - collisionBox.Bottom);
                CurrentVelocity.Y = 0;
                NumJumps = 2;
            }

            if (collisionBox.Top < currentCeiling)
            {
                if (CurrentAnimation.Name == AnimationName.jumpingIdle)
                    SetCurrentAnimation(AnimationName.falling);

                Position = new Vector2(Position.X, currentCeiling + (Position.Y - collisionBox.Top));
                CurrentVelocity.Y = 0;
            }

            Position -= GetIntersections(GetCollisionBox(gameState));
        }

        public void ApplyGravity(float floor)
        {
            var collisionBox = GetCollisionBox(gameState);
            if ((CurrentAnimation.IsLooping || CurrentAnimation.Name != AnimationName.jumpingIdle) && collisionBox.Bottom != floor)
            {
                var constant = gravity == gameState.CurrentLevel.Gravity ? 1f : (gravity - gameState.CurrentLevel.Gravity) * 100;
                CurrentVelocity.Y += gameState.CurrentLevel.Gravity * gameState.tileSize * constant;
                if (NumJumps == 2)
                    NumJumps--;
            }
        }

        public abstract void Update(GameProperties gameState, GamePadState gamePadState);

        public abstract void HandleButtons(GamePadState gamePadState);
    }
}
