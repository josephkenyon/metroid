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
                new Animation(samus, animationProperties[(int)AnimationName.shootingStandingStraight]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingStandingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingStandingStraightUp]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingStandingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.crouchingIdle]),
                new Animation(samus, animationProperties[(int)AnimationName.standingUp]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingCrouchingStraight]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingCrouchingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingCrouchingStraightUp]),
                new Animation(samus, animationProperties[(int)AnimationName.shootingCrouchingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.running]),
                new Animation(samus, animationProperties[(int)AnimationName.runningShootingCenter]),
                new Animation(samus, animationProperties[(int)AnimationName.runningShootingDiagonalDown]),
                new Animation(samus, animationProperties[(int)AnimationName.runningShootingDiagonalUp]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingIdle]),
                new Animation(samus, animationProperties[(int)AnimationName.falling]),
                new Animation(samus, animationProperties[(int)AnimationName.landing]),
                new Animation(samus, animationProperties[(int)AnimationName.jumpingSpinning]),
                new Animation(samus, animationProperties[(int)AnimationName.morphBall]),
            };
    }
}
