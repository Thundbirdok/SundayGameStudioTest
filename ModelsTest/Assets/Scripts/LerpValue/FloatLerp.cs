namespace Lerp
{
    using System;
    using UnityEngine;

    [Serializable]
    public class FloatLerp
    {
        public event Action OnValueChanged;
        
        public float Value => _lerp.Value;

        [SerializeField]
        private float maxDelta = 0.1f;

        private ValueLerp<float> _lerp;
        
        private bool _isInitialized;
        
        public void SetTargetValue(float value)
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
            
            _lerp = new ValueLerp<float>(maxDelta, Mathf.MoveTowards);
        }
    }
}
