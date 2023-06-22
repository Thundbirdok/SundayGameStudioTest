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
        
        private void OnEnable()
        {
            toMenu.onClick.AddListener(ToMenu);
            imageGrid.Initialize();
        }

        private void OnDisable()
        {
            toMenu.onClick.RemoveListener(ToMenu);
            imageGrid.Deinitialize();
        }

        private void ToMenu() => _ = SceneLoaderHandler.Load(menuScene);
    }
}
