using UnityEngine;

namespace Ui
{
    using KevinCastejon.MoreAttributes;
    using ScenesManagement;

    public class Gallery : MonoBehaviour
    {
        [SerializeField]
        private ImageGrid imageGrid;
        
        [SerializeField]
        private BackButton toMenu;

        [SerializeField, Scene]
        private string menuScene = "Menu"; 
        
        [SerializeField, Scene]
        private string viewScene = "View";
        
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

        private void ToMenu() => _ = SceneLoaderHandler.Load(menuScene);
        
        private void ToView(Sprite sprite)
        {
            SpriteToView.Sprite = sprite;
            _ = SceneLoaderHandler.Load(viewScene);
        }
    }
}
