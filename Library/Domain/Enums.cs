namespace Library.Domain
{
    public static class Enums
    {
        public enum Direction
        {
            left = -1,
            right = 1
        }

        public enum AnimationName
        {
            idle = 0,
            turning = 1,
            shootingStandingStraight = 2,
            shootingStandingDiagonalUp = 3,
            shootingStandingStraightUp = 4,
            shootingStandingDiagonalDown = 5,
            crouchingIdle = 6,
            standingUp = 7,
            shootingCrouchingStraight = 8,
            shootingCrouchingDiagonalUp = 9,
            shootingCrouchingStraightUp = 10,
            shootingCrouchingDiagonalDown = 11,
            running = 12,
            runningShootingCenter = 13,
            runningShootingDiagonalUp = 14,
            runningShootingDiagonalDown = 15,
            jumpingIdle = 16,
            falling = 17,
            landing = 18,
            jumpingSpinning = 19,
            morphBall = 20,
        }

        public enum AnimationType
        {
            turningType = 0,
            standingType = 1,
            crouchingType = 2,
            runningType = 3,
            jumpingType = 4,
            fallingType = 5,
            landingType = 6,
            morphBallType = 7,
        }
    }
}
