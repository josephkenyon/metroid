using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static Library.Domain.Constants;
using static Library.Domain.Enums;
using Library.Domain;

namespace Library.Assets
{
    public abstract class Character : GameObject
    {
        public bool Alive { get; private set; }
        public abstract int MaxHealth { get; }
        public Vector2 SpriteNumber { get; protected set; }
        public int SpriteTileSize { get; protected set; }
        public Direction Direction { get; protected set; }
        public int NumJumps { get; protected set; }
        public bool MovingLeft => CurrentVelocity.X < 0;
        public bool MovingRight => CurrentVelocity.X > 0;
        public bool IsGrounded => Position.Y == floor;
        public bool IsNotCrouching => CurrentAnimation.AnimationType != AnimationType.crouchingType;
        public Animation CurrentAnimation => animations[(int)CurrentAnimationIndex];

        protected double health;
        protected Texture2D spriteTexture;
        protected Vector2 Acceleration;
        protected Vector2 MaxVelocity;
        protected Vector2 CurrentVelocity;
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

        public void Turn()
        {
            SetCurrentAnimation(AnimationName.turning);
            CurrentAnimation.Direction = (Direction)((int)Direction * -1);
        }

        public void Jump()
        {
            switch (NumJumps)
            {
                case 2:
                    SetCurrentAnimation(AnimationName.jumpingIdle);
                    break;
                case 1:
                    SetCurrentAnimation(AnimationName.jumpingSpinning);
                    break;
            }
        }

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }

        public void AccelerateX(Direction overrideDirection, float constant = 1)
        {
            CurrentVelocity.X = CurrentVelocity.X + (Acceleration.X * (int)overrideDirection * Math.Abs(constant));
        }

        public void AccelerateX(float constant = 1)
        {
            CurrentVelocity.X = CurrentVelocity.X + (Acceleration.X * constant);
        }

        public void AccelerateY()
        {
            CurrentVelocity.Y = CurrentVelocity.Y - Acceleration.Y;
        }

        public void SetCurrentAnimation(AnimationName animationName)
        {
            if (CurrentAnimation.IsActionable(animations[(int)animationName].AnimationType) && CurrentAnimation.Name != animationName)
            {
                OverrideCurrentAnimation(animationName);
                if (CurrentAnimation.AnimationType == AnimationType.jumpingType)
                {
                    NumJumps--;
                }
            }
        }

        public void OverrideCurrentAnimation(AnimationName animationName)
        {
            CurrentAnimation.Reset();
            CurrentAnimationIndex = animationName;
        }

        public void TakeDamage(double amount)
        {
            health -= amount;
            if (health < 0)
            {
                Alive = false;
            }
        }

        public void Heal(double amount)
        {
            health += amount;
            if (health > MaxHealth)
            {
                health = MaxHealth;
            }
        }

        public abstract void Update(GamePadState gamePadState);

        public abstract void HandleButtons(GamePadState gamePadState);

        public abstract void HandleMovementX(Vector2 directionalInput);

        public abstract void HandleMovementY(Vector2 directionalInput);

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle drawRectangle = new Rectangle(
                location: CurrentAnimation.GetDrawCoordinates(Direction).ToPoint(),
                size: (Size * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: Position,
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
