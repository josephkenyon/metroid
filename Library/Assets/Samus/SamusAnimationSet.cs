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
                new AnimationProperties(turning, turningType, FrameCount: 4, Actionable: false, SpriteVerticalCoordinate: 0, OverrideFrameSkip: 2, ExecuteCompleted: TurningCompleted),
                new AnimationProperties(aimingStandingStraight, standingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 2, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(aimingStandingDiagonalUp, standingType, FrameCount: 2, Actionable:true, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(aimingStandingStraightUp, standingType, FrameCount: 3, Actionable:true, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(aimingStandingDiagonalDown, standingType, FrameCount: 2, Actionable:true, SpriteVerticalCoordinate: 2, ExecuteIncrement: IdleIncrement),
                new AnimationProperties(crouchingIdle, crouchingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 3, LoopFrameIndex: 2, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(standingUp, standingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 4, OverrideFrameSkip: 2, ExecuteCompleted: StandingUpCompleted),
                new AnimationProperties(aimingCrouchingStraight, crouchingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 6, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(aimingCrouchingDiagonalUp, crouchingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 5, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(aimingCrouchingStraightUp, crouchingType, FrameCount: 3, Actionable: true, SpriteVerticalCoordinate: 5, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(aimingCrouchingDiagonalDown, crouchingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 6, ExecuteIncrement: CrouchingIncrement),
                new AnimationProperties(running, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 7, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingCenter, runningType, FrameCount: 10, LoopFrameIndex: 1, Actionable: true, SpriteVerticalCoordinate: 8, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingDiagonalDown, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 9, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingDiagonalUp, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 10, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement, OverrideFrameSkip: 3),
                new AnimationProperties(jumpingIdle, jumpingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 11, LoopFrameIndex: 2, ExecuteIncrement: JumpingIdleIncrement),
                new AnimationProperties(falling, fallingType, FrameCount: 4, Actionable: true, SpriteVerticalCoordinate: 12, LoopFrameIndex: 3, ExecuteIncrement: FallingIncrement),
                new AnimationProperties(landing, landingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 13, ExecuteCompleted: LandingCompleted),
                new AnimationProperties(jumpingSpinning, jumpingType, FrameCount: 10, Actionable: false, SpriteVerticalCoordinate: 14, LoopFrameIndex: 2, ExecuteIncrement: JumpingSpinningIncrement, ExecuteBegin: JumpingSpinningBegin),
                new AnimationProperties(morphBall, morphBallType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 15, LoopFrameIndex: 3, ExecuteIncrement: MorphBallIncrement),
                new AnimationProperties(jumpingAimingStraight, jumpingShootingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 6, LoopFrameIndex: 0, ExecuteIncrement: JumpingAimingIncrement),
                new AnimationProperties(jumpingAimingDiagonalUp, jumpingShootingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 5, LoopFrameIndex: 1, ExecuteIncrement: JumpingAimingIncrement),
                new AnimationProperties(jumpingAimingStraightUp, jumpingShootingType, FrameCount: 3, Actionable: true, SpriteVerticalCoordinate: 5, LoopFrameIndex: 2, ExecuteIncrement: JumpingAimingIncrement),
                new AnimationProperties(jumpingAimingDiagonalDown, jumpingShootingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 6, LoopFrameIndex: 1, ExecuteIncrement: JumpingAimingIncrement),
                new AnimationProperties(jumpingAimingStraightDown, jumpingShootingType, FrameCount: 3, Actionable: true, SpriteVerticalCoordinate: 6, LoopFrameIndex: 2, ExecuteIncrement: JumpingAimingIncrement),
                new AnimationProperties(dead, jumpingType, FrameCount: 4, Actionable: false, SpriteVerticalCoordinate: 3, LoopFrameIndex: 3),
            };
    }
}
