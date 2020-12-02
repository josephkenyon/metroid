using Library.Domain;
using static Library.Domain.Constants;

namespace Library.Assets.Samus
{
    public static class SamusAnimationLoopActions
    {
        public static void RunningLoop(Animation animation, Character character)
        {
            character.characterSounds.runSounds[0].Play(0.3f * soundLevel, 0f, 0f);
        }
    }
}
