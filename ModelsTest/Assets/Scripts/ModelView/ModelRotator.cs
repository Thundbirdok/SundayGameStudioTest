using UnityEngine;

namespace ModelView
{
    public class ModelRotator : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        
        private void FixedUpdate()
        {
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * speed);
        }
    }
}
