namespace Audio
{
    using System;
    using Settings;

    public class AudioSettingsSaver : SettingsSaver<AudioSetting, AudioSettingJson>
    {
        private void Awake() => DontDestroyOnLoad(transform.root.gameObject);
    }
}