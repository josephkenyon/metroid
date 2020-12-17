using Library.Domain;
using Microsoft.Xna.Framework;
using static Library.Assets.Samus.SamusAnimationBeginActions;
using static Library.Assets.Samus.SamusAnimationCompletedActions;
using static Library.Assets.Samus.SamusAnimationIncrementActions;
using static Library.Domain.Enums.AnimationName;
using static Library.Domain.Enums.AnimationType;

namespace Library.Assets.Samus
{
    internal static class SamusAnimationSet
    {
        static Vector2 Straight = new Vector2(1, 0);
        static Vector2 DiagonalUp = new Vector2(0.7f, -0.7f);
        static Vector2 StraightUp = new Vector2(0, -1);
        static Vector2 DiagonalDown = new Vector2(0.7f, 0.7f);
        static Vector2 StraightDown = new Vector2(0f, 1f);

        public static AnimationProperties[] SamusAnimationProperties => new AnimationProperties[]
            {
                new AnimationProperties(idle, standingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement, GunLocation: new Vector2(14, -29), GunDirection: new Vector2(1, 0)),
                new AnimationProperties(turning, turningType, FrameCount: 4, Actionable: false, SpriteVerticalCoordinate: 0, OverrideFrameSkip: 2, ExecuteCompleted: TurningCompleted),
                new AnimationProperties(aimingStandingStraight, standingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 2, ExecuteIncrement: IdleIncrement, GunLocation: new Vector2(14, -29), GunDirection: Straight),
                new AnimationProperties(aimingStandingDiagonalUp, standingType, FrameCount: 2, Actionable:true, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement, GunLocation: new Vector2(19, -47), GunDirection: DiagonalUp),
                new AnimationProperties(aimingStandingStraightUp, standingType, FrameCount: 3, Actionable:true, SpriteVerticalCoordinate: 1, ExecuteIncrement: IdleIncrement, GunLocation: new Vector2(1, -54), GunDirection: StraightUp),
                new AnimationProperties(aimingStandingDiagonalDown, standingType, FrameCount: 2, Actionable:true, SpriteVerticalCoordinate: 2, ExecuteIncrement: IdleIncrement, GunLocation: new Vector2(18, -19), GunDirection: DiagonalDown),
                new AnimationProperties(crouchingIdle, crouchingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 3, LoopFrameIndex: 2, ExecuteIncrement: CrouchingIncrement, GunLocation: new Vector2(13, -16), GunDirection: Straight),
                new AnimationProperties(standingUp, standingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 4, OverrideFrameSkip: 2, ExecuteCompleted: StandingUpCompleted),
                new AnimationProperties(aimingCrouchingStraight, crouchingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 6, ExecuteIncrement: CrouchingIncrement, GunLocation: new Vector2(19, -17), GunDirection: Straight),
                new AnimationProperties(aimingCrouchingDiagonalUp, crouchingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 5, ExecuteIncrement: CrouchingIncrement, GunLocation: new Vector2(15, -37), GunDirection: DiagonalUp),
                new AnimationProperties(aimingCrouchingStraightUp, crouchingType, FrameCount: 3, Actionable: true, SpriteVerticalCoordinate: 5, ExecuteIncrement: CrouchingIncrement, GunLocation: new Vector2(0, -43), GunDirection: StraightUp),
                new AnimationProperties(aimingCrouchingDiagonalDown, crouchingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 6, ExecuteIncrement: CrouchingIncrement, GunLocation: new Vector2(17, -14), GunDirection: DiagonalDown),
                new AnimationProperties(running, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 7, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement),
                new AnimationProperties(runningAimingCenter, runningType, FrameCount: 10, LoopFrameIndex: 1, Actionable: true, SpriteVerticalCoordinate: 8, ExecuteIncrement: RunningIncrement, GunLocation: new Vector2(20, -29), GunDirection: Straight),
                new AnimationProperties(runningAimingDiagonalDown, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 9, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement, GunLocation: new Vector2(16, -21), GunDirection: DiagonalDown),
                new AnimationProperties(runningAimingDiagonalUp, runningType, FrameCount: 10, Actionable: true, SpriteVerticalCoordinate: 10, LoopFrameIndex: 1, ExecuteIncrement: RunningIncrement, OverrideFrameSkip: 3, GunLocation: new Vector2(18, -47), GunDirection: DiagonalUp),
                new AnimationProperties(jumpingIdle, jumpingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 11, LoopFrameIndex: 2, ExecuteIncrement: JumpingIdleIncrement, ExecuteBegin: JumpingIdleBegin),
                new AnimationProperties(falling, fallingType, FrameCount: 4, Actionable: true, SpriteVerticalCoordinate: 12, LoopFrameIndex: 3, ExecuteIncrement: FallingIncrement),
                new AnimationProperties(landing, landingType, FrameCount: 3, Actionable: false, SpriteVerticalCoordinate: 13, ExecuteCompleted: LandingCompleted),
                new AnimationProperties(jumpingSpinning, jumpingType, FrameCount: 10, Actionable: false, SpriteVerticalCoordinate: 14, LoopFrameIndex: 2, ExecuteIncrement: JumpingSpinningIncrement, ExecuteBegin: JumpingSpinningBegin),
                new AnimationProperties(morphBall, morphBallType, FrameCount: 10, Actionable: false, SpriteVerticalCoordinate: 15, LoopFrameIndex: 3, ExecuteIncrement: MorphBallIncrement),
                new AnimationProperties(jumpingAimingStraight, jumpingShootingType, FrameCount: 1, Actionable: true, SpriteVerticalCoordinate: 6, LoopFrameIndex: 0, ExecuteIncrement: JumpingAimingIncrement, GunLocation: new Vector2(19, -19), GunDirection: Straight),
                new AnimationProperties(jumpingAimingDiagonalUp, jumpingShootingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 5, LoopFrameIndex: 1, ExecuteIncrement: JumpingAimingIncrement, GunLocation: new Vector2(15, -37), GunDirection: DiagonalUp),
                new AnimationProperties(jumpingAimingStraightUp, jumpingShootingType, FrameCount: 3, Actionable: true, SpriteVerticalCoordinate: 5, LoopFrameIndex: 2, ExecuteIncrement: JumpingAimingIncrement, GunLocation: new Vector2(0, -43), GunDirection: StraightUp),
                new AnimationProperties(jumpingAimingDiagonalDown, jumpingShootingType, FrameCount: 2, Actionable: true, SpriteVerticalCoordinate: 6, LoopFrameIndex: 1, ExecuteIncrement: JumpingAimingIncrement, GunLocation: new Vector2(17, -14), GunDirection: DiagonalDown),
                new AnimationProperties(jumpingAimingStraightDown, jumpingShootingType, FrameCount: 3, Actionable: true, SpriteVerticalCoordinate: 6, LoopFrameIndex: 2, ExecuteIncrement: JumpingAimingIncrement, GunLocation: new Vector2(6, -7), GunDirection: StraightDown),
                new AnimationProperties(dead, jumpingType, FrameCount: 4, Actionable: false, SpriteVerticalCoordinate: 3, LoopFrameIndex: 3, ExecuteCompleted: DeathCompleted),
            };
    }
}
