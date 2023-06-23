using UnityEngine;

namespace Audio
{
    using System;

    [CreateAssetMenu(fileName = "Sound", menuName = "Audio/Sound")]
    public class Sound : ScriptableObject
    {
        public event Action<AudioClip, float> OnSoundPlay;
        
        [SerializeField]
        private AudioClip clickClip;
        
        [SerializeField]
        private AudioSetting setting;

        public void Play() => OnSoundPlay?.Invoke(clickClip, setting.Volume);
    }
}
