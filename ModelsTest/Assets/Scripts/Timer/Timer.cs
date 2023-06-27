namespace Timer
{
    using System;
    using UnityEngine;

    [Serializable]
    public class Timer
    {
        public event Action OnFinished;

        public TimerStates State { get; private set; }

        [field: SerializeField]
        public float Duration { get; private set; }

        private float _time;
        
        public Timer(float duration)
        {
            Duration = duration;
        }

        public void ResetTime()
        {
            _time = 0f;
            State = TimerStates.AtZero;
        }

        /// <summary>
        /// Add time
        /// </summary>
        /// <param name="time"></param>
        /// <returns>True if timer ended during this call</returns>
        public bool AddTime(float time)
        {
            if (State == TimerStates.Finished)
            {
                return false;
            }

            State = TimerStates.Running;
            
            _time += time;

            if (_time < Duration)
            {
                return false;
            }

            _time = 0f;

            State = TimerStates.Finished;
            OnFinished?.Invoke();
            
            return true;
        }
    }

    public enum TimerStates
    {
        Running,
        Finished,
        AtZero
    }
}
