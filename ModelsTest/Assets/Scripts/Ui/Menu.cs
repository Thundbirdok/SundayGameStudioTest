using UnityEngine;

namespace Ui
{
    using KevinCastejon.MoreAttributes;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button level1;

        [SerializeField, Scene]
        private string level1Scene = "Level1";
        
        private void OnEnable()
        {
            level1.onClick.AddListener(LoadLevel1);
        }

        private void OnDisable()
        {
            level1.onClick.RemoveListener(LoadLevel1);
        }

        private void LoadLevel1()
        {
            SceneManager.LoadSceneAsync(level1Scene);
        }
    }
}
