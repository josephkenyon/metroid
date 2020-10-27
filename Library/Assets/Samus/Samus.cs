using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
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
            SpriteTileSize = 64;
            Direction = Direction.left;
            Position = new Vector2(tileSize * 3, tileSize * 3);
            Acceleration = new Vector2(0.025f * tileSize, 0.03f * tileSize);
            MaxVelocity = new Vector2(0.18f * tileSize, 0.005f * tileSize);
            SpriteSize = new Vector2(3, 4);
            animations = AnimationInitalizers.InitializeSamusAnimations(this, SamusAnimationProperties);
            SetCurrentAnimation(AnimationName.idle);
        }

        public override void Update(GameState gameState, GamePadState gamePadState)
        {
            this.gameState = gameState;
            this.gamePadState = gamePadState;
            float currentFloor = GetFloor();
            float currentCeiling = GetCeiling();
            float currentRightWall = GetRightWall();
            float currentLeftWall = GetLeftWall();

            HandleButtons(gamePadState);
            CurrentAnimation.Increment(gamePadState);

            Decelerate();
            ApplyGravity(currentFloor);

            Position += CurrentVelocity;


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
                if ( NumJumps < 2 )
                {
                    NumJumps = 2;
                    SetCurrentAnimation(AnimationName.landing);
                }
                Position = new Vector2(Position.X, currentFloor - GetCollisionBox().Bottom + Position.Y);
                CurrentVelocity.Y = 0;
            }

            if ( GetCollisionBox().Top < currentCeiling )
            {
                SetCurrentAnimation(AnimationName.falling);
                Position = new Vector2(Position.X, currentCeiling + (Position.Y - GetCollisionBox().Top));
                CurrentVelocity.Y = 0;
            }

            if ( CurrentVelocity.Y > 0 )
            {
                SetCurrentAnimation(AnimationName.falling);
            }

        }

        public override void ApplyGravity(float floor)
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

        public void Decelerate()
        {
            if ( CurrentAnimation.AnimationType != AnimationType.runningType )
            {
                float constant;
                switch ( CurrentAnimation.Name )
                {
                    case AnimationName.jumpingIdle:
                        constant = 0.6f;
                        break;
                    case AnimationName.jumpingSpinning:
                        constant = 0.2f;
                        break;
                    default:
                        constant = 1f;
                        break;
                }

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
            Rectangle collisionBox = SamusCollisionBoxes.ContainsKey(CurrentAnimation.Name)
                ? SamusCollisionBoxes[CurrentAnimation.Name]
                : DefaultCollisionBox;

            if ( SpriteTileSize != 64 )
            {
                throw new NotImplementedException();
            }

            collisionBox.Width = collisionBox.Width * tileSize / 16;
            collisionBox.Height = collisionBox.Height * tileSize / 16;
            collisionBox.X = (int)(Position.X - (collisionBox.Width / 2) + (collisionBox.X * tileSize / 16));
            collisionBox.Y = (int)(Position.Y - collisionBox.Height + (collisionBox.Y * tileSize / 16));

            collisionBox.Y = (int)(collisionBox.Y + (collisionBox.Height * 0.03));
            collisionBox.Height = (int)(collisionBox.Height * 0.9);

            return collisionBox;
        }

        public override float GetFloor()
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

        public override float GetCeiling()
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

            return (from block in candidates select (float?)block.Position.Y + tileSize).Max() ?? -tileSize;
        }

        public override float GetLeftWall()
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


        public override float GetRightWall()
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
    }
}
