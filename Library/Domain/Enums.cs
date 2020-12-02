using System.Collections.Generic;

namespace Library.Domain
{
    public static class Enums
    {
        public enum Direction
        {
            left = -1,
            right = 1
        }

        public enum SamusColor
        {
            Blue,
            Green,
            Orange,
            Purple,
            Red,
            Yellow,
            White,
            Black
        }

        public static Dictionary<string, SamusColor> SamusColorStringConversions = new Dictionary<string, SamusColor>
        {
            { "Blue", SamusColor.Blue },
            { "Green", SamusColor.Green },
            { "Orange", SamusColor.Orange },
            { "Purple", SamusColor.Purple },
            { "Red", SamusColor.Red },
            { "Yellow", SamusColor.Yellow },
            { "White", SamusColor.White },
            { "Black", SamusColor.Black },
        };


        public enum WeaponType
        {
            Charge = 0,
            Rocket = 1,
        }

        public enum PowerUpType
        {
            Health = 0,
            RocketAmmo = 1,
        }

        public enum AnimationName
        {
            idle = 0,
            turning = 1,
            aimingStandingStraight = 2,
            aimingStandingDiagonalUp = 3,
            aimingStandingStraightUp = 4,
            aimingStandingDiagonalDown = 5,
            crouchingIdle = 6,
            standingUp = 7,
            aimingCrouchingStraight = 8,
            aimingCrouchingDiagonalUp = 9,
            aimingCrouchingStraightUp = 10,
            aimingCrouchingDiagonalDown = 11,
            running = 12,
            runningAimingCenter = 13,
            runningAimingDiagonalUp = 14,
            runningAimingDiagonalDown = 15,
            jumpingIdle = 16,
            falling = 17,
            landing = 18,
            jumpingSpinning = 19,
            morphBall = 20,
            jumpingAimingStraight = 21,
            jumpingAimingDiagonalUp = 22,
            jumpingAimingStraightUp = 23,
            jumpingAimingDiagonalDown = 24,
            jumpingAimingStraightDown = 25,
            dead = 26,
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
            jumpingShootingType = 8,
            deadType = 9,
        }
    }
}
