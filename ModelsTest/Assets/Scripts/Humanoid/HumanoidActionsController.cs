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
        private HumanoidInputController inputController;
        
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

        private bool _isFire;
        
        private Vector3 _aimTarget;

        private void OnEnable()
        {
            Initialize();
            
            inputController.Enable();
            animationsController.Enable();
             
            Subscribe();
        }

        private void OnDisable()
        {
            inputController.Disable();
            animationsController.Disable();
            
            Unsubscribe();
        }

        private void FixedUpdate() => movementController.Move();

        private void Update()
        {
            UpdateValueLerps();

            animationsController.UpdateState(Time.deltaTime, inputController.ActiveActions);
            
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
            
            inputController.Initialize();
        }

        private void Subscribe()
        {
            inputController.OnWalk += WalkInput;
            directionInput.OnValueChanged += Walk;
            
            inputController.OnLook += LookInput;
            lookInput.OnValueChanged += Look;
            
            inputController.OnSprint += SprintInput;
            sprinting.OnValueChanged += Sprint;
            
            inputController.OnAim += AimInput;
            aiming.OnValueChanged += Aim;

            inputController.OnJump += JumpInput;

            inputController.OnFire += FireInput;
        }

        private void Unsubscribe()
        {
            inputController.OnWalk -= WalkInput;
            directionInput.OnValueChanged -= Walk;
            
            inputController.OnLook -= LookInput;
            lookInput.OnValueChanged -= Look;
            
            inputController.OnSprint -= SprintInput;
            sprinting.OnValueChanged -= Sprint;
            
            inputController.OnAim -= AimInput;
            aiming.OnValueChanged -= Aim;

            inputController.OnJump -= JumpInput;

            inputController.OnFire -= FireInput;
        }

        private void WalkInput(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            directionInput.SetTargetValue(direction);

            Walk();
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
        }

        private void Aim()
        {
            animationsController.Aim(aiming.Value);
            DebugLog("Aiming: " + aiming.Value);
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            if (context.started)
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
            _isFire = context.started;

            Fire();
        }

        private void Fire()
        {
            animationsController.Fire(_isFire);
            DebugLog("Fire: " + _isFire);
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
