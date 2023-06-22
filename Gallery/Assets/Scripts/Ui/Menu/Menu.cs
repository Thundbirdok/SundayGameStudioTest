namespace Ui.Menu
{
    using EasyTransition;
    using KevinCastejon.MoreAttributes;
    using ScenesManagement;
    using UnityEngine;
    using UnityEngine.UI;

    public class Menu : MonoBehaviour
    {
        [SerializeField, Scene]
        private string galleryScene = "Gallery";

        [SerializeField]
        private Button toGallery;

        [SerializeField]
        private TransitionSettings transition;

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

        private void ToGallery() => SceneLoaderHandler.LoadThroughLoadingScene(galleryScene, transition);
    }
}
