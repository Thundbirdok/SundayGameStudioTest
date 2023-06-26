namespace Ui.Menu
{
    using KevinCastejon.MoreAttributes;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button level1;

        [SerializeField, Scene]
        private string level1Scene = "Level1";
        
        [SerializeField]
        private Button level2;

        [SerializeField, Scene]
        private string level2Scene = "Level2";
        
        private void OnEnable()
        {
            level1.onClick.AddListener(LoadLevel1);
            level2.onClick.AddListener(LoadLevel2);
        }

        private void OnDisable()
        {
            level1.onClick.RemoveListener(LoadLevel1);
            level2.onClick.RemoveListener(LoadLevel2);
        }

        private void LoadLevel1() => SceneManager.LoadSceneAsync(level1Scene);
        private void LoadLevel2() => SceneManager.LoadSceneAsync(level2Scene);
    }
}
