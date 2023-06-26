using UnityEngine;

namespace Ui.Level2
{
    using KevinCastejon.MoreAttributes;
    using Ui.Common;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class Level2 : MonoBehaviour
    {
        [SerializeField, Scene]
        private string menu = "Menu";

        [SerializeField]
        private Button toMenu;

        [SerializeField]
        private CarMobileInputs carMobileInputs;

        private void Awake() => carMobileInputs.Initialize();

        private void OnEnable() => toMenu.onClick.AddListener(ToMenu);

        private void OnDisable() => toMenu.onClick.RemoveListener(ToMenu);

        private void ToMenu() => SceneManager.LoadSceneAsync(menu);
    }
}
