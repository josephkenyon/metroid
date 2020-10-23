using Library.Domain;
using System;
using static Library.Domain.Enums;

namespace Library.Assets.Samus
{
    public static class SamusAnimationActions
    {
        public static void TurningCompleted(Animation animation, Character character)
        {
            character.SetDirection(animation.Direction);
            character.SetCurrentAnimation(AnimationName.idle);
        }
    }
}
