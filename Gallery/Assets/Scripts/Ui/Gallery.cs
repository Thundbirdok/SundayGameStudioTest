using UnityEngine;

namespace Ui
{
    using ScenesManagement;
    using UnityEngine.UI;

    public class Gallery : MonoBehaviour
    {
        [SerializeField]
        private ImageGrid imageGrid;
        
        [SerializeField]
        private Button toMenu;

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
