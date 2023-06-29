namespace Interactions
{
    using UnityEngine;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

    using UnityEngine.InputSystem.EnhancedTouch;
    using TouchPhase = UnityEngine.InputSystem.TouchPhase;
    
#endif

    public class Pointer : MonoBehaviour
    {
        [SerializeField]
        private Camera raycastCamera;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR

        private void OnEnable()
        {
            TouchSimulation.Enable();
            EnhancedTouchSupport.Enable();
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += Touch;
        }
        
        private void Touch(Finger finger) 
        {
            if (finger.currentTouch.phase != TouchPhase.Began)
            {
                return;
            }

            var ray = raycastCamera.ScreenPointToRay(finger.currentTouch.screenPosition);

            if (Physics.Raycast(ray, out var hit) == false)
            {
                return;
            }

            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }

#else

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) == false)
            {
                return;
            }

            var ray = raycastCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit) == false)
            {
                return;
            }

            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }

#endif
        
    }
}
