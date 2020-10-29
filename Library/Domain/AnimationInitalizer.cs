using Library.Assets.Samus;
using static Library.Domain.Enums;

namespace Library.Domain
{
    internal static class AnimationInitalizers
    {
        public static Animation[] InitializeSamusAnimations(Samus samus, AnimationProperties[] animationProperties)
            => new Animation[]
            {
                new Animation(samus, animationProperties[(int)AnimationName.idle]),
                new Animation(samus, animationProperties[(int)AnimationName.turning]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingStandingStraight]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingStandingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingStandingStraightUp]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingStandingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.crouchingIdle]),
                new Animation(samus, animationProperties[(int)AnimationName.standingUp]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingCrouchingStraight]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingCrouchingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingCrouchingStraightUp]),
                new Animation(samus, animationProperties[(int)AnimationName.aimingCrouchingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.running]),
                new Animation(samus, animationProperties[(int)AnimationName.runningAimingCenter]),
                new Animation(samus, animationProperties[(int)AnimationName.runningAimingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.runningAimingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingIdle]),
                new Animation(samus, animationProperties[(int)AnimationName.falling]),
                new Animation(samus, animationProperties[(int)AnimationName.landing]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingSpinning]),
                new Animation(samus, animationProperties[(int)AnimationName.morphBall]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingAimingStraight]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingAimingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingAimingStraightUp]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingAimingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingAimingStraightDown]),
                new Animation(samus, animationProperties[(int)AnimationName.dead]),

            };
    }
}
