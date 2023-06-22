namespace ScenesManagement
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneLoader : MonoBehaviour
    {
        public event Action OnProgressChanged;
        
        public float LoadProgress => Mathf.Min(_timer / minLoadTime, _sceneLoading.progress + 0.1f);
        
        [SerializeField]
        private float minLoadTime = 3;
        
        private float _timer;

        private AsyncOperation _sceneLoading;
        
        private async void Awake()
        {
            _sceneLoading = SceneManager.LoadSceneAsync(SceneLoaderHandler.Scene);

            _sceneLoading.allowSceneActivation = false;
            
            while (_sceneLoading.isDone == false && _timer < minLoadTime)
            {
                await Task.Yield();
                
                _timer += Time.deltaTime;
                
                OnProgressChanged?.Invoke();
            }
            
            _sceneLoading.allowSceneActivation = true;
        }
    }
}
