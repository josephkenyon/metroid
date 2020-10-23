using Library.Domain;
using static Library.Domain.Enums.AnimationName;
using static Library.Domain.Enums.AnimationType;

namespace Library.Assets.Samus
{
    internal static class SamusAnimationSet
    {
        public static AnimationProperties[] SamusAnimationProperties => new AnimationProperties[]
            {
                new AnimationProperties(Name: idle, AnimationType: standingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 1),
                new AnimationProperties(Name: turning, AnimationType: turningType, FrameCount: 4, Actionable:false, SpriteVerticalCoordinate: 0, ExecuteCompleted: SamusAnimationActions.TurningCompleted),
                new AnimationProperties(Name: shootingStandingStraight, AnimationType: standingType, FrameCount: 1, Actionable: false, SpriteVerticalCoordinate: 1),
                new AnimationProperties(Name: shootingStandingDiagonalUp, AnimationType: standingType, FrameCount: 2, Actionable:false, SpriteVerticalCoordinate: 1),
                new AnimationProperties(Name: shootingStandingStraightUp, AnimationType: standingType, FrameCount: 3, Actionable:false, SpriteVerticalCoordinate: 1),
                new AnimationProperties(Name: shootingStandingDiagonalDown, AnimationType: standingType, FrameCount: 2, Actionable:false, SpriteVerticalCoordinate: 2),
                new AnimationProperties(Name: crouchingIdle, AnimationType: crouchingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 3, LoopFrameIndex: 2),
                new AnimationProperties(Name: standingUp, AnimationType: standingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 4, LoopFrameIndex: 2),
                new AnimationProperties(Name: shootingCrouchingStraight, AnimationType: crouchingType, FrameCount: 1, Actionable: false, SpriteVerticalCoordinate: 5),
                new AnimationProperties(Name: shootingCrouchingDiagonalUp, AnimationType: crouchingType, FrameCount: 2, Actionable: false, SpriteVerticalCoordinate: 5),
                new AnimationProperties(Name: shootingCrouchingStraightUp, AnimationType: crouchingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 5),
                new AnimationProperties(Name: shootingCrouchingDiagonalDown, AnimationType: crouchingType, FrameCount: 2, Actionable: false, SpriteVerticalCoordinate: 6),
                new AnimationProperties(Name: running, AnimationType: runningType, FrameCount: 10, Actionable:true, SpriteVerticalCoordinate: 7, LoopFrameIndex: 1),
                new AnimationProperties(Name: runningShootingCenter, AnimationType: runningType, FrameCount: 9, LoopFrameIndex:1, Actionable: true, SpriteVerticalCoordinate: 8),
                new AnimationProperties(Name: runningShootingDiagonalDown, AnimationType: runningType, FrameCount: 10, Actionable:true, SpriteVerticalCoordinate: 9, LoopFrameIndex: 1),
                new AnimationProperties(Name: runningShootingDiagonalUp, AnimationType: runningType, FrameCount: 10, Actionable:true, SpriteVerticalCoordinate: 10, LoopFrameIndex: 1),
                new AnimationProperties(Name: jumpingIdle, AnimationType: jumpingType, FrameCount: 3, Actionable:false, SpriteVerticalCoordinate: 11, LoopFrameIndex:2),
                new AnimationProperties(Name: falling, AnimationType: fallingType, FrameCount: 4, Actionable:true, SpriteVerticalCoordinate: 12, LoopFrameIndex: 3),
                new AnimationProperties(Name: landing, AnimationType: landingType, FrameCount: 3, Actionable:false, SpriteVerticalCoordinate: 13),
                new AnimationProperties(Name: jumpingSpinning, AnimationType: jumpingType, FrameCount: 10, Actionable: false, SpriteVerticalCoordinate: 14, LoopFrameIndex: 2),
                new AnimationProperties(Name: morphBall, AnimationType: morphBallType, FrameCount: 3, Actionable:false, SpriteVerticalCoordinate: 15, LoopFrameIndex: 3)
            };
    }
}
