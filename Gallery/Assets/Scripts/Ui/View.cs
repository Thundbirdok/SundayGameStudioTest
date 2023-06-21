using UnityEngine;

namespace Ui
{
    using ScenesManagement;
    using UnityEngine.UI;

    public class View : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Button backButton;

        private const string GALLERY_SCENE_NAME = "Gallery";

        private void OnEnable()
        {
            image.sprite = SpriteToView.Sprite;
            image.preserveAspect = true;

            Screen.orientation = ScreenOrientation.AutoRotation;
            
            backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            
            backButton.onClick.RemoveListener(Back);
        }

        private void Back()
        {
            _ = SceneLoaderHandler.Load(GALLERY_SCENE_NAME);
        }
    }
}
