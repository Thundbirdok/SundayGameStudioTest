namespace Ui.Effects
{
    using UnityEngine;

    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        private float rotationSpeed;
        
        private void Update()
        {
            var angle = rotationSpeed * Time.deltaTime;
            transform.Rotate(0, 0, angle);
        }
    }
}
