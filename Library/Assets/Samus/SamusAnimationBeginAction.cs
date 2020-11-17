using Library.Domain;
using static Library.Domain.Constants;

namespace Library.Assets.Samus
{
    public static class SamusAnimationCompletedActions
    {
        public static void JumpingSpinningBegin(Animation animation, Character character)
        {
            character.NumJumps = 0;
            character.AccelerateY(0.047f * tileSize);
            character.jumpSounds[2].Play(0.3f * soundLevel, 0f, 0f);
        }

        public static void JumpingIdleBegin(Animation animation, Character character)
        {
            character.jumpSounds[1].Play(0.3f * soundLevel, 0f, 0f);
        }
    }
}
