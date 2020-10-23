using Library.Domain;
using static Library.Assets.Samus.SamusAnimationBeginActions;
using static Library.Assets.Samus.SamusAnimationCompletedActions;
using static Library.Assets.Samus.SamusAnimationIncrementActions;
using static Library.Domain.Enums.AnimationName;
using static Library.Domain.Enums.AnimationType;

namespace Library.Assets.Samus
{
    internal static class SamusAnimationSet
    {
        public static AnimationProperties[] SamusAnimationProperties => new AnimationProperties[]
            {
                new AnimationProperties(idle, standingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(turning, turningType, FrameCount: 4, Actionable: false, SpriteVerticalCoordinate: 0, OverrideFrameSkip: 2,ExecuteCompleted: TurningCompleted),
                new AnimationProperties(aimingStandingStraight, standingType, FrameCount: 1, Actionable: false, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(aimingStandingDiagonalUp, standingType, FrameCount: 2, Actionable:false, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(aimingStandingStraightUp, standingType, FrameCount: 3, Actionable:false, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(aimingStandingDiagonalDown, standingType, FrameCount: 2, Actionable:false, SpriteVerticalCoordinate: 2, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(crouchingIdle, crouchingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 3, LoopFrameIndex: 2, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(standingUp, standingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 4, ExecuteCompleted: StandingUpCompleted),
                new AnimationProperties(aimingCrouchingStraight, crouchingType, FrameCount: 1, Actionable: false, SpriteVerticalCoordinate: 4, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(aimingCrouchingDiagonalUp, crouchingType, FrameCount: 2, Actionable: false, SpriteVerticalCoordinate: 5, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(aimingCrouchingStraightUp, crouchingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 5, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(aimingCrouchingDiagonalDown, crouchingType, FrameCount: 2, Actionable: false, SpriteVerticalCoordinate: 6, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(running, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 7, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingCenter, runningType, FrameCount: 10, LoopFrameIndex: 1, Actionable: true, SpriteVerticalCoordinate: 8, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingDiagonalDown, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 9, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingDiagonalUp, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 10, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement, OverrideFrameSkip: 3),
                new AnimationProperties(jumpingIdle, jumpingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 11, LoopFrameIndex: 2, ExecuteIncrement: JumpingIdleIncrement),
                new AnimationProperties(falling, fallingType, FrameCount: 4, Actionable: true, SpriteVerticalCoordinate: 12, LoopFrameIndex: 3, ExecuteIncrement: FallingIncrement),
                new AnimationProperties(landing, landingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 13),
                new AnimationProperties(jumpingSpinning, jumpingType, FrameCount: 10, Actionable: false, SpriteVerticalCoordinate: 14, LoopFrameIndex: 2, ExecuteIncrement: JumpingSpinningIncrement, ExecuteBegin: JumpingSpinningBegin),
                new AnimationProperties(morphBall, morphBallType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 15, LoopFrameIndex: 3)
            };
    }
}
