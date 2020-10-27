using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public abstract class Character : AnimateObject
    {
        public abstract int MaxHealth { get; }
        public bool Alive { get; private set; }
        public int NumJumps { get; protected set; }

        public bool IsGrounded => GetCollisionBox().Bottom == GetFloor();
        public bool IsNotCrouching => CurrentAnimation.AnimationType != AnimationType.crouchingType;

        public Vector2 SpriteNumber { get; protected set; }
        public Animation CurrentAnimation => animations[(int)CurrentAnimationIndex];
        public GamePadState gamePadState;
        public GameState gameState;

        protected double health;
        protected Animation[] animations;
        protected AnimationName CurrentAnimationIndex;

        public bool AtMaxSpeedX()
        {
            return CurrentVelocity.X * (int)Direction >= MaxVelocity.X;
        }

        public bool AtMaxSpeedX(Direction overrideDirection)
        {
            return CurrentVelocity.X * (int)overrideDirection >= MaxVelocity.X;
        }

        public void AccelerateX(Direction overrideDirection, float constant = 1)
        {
            CurrentVelocity.X = CurrentVelocity.X + (Acceleration.X * (int)overrideDirection * Math.Abs(constant));
        }

        public void AccelerateX(float constant = 1f)
        {
            CurrentVelocity.X = CurrentVelocity.X + (Acceleration.X * constant);
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
            if ( CurrentAnimation.IsActionable(animations[(int)animationName].AnimationType) && CurrentAnimation.Name != animationName )
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
            CurrentAnimation.Reset();
            CurrentAnimationIndex = animationName;
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

        public abstract void Update(GameState gameState, GamePadState gamePadState);

        public abstract void HandleButtons(GamePadState gamePadState);

        public abstract void ApplyGravity(float floor);

        public abstract float GetFloor();

        public abstract float GetCeiling();

        public abstract float GetLeftWall();

        public abstract float GetRightWall();

        public override void Draw(SpriteBatch spriteBatch)
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
                position: position,
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
    }
}
