using UnityEngine;

namespace Ui
{
    using KevinCastejon.MoreAttributes;
    using ScenesManagement;
    using UnityEngine.UI;

    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button toGallery;

        [SerializeField, Scene]
        private string galleryScene = "Gallery";
        
        private void OnEnable()
        {
            toGallery.onClick.AddListener(ToGallery);
            Input.backButtonLeavesApp = true;
        }

        private void OnDisable()
        {
            toGallery.onClick.RemoveListener(ToGallery);
            Input.backButtonLeavesApp = false;
        }

        private void ToGallery() => _ = SceneLoaderHandler.Load(galleryScene);
    }
}
