using Library.Domain;
using Library.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Library.Domain.Constants;
using static Library.Domain.Enums;

namespace Library.Assets
{
    public class Projectile : AnimateObject
    {
        public readonly WeaponProperties WeaponType;
        public readonly Weapon Weapon;
        public Vector2 StartingCoordinates;
        public int CurrentFrameIndex = 0;
        public int CurrentFrame => CurrentFrameIndex * WeaponType.frameSkip;
        internal int LiveFinalFrame => (WeaponType.liveFrames * WeaponType.frameSkip) - 1;
        internal int DeathFinalFrame => (WeaponType.deathFrames * WeaponType.frameSkip);
        internal bool LiveAnimationCompleted => CurrentFrameIndex >= LiveFinalFrame;
        public bool DeathAnimationCompleted => CurrentFrameIndex >= DeathFinalFrame;
        public bool Dead { get; private set; }
        public new Vector2 Direction;
        public Vector2 velocity;

        public bool AtMaxRange => Math.Abs((StartingCoordinates - Position).Length()) > WeaponType.range;
        public Polygon Polygon => new Polygon(Position.ToPoint(), WeaponType.collisionBoxSize, Angle);
        public float Angle => (float)Math.Atan2(Direction.Y, Direction.X);

        public override Rectangle GetCollisionBox(GameProperties gameState)
        {
            return new Rectangle(
                (int)(Position.X - WeaponType.collisionBoxSize.X / 2),
                (int)(Position.Y - WeaponType.collisionBoxSize.Y / 2),
                WeaponType.collisionBoxSize.X,
                WeaponType.collisionBoxSize.Y
            );
        }



        public void Update(GameProperties gameState)
        {
            if (CurrentFrameIndex == WeaponType.damageFrame * WeaponType.frameSkip) {
                var myPlayer = gameState.players[(PlayerIndex)Weapon.playerIndex];
                if (WeaponType.weaponType == Enums.WeaponType.Bomb && GetCollisionBox(gameState).Intersects(myPlayer.GetCollisionBox(gameState)))
                {
                    myPlayer.SetVelocityY(myPlayer.GetCurrentVelocity.Y + -15f);
                    if (myPlayer.CurrentAnimation.AnimationType != AnimationType.morphBallType)
                        myPlayer.SetCurrentAnimation(AnimationName.falling);
                }

                WillCollideWithPlayer(gameState);
            }

            var terrainCollision = WillCollideWithTerrain(gameState);
            if (!Dead)
            {
                if (AtMaxRange || terrainCollision || (WeaponType.weaponType != Enums.WeaponType.Bomb && WillCollideWithPlayer(gameState)))
                {
                    Dead = true;
                    if (Weapon.WeaponExplosionSounds.ContainsKey(WeaponType.weaponType))
                        Weapon.WeaponExplosionSounds[WeaponType.weaponType].Play(0.5f * gameState.soundLevel, 0, 0);

                    if (terrainCollision)
                        Weapon.WeaponCollisionSound.Play(0.23f * gameState.soundLevel, 0, 0);
                }
            }

            if (!Dead && LiveAnimationCompleted)
            {
                CurrentFrameIndex = WeaponType.liveLoopIndex * WeaponType.frameSkip;
            }
            else if (!DeathAnimationCompleted)
            {
                CurrentFrameIndex += 1;
                if (!Dead)
                {
                    Position += velocity;
                    velocity *= (1f + ((WeaponType.acceleration - 1f) * gameState.CurrentLevel.Gravity / gravity));
                    if (WeaponType.weaponSpeed < 0.001f)
                    {
                        StartingCoordinates -= Vector2.One;
                    }
                }
            }
        }

