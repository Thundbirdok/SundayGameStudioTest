using UnityEngine;

namespace Ui
{
    using ScenesManagement;
    using UnityEngine.UI;

    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button toGallery;

        private const string GALLERY_SCENE_NAME = "Gallery";
        
        private void OnEnable()
        {
            toGallery.onClick.AddListener(ToGallery);
        }

        private void OnDisable()
        {
            toGallery.onClick.RemoveListener(ToGallery);
        }

        private void ToGallery() => _ = SceneLoaderHandler.Load(GALLERY_SCENE_NAME);
    }
}
