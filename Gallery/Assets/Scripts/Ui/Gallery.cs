using UnityEngine;

namespace Ui
{
    using ScenesManagement;

    public class Gallery : MonoBehaviour
    {
        [SerializeField]
        private ImageGrid imageGrid;
        
        [SerializeField]
        private BackButton toMenu;

        private const string MENU_SCENE_NAME = "Menu"; 
        
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

        private void ToMenu() => _ = SceneLoaderHandler.Load(MENU_SCENE_NAME);
    }
}
