namespace Car
{
    using System;
    using UnityEngine;
    
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

        private float _horizontalInput;
        private float _verticalInput;
        private float _currentSteerAngle;
        private bool _isHandBreaking;

        private void Update()
        {
            GetInput();
        }

        private void FixedUpdate() 
        {
            HandleVelocity();
            HandleSteering();
            UpdateWheels();
        }

        private void GetInput() 
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            
            _verticalInput = Input.GetAxis("Vertical");
            
            _isHandBreaking = Input.GetKey(KeyCode.Space);
        }

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
            var torque = _verticalInput * motorForce;

            SetMotorTorque(torque);

            if (_isHandBreaking)
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
                > 0 when _verticalInput < 0f => true,
                < 0 when _verticalInput > 0f => true,
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
            return relativeVelocity <= breakVelocity && _verticalInput == 0;
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
            _currentSteerAngle = maxSteerAngle * _horizontalInput;
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