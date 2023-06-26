using UnityEngine;

namespace Ui.Level1
{
    using KevinCastejon.MoreAttributes;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class Level1 : MonoBehaviour
    {
        [SerializeField, Scene]
        private string menu = "Menu";

        [SerializeField]
        private Button toMenu;

        private void OnEnable() => toMenu.onClick.AddListener(ToMenu);

        private void OnDisable() => toMenu.onClick.RemoveListener(ToMenu);

        private void ToMenu() => SceneManager.LoadSceneAsync(menu);
    }
}
