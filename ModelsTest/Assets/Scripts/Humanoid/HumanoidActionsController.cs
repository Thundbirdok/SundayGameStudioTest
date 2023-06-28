namespace Humanoid
{
    using Cinemachine;
    using Lerp;
    using Timer;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class HumanoidActionsController : MonoBehaviour
    {
        [SerializeField]
        private CharacterController characterController;
        
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private Transform modelTransform;
        
        [SerializeField]
        private Camera raycastCamera;
        
        [SerializeField]
        private CinemachineVirtualCamera aimCamera;

        [SerializeField]
        private Transform cameraTarget;
        
        private bool _isInitialized;

        [SerializeField]
        private Vector2Lerp directionInput;

        [SerializeField]
        private Vector2Lerp lookInput;

        [SerializeField]
        private Vector2 lookSensitivity = Vector2.one;

        [SerializeField]
        private Vector2 aimSensitivity = Vector2.one / 2;

        [SerializeField]
        private Vector2 verticalAimMinMax = new Vector2(-65,  45);

        [SerializeField]
        private float walkSpeed = 0.1f;

        [SerializeField]
        private FloatLerp sprinting;

        [SerializeField]
        private FloatLerp aiming;

        [SerializeField]
        private FloatLerp idling;

        [SerializeField]
        private Timer idleTimer;

        [SerializeField]
        private float aimModelRotationSpeed = 20f;

        [SerializeField]
        private bool isPrintLog;

        private PlayerInputs _playerInputs;
        private PlayerInputs.HumanoidActions _actions;

        private int _activeActions;

        private bool _isFire;

        private Vector3 _aimTarget;

        private Vector3 _moveForceOfFrame;

        private Vector2 _cameraTargetRotation;

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

        private void FixedUpdate()
        {
            ApplyForceToCharacterController();
        }

        private void Update()
        {
            UpdateValueLerps();
            UpdateIdleState();

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
            return aiming.Value != 0 || directionInput.X != 0 || directionInput.Y != 0;
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
            
            directionInput.UpdateValue(deltaTime);
            lookInput.UpdateValue(deltaTime);
            idling.UpdateValue(deltaTime);
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
         
            idling.OnValueChanged -= Idle;
            aiming.OnValueChanged -= Aim;
            
            idleTimer.OnFinished -= StartIdle;
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
              + cameraTarget.eulerAngles.y;

            var targetDirection = 
                Quaternion.Euler(0.0f, targetRotation, 0.0f) 
                * Vector3.forward;

            var inputMagnitude = Mathf.Clamp01(directionInput.Value.magnitude);
            var sprintMultiplier = 1 + sprinting.Value;
            var currentSpeed = walkSpeed * inputMagnitude * sprintMultiplier;
            
            _moveForceOfFrame = targetDirection.normalized * currentSpeed;

            animator.SetFloat(AnimationParametersHandler.XDirection, directionInput.X);
            animator.SetFloat(AnimationParametersHandler.YDirection, directionInput.Y);

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
            GetLookInputsWithSensitivity(out var xInput, out var yInput);

            SetNewCameraRotation(xInput, yInput);

            DebugLog("Look: " + lookInput.Value);
        }

        private void SetNewCameraRotation(float xInput, float yInput)
        {
            _cameraTargetRotation.x += xInput;
            _cameraTargetRotation.y += yInput;

            ClampCameraTargetRotation();

            cameraTarget.rotation = Quaternion.Euler
                (_cameraTargetRotation.y, _cameraTargetRotation.x, 0.0f);
        }

        private void GetLookInputsWithSensitivity(out float xInput, out float yInput)
        {
            xInput = lookInput.Value.x;
            yInput = -lookInput.Value.y;

            if (aiming.Value == 0)
            {
                xInput *= lookSensitivity.x;
                yInput *= lookSensitivity.y;
                
                return;
            }

            xInput *= aimSensitivity.x;
            yInput *= aimSensitivity.y;
        }

        private void ClampCameraTargetRotation()
        {
            _cameraTargetRotation.x %= 360;

            _cameraTargetRotation.x = 
                _cameraTargetRotation.x > 180
                ? _cameraTargetRotation.x - 360
                : _cameraTargetRotation.x;

            _cameraTargetRotation.x = 
                _cameraTargetRotation.x < -180
                ? _cameraTargetRotation.x + 360
                : _cameraTargetRotation.x;

            _cameraTargetRotation.y = Mathf.Clamp(_cameraTargetRotation.y, verticalAimMinMax.x, verticalAimMinMax.y);
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
            animator.SetFloat(AnimationParametersHandler.Sprinting, sprinting.Value);
            
            DebugLog("Sprinting: " + sprinting.Value);
        }

        private void AimInput(InputAction.CallbackContext context)
        {
            var aimingValue = context.ReadValue<float>();
            aiming.SetTargetValue(aimingValue);

            aimCamera.gameObject.SetActive(aimingValue > 0);
            
            Aim();
            
            CheckActionPhase(context);
        }

        private void Aim()
        {
            animator.SetFloat(AnimationParametersHandler.Aiming, aiming.Value);
            
            DebugLog("Aiming: " + aiming.Value);
        }

        private void JumpInput(InputAction.CallbackContext context)
        {
            DoJump();
            
            CheckActionPhase(context);
        }

        private void DoJump()
        {
            DebugLog("Jump");
            
            animator.SetTrigger(AnimationParametersHandler.Jump);
        }
        
        private void FireInput(InputAction.CallbackContext context)
        {
            _isFire = CheckActionPhase(context);

            DebugLog("Fire: " + _isFire);
            
            Fire();
        }

        private void Fire()
        {
            animator.SetBool(AnimationParametersHandler.IsFire, _isFire);
        }

        private void StartIdle()
        {
            idling.SetTargetValue(1);
        }

        private void Idle()
        {
            animator.SetFloat(AnimationParametersHandler.Idling, idling.Value);
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
