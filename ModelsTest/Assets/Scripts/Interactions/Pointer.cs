namespace Interactions
{
    using UnityEngine;

    public class Pointer : MonoBehaviour
    {
        [SerializeField]
        private Camera raycastCamera;
        
        public void Update()
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
    }
}
