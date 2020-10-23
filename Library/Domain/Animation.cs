﻿using Library.Assets;
using Microsoft.Xna.Framework;
using System;
using static Library.Domain.Constants;
using static Library.Domain.Enums;


namespace Library.Domain
{
    public class Animation
    {
        private int CurrentFrameIndex { get; set; }
        private int FrameCount { get; set; }
        private int? OverrideFrameSkip { get; }
        private int? LoopFrameIndex { get; }
        private bool Actionable { get; }
        private int FrameSkip => OverrideFrameSkip != null ? (int)OverrideFrameSkip : animationFrameSkip;
        internal bool AnimationCompleted => CurrentFrameIndex >= (FrameCount * FrameSkip) - 1;
        internal bool Completed { get; set; }
        public bool IsLooping { get; private set; }
        private int SpriteVerticalCoordinate { get; }
        public AnimationType AnimationType { get; private set; }
        public Direction Direction { get; set; }
        public readonly AnimationName Name;
        private readonly Character Character;
        private Action<Animation, Character> ExecuteBegin { get; set; }
        private Action<Animation, Character> ExecuteIncrement { get; set; }
        private Action<Animation, Character> ExecuteCompleted { get; set; }

        public Animation(Character SetCharacter, AnimationProperties AnimationProperties)
        {
            IsLooping = false;
            Character = SetCharacter;
            Name = AnimationProperties.Name;
            AnimationType = AnimationProperties.AnimationType;
            FrameCount = AnimationProperties.FrameCount;
            LoopFrameIndex = AnimationProperties.LoopFrameIndex;
            OverrideFrameSkip = AnimationProperties.OverrideFrameSkip;
            Actionable = AnimationProperties.Actionable;
            SpriteVerticalCoordinate = AnimationProperties.SpriteVerticalCoordinate;
            ExecuteCompleted = AnimationProperties.ExecuteCompleted;
        }

        public bool IsActionable(AnimationType animationType)
        {
            return (animationType == AnimationType && IsLooping) || (Actionable || Completed);
        }

        public void IncrementFrame()
        {
            if (AnimationCompleted)
            {
                Completed = true;
                if (LoopFrameIndex != null)
                {
                    IsLooping = true;
                    CurrentFrameIndex = (int)LoopFrameIndex * FrameSkip;
                }
                else
                {
                    ExecuteCompleted?.Invoke(this, Character);
                }
            }
            else
            {
                CurrentFrameIndex += 1;
            }
        }

        public void Reset()
        {
            IsLooping = false;
            CurrentFrameIndex = 0;
            Completed = false;
        }

        public Vector2 GetDrawCoordinates(Direction direction)
        {
            Direction activeDirection = AnimationType == AnimationType.turningType ? Direction : direction;

            Vector2 frameOffset = activeDirection == Direction.right
                ? new Vector2(CurrentFrameIndex / FrameSkip, 0)
                : new Vector2(CurrentFrameIndex / FrameSkip + 1, 0) * -1;

            return new Vector2(0, 1) + Character.SpriteTileSize * Character.Size * (new Vector2(Character.SpriteNumber.X, SpriteVerticalCoordinate) + frameOffset);
        }
    }
}