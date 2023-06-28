namespace Humanoid
{
    using System;
    using Lerp;
    using Timer;
    using UnityEngine;

    [Serializable]
    public class HumanoidAnimationsController
    {
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private FloatLerp idling;

        [SerializeField]
        private Timer idleTimer;

        public void Enable()
        {
            idling.OnValueChanged += Idle;
            idleTimer.OnFinished += StartIdle;
        }

        public void Disable()
        {
            idling.OnValueChanged -= Idle;
            idleTimer.OnFinished -= StartIdle;
        }

        public void UpdateState(float deltaTime, int activeActions)
        {
            idling.UpdateValue(deltaTime);
            
            UpdateIdleState(deltaTime, activeActions);
        }
        
        public void Fire(bool isFire)
        {
            animator.SetBool(AnimationParametersHandler.IsFire, isFire);
        }
        
        public void Jump()
        {
            animator.SetTrigger(AnimationParametersHandler.Jump);
        }
        
        public void Aim(float aiming)
        {
            animator.SetFloat(AnimationParametersHandler.Aiming, aiming);
        }
        
        public void Sprint(float sprinting)
        {
            animator.SetFloat(AnimationParametersHandler.Sprinting, sprinting);
        }

        public void Walk(Vector2 direction)
        {
            animator.SetFloat(AnimationParametersHandler.XDirection, direction.x);
            animator.SetFloat(AnimationParametersHandler.YDirection, direction.y);
        }
        
        private void StartIdle()
        {
            idling.SetTargetValue(1);
        }

        private void Idle()
        {
            animator.SetFloat(AnimationParametersHandler.Idling, idling.Value);
        }
        
        private void UpdateIdleState(float deltaTime, int activeActions)
        {
            if (activeActions > 0 && idleTimer.State != TimerStates.AtZero)
            {
                idleTimer.ResetTime();
                idling.SetTargetValue(0);

                return;
            }

            idleTimer.AddTime(deltaTime);
        }
    }
}
