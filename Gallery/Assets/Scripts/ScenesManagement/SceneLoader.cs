namespace ScenesManagement
{
    using System;
    using System.Threading.Tasks;
    using EasyTransition;
    using UnityEngine;

    public class SceneLoader : MonoBehaviour
    {
        public event Action OnProgressChanged;
        
        public float LoadProgress => Mathf.Min(_timer / minLoadTime, SceneLoaderHandler.LoadingOperation.progress + 0.1f);
        
        [SerializeField]
        private float minLoadTime = 3;
        
        [SerializeField]
        private TransitionSettings transition;

        private float _timer;

        private async void Awake()
        {
            SceneLoaderHandler.LoadTargetScene();

            while (LoadProgress < 1)
            {
                await Task.Yield();
                
                _timer += Time.deltaTime;
                
                OnProgressChanged?.Invoke();
            }

            SceneLoaderHandler.SetTransition(transition, true);
        }
    }
}
