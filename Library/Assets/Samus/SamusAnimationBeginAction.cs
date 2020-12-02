using Library.Domain;
using System;
using static Library.Domain.Constants;

namespace Library.Assets.Samus
{
    public static class SamusAnimationCompletedActions
    {
        public static void JumpingSpinningBegin(Animation animation, Character character)
        {
            character.NumJumps = 0;

            var constant = 0.13f;

            if (character.GetCurrentVelocity.Y > -(constant * tileSize))
            {
                character.SetVelocityY(-(constant * 2) * tileSize);
            }
            else
            {
                character.AccelerateY(constant * 0.65f * tileSize);
            }

            character.characterSounds.jumpSounds[2].Play(0.3f * soundLevel, 0f, 0f);
        }

        public static void JumpingIdleBegin(Animation animation, Character character)
        {
            character.characterSounds.jumpSounds[1].Play(0.3f * soundLevel, 0f, 0f);
        }
    }
}
