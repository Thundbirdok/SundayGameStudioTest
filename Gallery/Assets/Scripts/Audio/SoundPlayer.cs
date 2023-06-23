using UnityEngine;

namespace Audio
{
    using System.Collections.Generic;

    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;

        [SerializeField]
        private List<Sound> sounds = new List<Sound>();

        private void Awake() => DontDestroyOnLoad(transform.root.gameObject);

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        private void Subscribe()
        {
            foreach (var sound in sounds)
            {
                sound.OnSoundPlay += Play;
            }
        }

        private void Unsubscribe()
        {
            foreach (var sound in sounds)
            {
                sound.OnSoundPlay -= Play;
            }
        }

        private void Play(AudioClip clip, float volume) => source.PlayOneShot(clip, volume);
    }
}
