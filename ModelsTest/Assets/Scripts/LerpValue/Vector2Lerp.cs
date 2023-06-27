namespace Lerp
{
    using System;
    using UnityEngine;
    
    [Serializable]
    public class Vector2Lerp
    {
        public event Action OnValueChanged;
        
        public Vector2 Value => _lerp.Value;

        public float X => Value.x;
        
        public float Y => Value.y;
        
        [SerializeField]
        private float maxDelta = 0.1f;

        private ValueLerp<Vector2> _lerp;
        
        private bool _isInitialized;
        
        public void SetTargetValue(Vector2 value)
        {
            Initialize();
            
            _lerp.SetTargetValue(value);
        }

        public void UpdateValue(float time)
        {
            Initialize();

            if (_lerp.UpdateValue(time))
            {
                OnValueChanged?.Invoke();
            }
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;
            
            _lerp = new ValueLerp<Vector2>(maxDelta, Vector2.MoveTowards);
        }
    }
}
