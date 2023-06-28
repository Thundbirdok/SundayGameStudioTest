namespace Humanoid
{
    using Lerp;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class HumanoidActionsController : MonoBehaviour
    {
        [SerializeField]
        private HumanoidAnimationsController animationsController;

        [SerializeField]
        private HumanoidCameraController cameraController;

        [SerializeField]
        private HumanoidMovementController movementController;

        [SerializeField]
        private Vector2Lerp directionInput;

        [SerializeField]
        private Vector2Lerp lookInput;

        [SerializeField]
        private FloatLerp sprinting;

        [SerializeField]
        private FloatLerp aiming;

        [SerializeField]
        private bool isPrintLog;
        
        private bool _isInitialized;

        private int _activeActions;

        private bool _isFire;

        private PlayerInputs _playerInputs;
        private PlayerInputs.HumanoidActions _actions;


        private Vector3 _aimTarget;

        private void OnEnable()
        {
            Initialize();
            
            _actions.Enable();
            
            animationsController.Enable();
             
            Subscribe();
        }

        private void OnDisable()
        {
            _actions.Disable();

            animationsController.Disable();
            
            Unsubscribe();
        }

        private void FixedUpdate()
        {
            movementController.Move();
        }

        private void Update()
        {
            UpdateValueLerps();

            animationsController.UpdateState(Time.deltaTime, _activeActions);
            
            movementController.UpdateModelTargetRotation
            (
                directionInput.Value,
                _isFire,
                aiming.Value != 0
            );
        }

        private void UpdateValueLerps()
        {
            var deltaTime = Time.deltaTime;

            directionInput.UpdateValue(deltaTime);
            lookInput.UpdateValue(deltaTime);
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
            
            _actions.Look.started += LookInput;
            _actions.Look.performed += LookInput;
            _actions.Look.canceled += LookInput;
            
            lookInput.OnValueChanged += Look;
            
            _actions.Sprint.started += SprintInput;
            _actions.Sprint.canceled += SprintInput;
            
            sprinting.OnValueChanged += Sprint;
            
            _actions.Aim.started += AimInput;
            _actions.Aim.canceled += AimInput;
            
            _actions.Jump.started += JumpInput;
            
            _actions.Fire.started += FireInput;
            _actions.Fire.canceled += FireInput;
            
            aiming.OnValueChanged += Aim;
        }

        private void Unsubscribe()
        {
            _actions.Walk.started -= WalkInput;
            _actions.Walk.performed -= WalkInput;
            _actions.Walk.canceled -= WalkInput;

            directionInput.OnValueChanged -= Walk;
            
            _actions.Look.started -= LookInput;
            _actions.Look.performed -= LookInput;
            _actions.Look.canceled -= LookInput;
            
            lookInput.OnValueChanged -= Look;
            
            _actions.Sprint.started -= SprintInput;
            _actions.Sprint.canceled -= SprintInput;
            
            _actions.Aim.started -= AimInput;
            _actions.Aim.canceled -= AimInput;
            
            _actions.Jump.started -= JumpInput;
            
            _actions.Fire.started -= FireInput;
            _actions.Fire.canceled -= FireInput;
            
            aiming.OnValueChanged -= Aim;
        }

        private void WalkInput(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            directionInput.SetTargetValue(direction);

            Walk();
            
            CheckActionPhase(context);
        }

        private void Walk()
        {
            movementController.UpdateMove
            (
                directionInput.Value,
                cameraController.CameraTarget.eulerAngles.y,
                sprinting.Value
            );
            
            animationsController.Walk(directionInput.Value);
            
            DebugLog("Walk: " + directionInput.Value);
        }

        private void LookInput(InputAction.CallbackContext context)
        {
            var look = context.ReadValue<Vector2>();
            lookInput.SetTargetValue(look);

            Look();
            Walk();
            
            CheckActionPhase(context);
        }

        private void Look()
        {
            cameraController.RotateCamera(lookInput.Value, aiming.Value > 0);
            
            DebugLog("Look: " + lookInput.Value);
        }

        private void SprintInput(InputAction.CallbackContext context)
        {
            var sprintingValue = context.ReadValue<float>();
            sprinting.SetTargetValue(sprintingValue);

            Sprint();
            
            CheckActionPhase(context);
        }

        private void Sprint()
        {
            animationsController.Sprint(sprinting.Value);
            
            DebugLog("Sprinting: " + sprinting.Value);
        }

        private void AimInput(InputAction.CallbackContext context)
        {
            var aimingValue = context.ReadValue<float>();
            aiming.SetTargetValue(aimingValue);
            
            cameraController.Aim(aimingValue > 0);
            
            Aim();
            
            CheckActionPhase(context);
        }

        private void Aim()
        {
            animationsController.Aim(aiming.Value);
            
            DebugLog("Aiming: " + aiming.Value);
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            if (CheckActionPhase(context))
            {
                Jump();
            }
        }

        private void Jump()
        {
            animationsController.Jump();
            
            DebugLog("Jump");
        }
        
        private void FireInput(InputAction.CallbackContext context)
        {
            _isFire = CheckActionPhase(context);

            Fire();
        }

        private void Fire()
        {
            animationsController.Fire(_isFire);
            
            DebugLog("Fire: " + _isFire);
        }

        private bool CheckActionPhase(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _activeActions++;

                return true;
            }

            if (context.canceled)
            {
                _activeActions--;

                return false;
            }
            
            return true;
        }

        private void DebugLog(string message)
        {
            if (isPrintLog)
            {
                Debug.Log(message);
            }
        }
    }
}