        public bool WillCollideWithTerrain(GameProperties gameState)
        {
            if (WeaponType.weaponSpeed < 0.001f) return false;

            IEnumerable<TerrainBlock> candidates = from block in gameState.CurrentLevel.BlockMap.Values
                                                   where block.Impenetrable && ((block.Position * gameState.tileSize) + Vector2.One * gameState.tileSize / 2 - Position).Length() < WeaponType.collisionBoxSize.ToVector2().Length() * gameState.tileSize / SpriteTileSize + velocity.Length() * 2
                                                   select block;
            foreach (TerrainBlock candidate in candidates)
            {
                var projectileCollisionBox = GetCollisionBox(gameState);
                var candidateCollisionBox = candidate.GetCollisionBox(gameState);
                var candidatePolygon = new Polygon(candidateCollisionBox);

                var nextPosition = new Polygon((Position + velocity).ToPoint(), (WeaponType.collisionBoxSize.ToVector2() * gameState.tileSize / SpriteTileSize).ToPoint(), Angle);
                if (new Polygon(Polygon.AddPoints(nextPosition.Points)).IsIntersectingWith(candidatePolygon))
                {

                    /*Position = new Vector2(
                        projectileCollisionBox.Right < candidateCollisionBox.Left
                        ? Position.X + candidateCollisionBox.Left - projectileCollisionBox.Right
                        : Position.X - projectileCollisionBox.Left - candidateCollisionBox.Right,
                        projectileCollisionBox.Bottom < candidateCollisionBox.Top
                        ? Position.Y + candidateCollisionBox.Top - projectileCollisionBox.Bottom
                        : Position.Y - projectileCollisionBox.Top - candidateCollisionBox.Bottom
                        );
                    */
                    return true;
                }
            }
            return false;
        }

        public bool WillCollideWithPlayer(GameProperties gameState)
        {
            foreach (Samus.Samus player in gameState.players.Values)
            {
                var candidatePolygon = new Polygon(player.GetCollisionBox(gameState));

                var nextPosition = new Polygon((Position + velocity).ToPoint(), (WeaponType.collisionBoxSize.ToVector2() * gameState.tileSize / SpriteTileSize).ToPoint(), Angle);
                if (new Polygon(Polygon.AddPoints(nextPosition.Points)).IsIntersectingWith(candidatePolygon) && Weapon.playerIndex != player.playerIndex && player.Alive)
                {
                    if (WeaponType.damageFrame == null || CurrentFrameIndex == WeaponType.damageFrame * WeaponType.frameSkip)
                    {
                        player.TakeDamage(WeaponType.weaponPower);
                        Weapon.Character.characterStats.shotsHit++;
                        if (WeaponType.weaponType == Enums.WeaponType.Rocket)
                        {
                            player.SetVelocity(player.GetCurrentVelocity + (Direction * 20));
                        }
                        else if (WeaponType.weaponType == Enums.WeaponType.Bomb) {
                            player.SetVelocityY(player.GetCurrentVelocity.Y + -15f);
                            if (player.CurrentAnimation.AnimationType != AnimationType.morphBallType)
                                player.SetCurrentAnimation(AnimationName.falling);
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public Point DrawCoordinates => new Point((int)(CurrentFrameIndex / WeaponType.frameSkip * SpriteTileSize * SpriteSize.Y), (int)((int)WeaponType.weaponType * SpriteTileSize * SpriteSize.Y));

        public override void Draw(SpriteBatch spriteBatch, GameProperties gameState)
        {

            Rectangle drawRectangle = new Rectangle(
                location: DrawCoordinates,
                size: (SpriteSize * SpriteTileSize).ToPoint()
            );
            spriteBatch.Draw(
                texture: spriteTexture,
                position: Position - gameState.CameraLocation,
                sourceRectangle:
                drawRectangle,
                color: gameState.CurrentLevel.TintColor,
                rotation: Angle,
                origin: new Vector2(SpriteSize.X * SpriteTileSize / 2, SpriteSize.Y * SpriteTileSize / 2),
                scale: gameState.tileSize / SpriteTileSize,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }

        public Projectile(Weapon weapon)
        {
            Vector2 gunLocation = weapon.Character.CurrentAnimation.GunLocation;
            gunLocation.X *= (int)weapon.Character.Direction;

            Vector2 gunDirection = weapon.Character.CurrentAnimation.GunDirection;
            gunDirection.X *= (int)weapon.Character.Direction;

            SpriteTileSize = 16;
            spriteTexture = weapon.texture;
            Position = weapon.Character.Position + (gunLocation * weapon.Character.gameState.tileSize / SpriteTileSize);
            StartingCoordinates = Position;
            SpriteSize = weapon.WeaponProperties.SpriteSize;
            Direction = gunDirection;
            Weapon = weapon;
            WeaponType = new WeaponProperties(weapon.WeaponProperties.weaponType);
            velocity = Direction * (WeaponType.weaponSpeed * weapon.Character.gameState.tileSize);

            if (weapon.WeaponProperties.weaponType == Enums.WeaponType.Bomb)
            {
                Position = new Vector2(Position.X, Position.Y - weapon.Character.GetCollisionBox(weapon.Character.gameState).Height / 3f);
            }
        }
    }
}
