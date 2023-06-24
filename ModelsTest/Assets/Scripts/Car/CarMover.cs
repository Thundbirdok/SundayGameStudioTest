namespace Car
{
    using UnityEngine;
    
    public class CarMover : MonoBehaviour
    {
        [SerializeField] private float motorForce;
        [SerializeField] private float breakForce;
        [SerializeField] private float maxSteerAngle;

        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;
        [SerializeField] private WheelCollider rearLeftWheelCollider;
        [SerializeField] private WheelCollider rearRightWheelCollider;

        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheelTransform;
        [SerializeField] private Transform rearLeftWheelTransform;
        [SerializeField] private Transform rearRightWheelTransform;

        private float _horizontalInput;
        private float _verticalInput;
        private float _currentSteerAngle;
        private float _currentBreakForce;
        private bool _isBreaking;

        private void FixedUpdate() 
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        private void GetInput() 
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            
            _verticalInput = Input.GetAxis("Vertical");
            
            _isBreaking = Input.GetKey(KeyCode.Space);
        }

        private void HandleMotor() 
        {
            frontLeftWheelCollider.motorTorque = _verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = _verticalInput * motorForce;
            _currentBreakForce = _isBreaking ? breakForce : 0f;
            
            ApplyBreaking();
        }

        private void ApplyBreaking() 
        {
            frontRightWheelCollider.brakeTorque = _currentBreakForce;
            frontLeftWheelCollider.brakeTorque = _currentBreakForce;
            rearLeftWheelCollider.brakeTorque = _currentBreakForce;
            rearRightWheelCollider.brakeTorque = _currentBreakForce;
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