namespace Ui.Gallery
{
    using EasyTransition;
    using KevinCastejon.MoreAttributes;
    using ScenesManagement;
    using Ui.Common;
    using UnityEngine;

    public class Gallery : MonoBehaviour
    {
        [SerializeField, Scene]
        private string menuScene = "Menu"; 
        
        [SerializeField, Scene]
        private string viewScene = "View";

        [SerializeField]
        private ImageGrid imageGrid;

        [SerializeField]
        private BackButton toMenu;

        [SerializeField]
        private TransitionSettings transition;
        
        private AsyncOperation _loadingOperation;
        
        private void OnEnable()
        {
            toMenu.onClick.AddListener(ToMenu);
            imageGrid.Initialize();
            imageGrid.OnImageSelected += ToView;
        }

        private void OnDisable()
        {
            toMenu.onClick.RemoveListener(ToMenu);
            imageGrid.Deinitialize();
            imageGrid.OnImageSelected -= ToView;
        }

        private void ToMenu() => SceneLoaderHandler.LoadThroughLoadingScene(menuScene, transition);
        
        private void ToView(Sprite sprite)
        {
            SpriteToView.Sprite = sprite;
            SceneLoaderHandler.LoadThroughLoadingScene(viewScene, transition);
        }
    }
}
