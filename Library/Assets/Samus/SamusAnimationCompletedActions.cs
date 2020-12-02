using Library.Domain;
using Microsoft.Xna.Framework;
using static Library.Domain.Enums;

namespace Library.Assets.Samus
{
    public static class SamusAnimationBeginActions
    {
        public static void TurningCompleted(Animation animation, Character character)
        {
            character.SetDirection(animation.Direction);
            character.SetCurrentAnimation(AnimationName.idle);
        }

        public static void StandingUpCompleted(Animation animation, Character character)
        {
            character.SetCurrentAnimation(AnimationName.idle);
        }

        public static void LandingCompleted(Animation animation, Character character)
        {
            character.SetCurrentAnimation(AnimationName.idle);
        }

        public static void DeathCompleted(Animation animation, Character character)
        {
            if (character.gameState.GameOver) {
                character.gameState.ScoreScreen = true;
            }
        }
    }
}
