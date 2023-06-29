using UnityEngine;

namespace Humanoid
{
    using System;
    using UnityEngine.InputSystem;

    [Serializable]
    public class HumanoidInputController
    {
        public event Action<InputAction.CallbackContext> OnWalk;
        public event Action<InputAction.CallbackContext> OnLook;
        public event Action<InputAction.CallbackContext> OnSprint;
        public event Action<InputAction.CallbackContext> OnAim;
        public event Action<InputAction.CallbackContext> OnJump;
        public event Action<InputAction.CallbackContext> OnFire;

        public int ActiveActions { get; private set; }
        
        private PlayerInputs _playerInputs;
        private PlayerInputs.HumanoidActions _actions;

        public void Initialize()
        {
            _playerInputs = new PlayerInputs();
            _actions = _playerInputs.Humanoid;
        }

        public void Enable()
        {
            _actions.Enable();
            Subscribe();
        }

        public void Disable()
        {
            _actions.Disable();
            Unsubscribe();
        }

        private void Subscribe()
        {
            _actions.Walk.started += WalkInput;
            _actions.Walk.performed += WalkInput;
            _actions.Walk.canceled += WalkInput;

            _actions.Look.started += LookInput;
            _actions.Look.performed += LookInput;
            _actions.Look.canceled += LookInput;

            _actions.Sprint.started += SprintInput;
            _actions.Sprint.canceled += SprintInput;

            _actions.Aim.started += AimInput;
            _actions.Aim.canceled += AimInput;
            
            _actions.Jump.started += JumpInput;
            
            _actions.Fire.started += FireInput;
            _actions.Fire.canceled += FireInput;
        }

        private void Unsubscribe()
        {
            _actions.Walk.started -= WalkInput;
            _actions.Walk.performed -= WalkInput;
            _actions.Walk.canceled -= WalkInput;

            _actions.Look.started -= LookInput;
            _actions.Look.performed -= LookInput;
            _actions.Look.canceled -= LookInput;

            _actions.Sprint.started -= SprintInput;
            _actions.Sprint.canceled -= SprintInput;
            
            _actions.Aim.started -= AimInput;
            _actions.Aim.canceled -= AimInput;
            
            _actions.Jump.started -= JumpInput;
            
            _actions.Fire.started -= FireInput;
            _actions.Fire.canceled -= FireInput;
        }

        private void WalkInput(InputAction.CallbackContext context)
        {
            CheckActionPhase(context);
            OnWalk?.Invoke(context);
        }

        private void LookInput(InputAction.CallbackContext context)
        {
            CheckActionPhase(context);
            OnLook?.Invoke(context);
        }

        private void SprintInput(InputAction.CallbackContext context)
        {
            CheckActionPhase(context);
            OnSprint?.Invoke(context);
        }

        private void AimInput(InputAction.CallbackContext context)
        {
            CheckActionPhase(context);
            OnAim?.Invoke(context);
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            CheckActionPhase(context);
            OnJump?.Invoke(context);
        }

        private void FireInput(InputAction.CallbackContext context)
        {
            CheckActionPhase(context);
            OnFire?.Invoke(context);
        }

        private bool CheckActionPhase(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ActiveActions++;

                return true;
            }

            if (context.canceled)
            {
                ActiveActions--;

                return false;
            }
            
            return true;
        }
    }
}
