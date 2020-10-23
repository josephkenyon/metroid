using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library
{
    public class Animation
    {
        private int CurrentFrameIndex { get; set; }
        private int FrameCount { get; set; }
        private int? LoopFrameIndex { get; }
        private bool IsActionable { get; }
        private int SpriteVerticalCoordinates { get; }

        public Animation(int FrameCount, int LoopFrameIndex, bool IsActionable, int SpriteVerticalCoordinates)
        {
            this.FrameCount = FrameCount;
            this.LoopFrameIndex = LoopFrameIndex;
            this.IsActionable = IsActionable;
            this.SpriteVerticalCoordinates = SpriteVerticalCoordinates;
        }

        public void IncrementFrame()
        {
            FrameCount++;
        }

        public Vector2 GetDrawCoordinates()
        {
            return new Vector2(1f, 1f);
        }
    }
}
