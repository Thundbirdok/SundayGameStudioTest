using UnityEngine;

namespace Audio
{
    using UnityEngine.UI;

    public class ButtonClick : MonoBehaviour
    {
        [SerializeField]
        private Button button;

        [SerializeField]
        private Sound sound;
        
        private void OnEnable() => button.onClick.AddListener(Click);

        private void OnDisable() => button.onClick.RemoveListener(Click);

        private void Click() => sound.Play();
    }
}
