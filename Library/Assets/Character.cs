using Library.Domain;
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

        public bool IsGrounded => Position.Y == floor;
        public bool IsNotCrouching => CurrentAnimation.AnimationType != AnimationType.crouchingType;

        public Vector2 SpriteNumber { get; protected set; }
        public Animation CurrentAnimation => animations[(int)CurrentAnimationIndex];

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

        public void SetCurrentAnimation(AnimationName animationName, int startingFrame = 0)
        {
            if ( CurrentAnimation.IsActionable(animations[(int)animationName].AnimationType) && CurrentAnimation.Name != animationName )
            {
                OverrideCurrentAnimation(animationName, startingFrame);
                if ( CurrentAnimation.AnimationType == AnimationType.jumpingType )
                {
                    NumJumps--;
                }
            }
        }

        public void OverrideCurrentAnimation(AnimationName animationName, int startingFrame = 0)
        {
            CurrentAnimation.Reset(startingFrame);
            CurrentAnimationIndex = animationName;
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

        public abstract void Update(GamePadState gamePadState);

        public abstract void HandleButtons(GamePadState gamePadState);

        public abstract void HandleMovementX(Vector2 directionalInput);

        public abstract void HandleMovementY(Vector2 directionalInput);

        public abstract Rectangle GetCollisionBox();

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRectangle = new Rectangle(
                location: CurrentAnimation.GetDrawCoordinates(Direction).ToPoint(),
                size: (SpriteSize * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: new Vector2(Position.X - (SpriteSize.X * SpriteTileSize * tileSize / SpriteTileSize / 2), Position.Y - (SpriteSize.Y * SpriteTileSize * tileSize / SpriteTileSize)),
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
