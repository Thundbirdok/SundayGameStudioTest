namespace Humanoid
{
    using Lerp;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class HumanoidActionsController : MonoBehaviour
    {
        public int ActiveActions { get; private set; }

        [SerializeField]
        private CharacterController characterController;

        [SerializeField]
        private AnimationsController animationsController;

        [SerializeField]
        private CameraController cameraController;
        
        [SerializeField]
        private Transform modelTransform;

        [SerializeField]
        private Camera raycastCamera;

        [SerializeField]
        private Vector2Lerp directionInput;

        [SerializeField]
        private Vector2Lerp lookInput;

        [SerializeField]
        private float walkSpeed = 0.1f;

        [SerializeField]
        private FloatLerp sprinting;

        [SerializeField]
        private FloatLerp aiming;

        [SerializeField]
        private float aimModelRotationSpeed = 20f;

        [SerializeField]
        private bool isPrintLog;

        private bool _isInitialized;

        private PlayerInputs _playerInputs;
        private PlayerInputs.HumanoidActions _actions;

        private bool _isFire;

        private Vector3 _aimTarget;

        private Vector3 _moveForceOfFrame;

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
            ApplyForceToCharacterController();
        }

        private void Update()
        {
            UpdateValueLerps();

            animationsController.UpdateState(Time.deltaTime);
            
            RotateModel();
        }

        private void RotateModel()
        {
            if (IsNeedRotateModel() == false)
            {
                return;
            }

            if (Raycast(out var hit) == false)
            {
                return;
            }

            var position = modelTransform.position;

            hit.y = position.y;
            var direction = (hit - position).normalized;

            DebugLog("Hit: " + hit + " AimDirection: " + direction);

            modelTransform.forward = Vector3.Lerp
            (
                modelTransform.forward,
                direction,
                Time.deltaTime * aimModelRotationSpeed
            );
        }

        private bool IsNeedRotateModel()
        {
            return aiming.Value != 0 || _isFire || directionInput.X != 0 || directionInput.Y != 0;
        }

        private void ApplyForceToCharacterController()
        { 
            characterController.Move(_moveForceOfFrame);
        }

        private bool Raycast(out Vector3 hit)
        {
            var screenCenterPoint = new Vector2((float)Screen.width / 2, (float)Screen.height / 2);
            var ray = raycastCamera.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out var raycastHit, 1500))
            {
                hit = raycastHit.point;

                return true;
            }

            hit = Vector3.zero;
            
            return false;
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
            
            animationsController.Initialize(this);
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
            var targetRotation = Mathf.Atan2
              (
                  directionInput.X,
                  directionInput.Y
              )
              * Mathf.Rad2Deg
              + cameraController.CameraTarget.eulerAngles.y;

            var targetDirection = 
                Quaternion.Euler(0.0f, targetRotation, 0.0f) 
                * Vector3.forward;

            var inputMagnitude = Mathf.Clamp01(directionInput.Value.magnitude);
            var sprintMultiplier = 1 + sprinting.Value;
            var currentSpeed = walkSpeed * inputMagnitude * sprintMultiplier;
            
            _moveForceOfFrame = targetDirection.normalized * currentSpeed;
            
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

        private void DebugLog(string message)
        {
            if (isPrintLog)
            {
                Debug.Log(message);
            }
        }
    }
}
