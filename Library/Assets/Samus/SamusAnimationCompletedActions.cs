using Library.Domain;
using Microsoft.Xna.Framework.Input;
using System;
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
    }
}
