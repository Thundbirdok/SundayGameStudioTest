namespace Car
{
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class CarMover : MonoBehaviour
    {
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

        [SerializeField]
        private Transform frontLeftWheelTransform;
        
        [SerializeField]
        private Transform frontRightWheelTransform;
        
        [SerializeField]
        private Transform rearLeftWheelTransform;
        
        [SerializeField]
        private Transform rearRightWheelTransform;

        private float _turnInput;
        private float _accelerateInput;
        private float _currentSteerAngle;
        private bool _handBreakInput;

        private bool _isInitialized;

        private PlayerInputs _playerInputs;
        private PlayerInputs.CarActions _actions;
        
        private void OnEnable()
        {
            Initialized();
            
            _actions.Enable();
            
            _actions.Accelerate.started += Accelerate;
            _actions.Turn.started += Turn;
            _actions.HandBreak.started += HandBreak;
            
            _actions.Accelerate.canceled += Accelerate;
            _actions.Turn.canceled += Turn;
            _actions.HandBreak.canceled += HandBreak;
        }

        private void OnDisable()
        {
            _actions.Accelerate.performed -= Accelerate;
            _actions.Turn.performed -= Turn;
            _actions.HandBreak.performed -= HandBreak;
            
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

            if (_handBreakInput)
            {
                SetHandBreakTorque(handBreakForce);
            }
            else
            {
                SetBreakTorque(0);
            }
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

        private void Break()
        {
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
            _currentSteerAngle = maxSteerAngle * _turnInput;
            frontLeftWheelCollider.steerAngle = _currentSteerAngle;
            frontRightWheelCollider.steerAngle = _currentSteerAngle;
        }

        private void UpdateWheels() 
        {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private static void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform) 
        {
            wheelCollider.GetWorldPose(out var position, out var rotation);
            wheelTransform.rotation = rotation;
            wheelTransform.position = position;
        }
    }
}