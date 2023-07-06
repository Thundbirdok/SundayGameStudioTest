namespace Car
{
    using UnityEngine;

    public class CarEffectsController : MonoBehaviour
    {
        [SerializeField]
        private CarMover mover;
        
        [SerializeField]
        private TrailRenderer tireRenderer;

        [SerializeField]
        private Transform container;

        private TrailRenderer frontLeftTireTrail;
        private TrailRenderer frontRightTireTrail;
        private TrailRenderer rearLeftTireTrail;
        private TrailRenderer rearRightTireTrail;
        
        private float wheelRadius = 0.64f;
        
        private void Awake()
        {
            var lOffset = new Vector3(0, -wheelRadius);
            var rOffset = new Vector3(0, -wheelRadius);

            var flPosition = transform.TransformPoint(mover.FrontLeftWheelTransform.localPosition + lOffset);
            var frPosition = transform.TransformPoint(mover.FrontRightWheelTransform.localPosition + rOffset);
            var rlPosition = transform.TransformPoint(mover.RearLeftWheelTransform.localPosition + lOffset);
            var rrPosition = transform.TransformPoint(mover.RearRightWheelTransform.localPosition + rOffset);
            
            frontLeftTireTrail = Instantiate(tireRenderer, flPosition, Quaternion.identity, container);
            frontRightTireTrail = Instantiate(tireRenderer, frPosition, Quaternion.identity, container);
            rearLeftTireTrail = Instantiate(tireRenderer, rlPosition, Quaternion.identity, container);
            rearRightTireTrail = Instantiate(tireRenderer, rrPosition, Quaternion.identity, container);
        }

        private void Update()
        {
            frontLeftTireTrail.emitting = mover.IsBreaking;
            frontRightTireTrail.emitting = mover.IsBreaking;
            rearLeftTireTrail.emitting = mover.IsBreaking || mover.IsHandBreaking;
            rearRightTireTrail.emitting = mover.IsBreaking || mover.IsHandBreaking;

            var steeringAngle = Quaternion.Euler
            (
                0,
                mover.CurrentSteerAngle,
                0
            );

            frontLeftTireTrail.transform.localRotation = steeringAngle;
            
            frontRightTireTrail.transform.localRotation = steeringAngle;
        }
    }
}
