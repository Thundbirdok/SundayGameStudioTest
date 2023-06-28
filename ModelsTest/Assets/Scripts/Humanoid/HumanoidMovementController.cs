using UnityEngine;

namespace Humanoid
{
    using System;

    [Serializable]
    public class HumanoidMovementController
    {
        [SerializeField]
        private CharacterController characterController;
        
        [SerializeField]
        private Transform modelTransform;

        [SerializeField]
        private Camera raycastCamera;
        
        [SerializeField]
        private float walkSpeed = 0.1f;
        
        [SerializeField]
        private float aimModelRotationSpeed = 20f;

        private Vector3 _modelTargetRotation;

        private Vector3 _moveForceOfFrame;

        public void UpdateModelTargetRotation(Vector2 directionInput, bool isFire, bool isAiming)
        {
            if (IsNeedRotateModel(directionInput, isFire, isAiming) == false)
            {
                return;
            }

            if (Raycast(out var hit) == false)
            {
                return;
            }

            var position = modelTransform.position;

            hit.y = position.y;
            _modelTargetRotation = (hit - position).normalized;
        }

        public void UpdateMove(Vector2 directionInput, float rotation, float sprinting)
        {
            var targetRotation = 
                Mathf.Atan2
                (
                    directionInput.x,
                    directionInput.y
                )
                * Mathf.Rad2Deg
                + rotation;

            var targetDirection = 
                Quaternion.Euler(0.0f, targetRotation, 0.0f) 
                * Vector3.forward;

            var inputMagnitude = Mathf.Clamp01(directionInput.magnitude);
            var sprintMultiplier = 1 + sprinting;
            var currentSpeed = walkSpeed * inputMagnitude * sprintMultiplier;
            
            _moveForceOfFrame = targetDirection.normalized * currentSpeed;
        }

        public void Move()
        {
            ApplyForceToCharacterController();
            Rotate();
        }
        
        private void Rotate()
        {
            modelTransform.forward = Vector3.Lerp
            (
                modelTransform.forward,
                _modelTargetRotation,
                Time.deltaTime * aimModelRotationSpeed
            );
        }

        private bool IsNeedRotateModel(Vector2 directionInput, bool isFire, bool isAiming)
        {
            return isAiming 
                   || isFire 
                   || directionInput.x != 0 
                   || directionInput.y != 0;
        }

        public void ApplyForceToCharacterController()
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
    }
}
