namespace Humanoid
{
    using System;
    using Lerp;
    using Timer;
    using UnityEngine;

    [Serializable]
    public class AnimationsController
    {
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private FloatLerp idling;

        [SerializeField]
        private Timer idleTimer;
        
        private HumanoidActionsController _actionsController;

        public void Initialize(HumanoidActionsController actionsController)
        {
            _actionsController = actionsController;
        }
        
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

        public void UpdateState(float deltaTime)
        {
            idling.UpdateValue(deltaTime);
            
            UpdateIdleState(deltaTime);
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
        
        private void UpdateIdleState(float deltaTime)
        {
            if (_actionsController.ActiveActions > 0 && idleTimer.State != TimerStates.AtZero)
            {
                idleTimer.ResetTime();
                idling.SetTargetValue(0);

                return;
            }

            idleTimer.AddTime(deltaTime);
        }
    }
}
