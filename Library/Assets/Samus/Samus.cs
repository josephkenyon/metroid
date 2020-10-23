using Library.Domain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        public override void Update(GamePadState gamePadState)
        {
            HandleMovementX(gamePadState.ThumbSticks.Left);
            HandleButtons(gamePadState);
            CurrentAnimation.Increment(gamePadState);

            if ( CurrentAnimation.AnimationType != AnimationType.runningType && CurrentAnimation.Name != AnimationName.jumpingSpinning )
            {
                float constant = CurrentAnimation.Name == AnimationName.jumpingIdle ? 0.6f : 1f;

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

            if ( CurrentAnimation.IsLooping || CurrentAnimation.Name != AnimationName.jumpingIdle )
            {
                if ( Position.Y != floor )
                {
                    CurrentVelocity.Y += gravity;
                }
            }

            Position += CurrentVelocity;

            if ( Position.Y >= floor )
            {
                if ( NumJumps < 2 )
                {
                    NumJumps = 2;
                    OverrideCurrentAnimation(AnimationName.landing);
                }
                Position = new Vector2(Position.X, floor);
                CurrentVelocity.Y = 0;
            }

            if ( CurrentVelocity.Y > 0 )
            {
                SetCurrentAnimation(AnimationName.falling);
            }

        }


        public override void HandleMovementX(Vector2 directionalInput)
        {
            if ( IsGrounded && IsNotCrouching )
            {
                if ( directionalInput.X == 0 && CurrentAnimation.AnimationType != AnimationType.standingType )
                {
                    SetCurrentAnimation(AnimationName.idle);
                }
                else if ( Math.Sign((int)Direction) != Math.Sign(directionalInput.X) && directionalInput.X != 0 )
                {
                    SetCurrentAnimation(AnimationName.turning);
                    CurrentAnimation.Direction = (Direction)((int)Direction * -1);
                }
            }
        }

        public override void HandleMovementY(Vector2 directionalInput)
        {
            throw new NotImplementedException();
        }

        public override void HandleButtons(GamePadState gamePadState)
        {
            if ( gamePadState.Buttons.A == ButtonState.Pressed && IsGrounded && NumJumps == 2 )
            {
                SetCurrentAnimation(AnimationName.jumpingIdle);
            }
        }

        public override Rectangle GetCollisionBox()
        {
            var collisionBox = SamusCollisionBoxes.ContainsKey(CurrentAnimation.Name)
                ? SamusCollisionBoxes[CurrentAnimation.Name]
                : DefaultCollisionBox;

            collisionBox.Width = collisionBox.Width * tileSize / SpriteTileSize;
            collisionBox.Height = collisionBox.Height * tileSize / SpriteTileSize;
            collisionBox.X = (int)(Position.X - (collisionBox.Width / 2) + (collisionBox.X * tileSize / SpriteTileSize));
            collisionBox.Y = (int)(Position.Y - collisionBox.Height + (collisionBox.Y * tileSize / SpriteTileSize));

            return collisionBox;
        }
    }
}
