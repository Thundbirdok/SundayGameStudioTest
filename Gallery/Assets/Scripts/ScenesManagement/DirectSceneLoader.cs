using UnityEngine;

namespace ScenesManagement
{
    using KevinCastejon.MoreAttributes;
    using UnityEngine.SceneManagement;

    public class DirectSceneLoader : MonoBehaviour
    {
        [SerializeField,  Scene]
        private string scene = "Menu";

        private void Awake() => SceneManager.LoadSceneAsync(scene);
    }
}
