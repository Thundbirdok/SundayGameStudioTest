namespace Ui.Menu
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private LoadLevelButton[] loadLevelButtons;

        private void OnEnable()
        {
            foreach (var button in loadLevelButtons)
            {
                button.OnLoadSceneClick += LoadLevel;
            }
        }

        private void OnDisable()
        {
            foreach (var button in loadLevelButtons)
            {
                button.OnLoadSceneClick -= LoadLevel;
            }
        }

        private static void LoadLevel(string scene) => SceneManager.LoadSceneAsync(scene);
    }
}
