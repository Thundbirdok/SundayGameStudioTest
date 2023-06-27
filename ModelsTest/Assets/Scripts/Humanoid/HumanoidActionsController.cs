namespace Humanoid
{
    using Lerp;
    using Timer;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class HumanoidActionsController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        private bool _isInitialized;
        
        private PlayerInputs _playerInputs;
        private PlayerInputs.HumanoidActions _actions;

        [SerializeField]
        private Vector2Lerp directionInput;
        private static readonly int XDirection = Animator.StringToHash("XDirection");
        private static readonly int YDirection = Animator.StringToHash("YDirection");

        [SerializeField]
        private FloatLerp sprinting;
        private static readonly int Sprinting = Animator.StringToHash("Sprinting");

        [SerializeField]
        private FloatLerp aiming;
        private static readonly int Aiming = Animator.StringToHash("Aiming");

        [SerializeField]
        private FloatLerp idling;
        
        [SerializeField]
        private Timer idleTimer;
        
        private static readonly int Idling = Animator.StringToHash("Idling");
        
        private int _activeActions;

        private bool _isFire;
        private static readonly int IsFire = Animator.StringToHash("IsFire");
        private static readonly int Jump = Animator.StringToHash("Jump");

        private void OnEnable()
        {
            Initialize();
            
            _actions.Enable();

            Subscribe();
        }

        private void OnDisable()
        {
            _actions.Disable();

            Unsubscribe();
        }

        private void Update()
        {
            UpdateValueLerps();
            UpdateIdleState();
        }

        private void UpdateIdleState()
        {
            if (_activeActions > 0 && idleTimer.State != TimerStates.AtZero)
            {
                idleTimer.ResetTime();
                idling.SetTargetValue(0);

                return;
            }

            idleTimer.AddTime(Time.deltaTime);
        }

        private void UpdateValueLerps()
        {
            var deltaTime = Time.deltaTime;
            
            idling.UpdateValue(deltaTime);
            directionInput.UpdateValue(deltaTime);
            sprinting.UpdateValue(deltaTime);
            aiming.UpdateValue(deltaTime);
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            
            _playerInputs = new PlayerInputs();
            _actions = _playerInputs.Humanoid;
        }

        private void Subscribe()
        {
            _actions.Walk.started += WalkInput;
            _actions.Walk.performed += WalkInput;
            _actions.Walk.canceled += WalkInput;

            directionInput.OnValueChanged += Walk;
            
            _actions.Sprint.started += SprintInput;
            _actions.Sprint.canceled += SprintInput;
            
            sprinting.OnValueChanged += Sprint;
            
            _actions.Aim.started += AimInput;
            _actions.Aim.canceled += AimInput;
            
            _actions.Jump.started += JumpInput;
            
            _actions.Fire.started += FireInput;
            _actions.Fire.canceled += FireInput;
         
            idling.OnValueChanged += Idle;
            aiming.OnValueChanged += Aim;
            
            idleTimer.OnFinished += StartIdle;
        }

        private void Unsubscribe()
        {
            _actions.Walk.started -= WalkInput;
            _actions.Walk.performed -= WalkInput;
            _actions.Walk.canceled -= WalkInput;

            directionInput.OnValueChanged -= Walk;
            
            _actions.Sprint.started -= SprintInput;
            _actions.Sprint.canceled -= SprintInput;
            
            _actions.Aim.started -= AimInput;
            _actions.Aim.canceled -= AimInput;
            
            _actions.Jump.started -= JumpInput;
            
            _actions.Fire.started -= FireInput;
            _actions.Fire.canceled -= FireInput;
         
            idling.OnValueChanged -= Idle;
            aiming.OnValueChanged -= Aim;
            
            idleTimer.OnFinished -= StartIdle;
        }

        private void WalkInput(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            directionInput.SetTargetValue(direction);

            Walk();
            
            if (context.started)
            {
                _activeActions++;
            }

            if (context.canceled)
            {
                _activeActions--;
            }
        }

        private void Walk()
        {
            animator.SetFloat(XDirection, directionInput.X);
            animator.SetFloat(YDirection, directionInput.Y);

            Debug.Log("Walk: " + directionInput.Value);
        }

        private void SprintInput(InputAction.CallbackContext context)
        {
            var sprintingValue = context.ReadValue<float>();
            sprinting.SetTargetValue(sprintingValue);

            Sprint();
            
            if (context.started)
            {
                _activeActions++;
            }

            if (context.canceled)
            {
                _activeActions--;
            }
        }

        private void Sprint()
        {
            animator.SetFloat(Sprinting, sprinting.Value);
            
            Debug.Log("Sprinting: " + sprinting.Value);
        }

        private void AimInput(InputAction.CallbackContext context)
        {
            var aimingValue = context.ReadValue<float>();
            aiming.SetTargetValue(aimingValue);

            Aim();
            
            if (context.started)
            {
                _activeActions++;
            }

            if (context.canceled)
            {
                _activeActions--;
            }
        }

        private void Aim()
        {
            animator.SetFloat(Aiming, aiming.Value);
            
            Debug.Log("Aiming: " + aiming.Value);
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            DoJump();
            
            if (context.started)
            {
                _activeActions++;
            }

            if (context.canceled)
            {
                _activeActions--;
            }
        }

        private void DoJump()
        {
            Debug.Log("Jump");
            
            animator.SetTrigger(Jump);
        }
        
        private void FireInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _isFire = true;
                
                _activeActions++;
            }

            if (context.canceled)
            {
                _isFire = false;
                
                _activeActions--;
            }

            Debug.Log("Fire: " + _isFire);
            
            Fire();
        }

        private void Fire()
        {
            animator.SetBool(IsFire, _isFire);
        }

        private void StartIdle()
        {
            idling.SetTargetValue(1);
        }

        private void Idle()
        {
            animator.SetFloat(Idling, idling.Value);
        }
    }
}
