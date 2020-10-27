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
        }
    }
}
