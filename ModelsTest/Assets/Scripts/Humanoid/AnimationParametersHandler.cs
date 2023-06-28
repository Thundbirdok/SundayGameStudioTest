using UnityEngine;

namespace Humanoid
{
    public static class AnimationParametersHandler
    {
        public static readonly int XDirection = Animator.StringToHash("XDirection");
        public static readonly int YDirection = Animator.StringToHash("YDirection");
        
        public static readonly int Idling = Animator.StringToHash("Idling");
        public static readonly int IsFire = Animator.StringToHash("IsFire");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int Aiming = Animator.StringToHash("Aiming");
        public static readonly int Sprinting = Animator.StringToHash("Sprinting");
    }
}
