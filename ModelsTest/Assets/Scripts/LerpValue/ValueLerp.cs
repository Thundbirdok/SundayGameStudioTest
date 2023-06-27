namespace Lerp
{
    using System;
    using UnityEngine;

    public class ValueLerp<T>
    {
        public T Value { get; private set; }
        
        private readonly float _maxDelta;
        
        private T _targetValue;

        private bool _isInTargetValue = true;

        private readonly Func<T, T, float, T> _smoothFunction;

        public ValueLerp(float maxDelta, Func<T, T, float, T> smoothFunction)
        {
            _maxDelta = maxDelta;
            _smoothFunction = smoothFunction;
        }
        
        public void SetTargetValue(T value)
        {
            _targetValue = value;

            _isInTargetValue = false;
        }

        public bool UpdateValue(float time)
        {
            if (_isInTargetValue)
            {
                return false;
            }

            Value = _smoothFunction(Value, _targetValue, _maxDelta * time);

            if (Value.Equals(_targetValue))
            {
                _isInTargetValue = true;
            }
            
            return true;
        }
    }
}
