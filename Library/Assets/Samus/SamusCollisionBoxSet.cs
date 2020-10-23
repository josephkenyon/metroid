﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Library.Domain.Enums;

namespace Library.Assets.Samus
{
    internal static class SamusCollisionBoxSet
    {
        public static Rectangle DefaultCollisionBox => new Rectangle(Point.Zero, new Point(12, 42));
        public static SortedDictionary<AnimationName, Rectangle> SamusCollisionBoxes => new SortedDictionary<AnimationName, Rectangle>()
        {
            [AnimationName.morphBall] = new Rectangle(Point.Zero, new Point(16, 46)),
            [AnimationName.falling] = new Rectangle(new Point(0, 0), new Point(12, 44)),
            [AnimationName.jumpingIdle] = new Rectangle(new Point(0, 0), new Point(12, 44)),
            [AnimationName.jumpingSpinning] = new Rectangle(new Point(0, -13), new Point(16, 16)),
            [AnimationName.crouchingIdle] = new Rectangle(new Point(0, 0), new Point(12, 32)),
            [AnimationName.aimingCrouchingStraight] = new Rectangle(new Point(0, 0), new Point(12, 32)),
            [AnimationName.aimingCrouchingStraightUp] = new Rectangle(new Point(0, 0), new Point(12, 32)),
            [AnimationName.aimingCrouchingDiagonalUp] = new Rectangle(new Point(0, 0), new Point(12, 32)),
            [AnimationName.aimingCrouchingDiagonalDown] = new Rectangle(new Point(0, 0), new Point(12, 32)),
        };


    }
}