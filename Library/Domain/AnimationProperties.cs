using Library.Assets;
using Microsoft.Xna.Framework.Input;
using System;
using static Library.Domain.Enums;

namespace Library.Domain
{
    public class AnimationProperties
    {
        public int FrameCount { get; set; }
        public int? OverrideFrameSkip { get; }
        public int? LoopFrameIndex { get; }
        public bool Actionable { get; }
        public int SpriteVerticalCoordinate { get; }
        public AnimationType AnimationType { get; private set; }
        public readonly AnimationName Name;
        public Action<Animation, Character> ExecuteBegin { get; private set; }
        public Action<Animation, Character> ExecuteIncrement { get; private set; }
        public Action<Animation, Character> ExecuteCompleted { get; private set; }

        public AnimationProperties
        (
            AnimationName Name,
            AnimationType AnimationType,
            int FrameCount, bool Actionable,
            int SpriteVerticalCoordinate,
            int? LoopFrameIndex = null,
            int? OverrideFrameSkip = null,
            Action<Animation, Character> ExecuteBegin = null,
            Action<Animation, Character> ExecuteIncrement = null,
            Action<Animation, Character> ExecuteCompleted = null
        )
        {
            this.Name = Name;
            this.AnimationType = AnimationType;
            this.FrameCount = FrameCount;
            this.LoopFrameIndex = LoopFrameIndex;
            this.OverrideFrameSkip = OverrideFrameSkip;
            this.Actionable = Actionable;
            this.SpriteVerticalCoordinate = SpriteVerticalCoordinate;
            this.ExecuteBegin = ExecuteBegin;
            this.ExecuteIncrement = ExecuteIncrement;
            this.ExecuteCompleted = ExecuteCompleted;
        }
    }
}
