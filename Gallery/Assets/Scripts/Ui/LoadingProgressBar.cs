using UnityEngine;

namespace Ui
{
    using ScenesManagement;
    using UnityEngine.UI;

    public class LoadingProgressBar : MonoBehaviour
    {
        [SerializeField]
        private SceneLoader sceneLoader;

        [SerializeField]
        private Slider progressBar;
        
        private void OnEnable()
        {
            UpdateBar();
            
            sceneLoader.OnProgressChanged += UpdateBar;
        }
        
        private void OnDisable()
        {
            sceneLoader.OnProgressChanged -= UpdateBar;
        }
        
        private void UpdateBar() => progressBar.value = sceneLoader.LoadProgress;
    }
}
