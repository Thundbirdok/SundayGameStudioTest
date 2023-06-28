namespace Humanoid
{
    using System;
    using Cinemachine;
    using UnityEngine;

    [Serializable]
    public class HumanoidCameraController
    {
        [field: SerializeField]
        public Transform CameraTarget { get; private set; }
        
        [SerializeField]
        private Vector2 lookSensitivity = Vector2.one;

        [SerializeField]
        private Vector2 aimSensitivity = Vector2.one / 2;
        
        [SerializeField]
        private Vector2 verticalAimMinMax = new Vector2(-65,  45);
        
        [SerializeField]
        private CinemachineVirtualCamera aimCamera;
        
        private Vector2 _cameraTargetRotation;

        public void RotateCamera(Vector2 lookInput, bool isAiming)
        {
            GetLookInputsWithSensitivity
            (
                lookInput,
                isAiming,
                out var xInput,
                out var yInput
            );

            SetNewCameraRotation(xInput, yInput);
        }

        public void Aim(bool isAiming)
        {
            aimCamera.gameObject.SetActive(isAiming);
        }
        
        private void SetNewCameraRotation(float xInput, float yInput)
        {
            _cameraTargetRotation.x += xInput;
            _cameraTargetRotation.y += yInput;

            ClampCameraTargetRotation();

            CameraTarget.rotation = Quaternion.Euler
            (
                _cameraTargetRotation.y,
                _cameraTargetRotation.x,
                0.0f
            );
        }
        
        private void GetLookInputsWithSensitivity
        (
            in Vector2 lookInput,
            in bool isAiming,
            out float xInput,
            out float yInput
        )
        {
            xInput = lookInput.x;
            yInput = -lookInput.y;

            if (isAiming)
            {
                xInput *= aimSensitivity.x;
                yInput *= aimSensitivity.y;
                
                return;
            }

            xInput *= lookSensitivity.x;
            yInput *= lookSensitivity.y;
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
    }
}
