namespace Car
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CarMover : MonoBehaviour
    {
        public bool IsBreaking { get; private set; }
        public bool IsHandBreaking { get; private set; }
        public float CurrentSteerAngle { get; private set; }

        [SerializeReference]
        private Rigidbody carRigidbody;

        [SerializeField] 
        private float motorForce;

        [SerializeField]
        private float breakVelocity;

        [SerializeField]
        private float breakForce;

        [SerializeField]
        private float handBreakForce;

        [SerializeField]
        private float maxSteerAngle;

        [SerializeField] 
        private WheelCollider frontLeftWheelCollider;

        [SerializeField]
        private WheelCollider frontRightWheelCollider;

        [SerializeField] 
        private WheelCollider rearLeftWheelCollider;

        [SerializeField]
        private WheelCollider rearRightWheelCollider;

        [field: SerializeField]
        public Transform FrontLeftWheelTransform { get; private set; }

        [field: SerializeField]
        public Transform FrontRightWheelTransform { get; private set; }

        [field: SerializeField]
        public Transform RearLeftWheelTransform { get; private set; }

        [field: SerializeField]
        public Transform RearRightWheelTransform { get; private set; }

        private float _turnInput;
        private float _accelerateInput;
        private bool _handBreakInput;

        private bool _isInitialized;

        private PlayerInputs _playerInputs;
        private PlayerInputs.CarActions _actions;
        
        private void OnEnable()
        {
            Initialized();
            
            _actions.Enable();
            
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();

            _actions.Disable();
        }

        private void FixedUpdate() 
        {
            HandleVelocity();
            HandleSteering();
            UpdateWheels();
        }

        private void Initialized()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            _playerInputs = new PlayerInputs(); 
            _actions = _playerInputs.Car;
        }

        private void Subscribe()
        {
            _actions.Accelerate.started += Accelerate;
            _actions.Turn.started += Turn;
            _actions.HandBreak.started += HandBreak;

            _actions.Accelerate.canceled += Accelerate;
            _actions.Turn.canceled += Turn;
            _actions.HandBreak.canceled += HandBreak;
        }

        private void Unsubscribe()
        {
            _actions.Accelerate.started -= Accelerate;
            _actions.Turn.started -= Turn;
            _actions.HandBreak.started -= HandBreak;

            _actions.Accelerate.canceled -= Accelerate;
            _actions.Turn.canceled -= Turn;
            _actions.HandBreak.canceled -= HandBreak;
        }

        private void Accelerate(InputAction.CallbackContext context) => _accelerateInput = context.ReadValue<float>();

        private void Turn(InputAction.CallbackContext context) => _turnInput = context.ReadValue<float>();

        private void HandBreak(InputAction.CallbackContext context) => _handBreakInput = context.started;

        private void HandleVelocity() 
        {
            var relativeVelocity = transform.InverseTransformDirection(carRigidbody.velocity);

            var forwardVelocity = relativeVelocity.z;

            if (IsMovesTooSlow(forwardVelocity))
            {
                Break();

                return;
            }
            
            if (IsTryAccelerateAgainstVelocity(forwardVelocity))
            {
                Break();
                
                return;
            }

            Accelerate();
        }

        private void Accelerate()
        {
            var torque = _accelerateInput * motorForce;
                
            SetMotorTorque(torque);
            
            IsBreaking = false;
            
            if (_handBreakInput)
            {
                HandBreak();
                
                return;
            }

            SetBreakTorque(0);
        }

        private bool IsTryAccelerateAgainstVelocity(float relativeVelocity)
        {
            return relativeVelocity switch
            {
                > 0 when _accelerateInput < 0f => true,
                < 0 when _accelerateInput > 0f => true,
                _                            => false
            };
        }

        private void HandBreak()
        {
            IsBreaking = false;
            IsHandBreaking = true;

            SetHandBreakTorque(handBreakForce);
        }

        private void Break()
        {
            IsBreaking = true;
            IsHandBreaking = false;
            
            SetMotorTorque(0);
            SetBreakTorque(breakForce);
        }

        private bool IsMovesTooSlow(float relativeVelocity)
        {
            return relativeVelocity <= breakVelocity && _accelerateInput == 0;
        }

        private void SetMotorTorque(float torque)
        {
            frontLeftWheelCollider.motorTorque = torque;
            frontRightWheelCollider.motorTorque = torque;
            rearLeftWheelCollider.motorTorque = torque;
            rearRightWheelCollider.motorTorque = torque;
        }

        private void SetBreakTorque(float breakTorque) 
        {
            frontRightWheelCollider.brakeTorque = breakTorque;
            frontLeftWheelCollider.brakeTorque = breakTorque;
            rearLeftWheelCollider.brakeTorque = breakTorque;
            rearRightWheelCollider.brakeTorque = breakTorque;
        }

        private void SetHandBreakTorque(float breakTorque) 
        {
            frontRightWheelCollider.brakeTorque = 0;
            frontLeftWheelCollider.brakeTorque = 0;
            rearLeftWheelCollider.brakeTorque = breakTorque;
            rearRightWheelCollider.brakeTorque = breakTorque;
        }
        
        private void HandleSteering() 
        {
            CurrentSteerAngle = maxSteerAngle * _turnInput;
            frontLeftWheelCollider.steerAngle = CurrentSteerAngle;
            frontRightWheelCollider.steerAngle = CurrentSteerAngle;
        }

        private void UpdateWheels() 
        {
            UpdateSingleWheel(frontLeftWheelCollider, FrontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, FrontRightWheelTransform);
            UpdateSingleWheel(rearRightWheelCollider, RearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, RearLeftWheelTransform);
        }

        private static void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) 
        {
            wheelCollider.GetWorldPose(out var position, out var rotation);
            wheelTransform.rotation = rotation;
            wheelTransform.position = position;
        }
    }
}