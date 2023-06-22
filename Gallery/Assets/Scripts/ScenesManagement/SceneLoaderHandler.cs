namespace ScenesManagement
{
    using EasyTransition;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class SceneLoaderHandler
    {
        private const string LOADING_SCENE = "Loading";

        public static AsyncOperation LoadingOperation { get; private set; }
        
        private static string _scene;

        private static bool _isLoading;
        
        public static void LoadThroughLoadingScene(string sceneName, TransitionSettings transition)
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            
            _scene = sceneName;
            
            LoadScene(LOADING_SCENE, transition,true);
        }

        public static void LoadTargetScene()
        {
            LoadScene(_scene);

            _isLoading = false;
        }
        
        private static void LoadScene(string sceneName)
        {
            LoadingOperation = SceneManager.LoadSceneAsync(sceneName);

            LoadingOperation.allowSceneActivation = false;
        }
        
        private static void LoadScene(string sceneName,  TransitionSettings transition, bool activateScene)
        {
            LoadingOperation = SceneManager.LoadSceneAsync(sceneName);

            LoadingOperation.allowSceneActivation = false;
            
            SetTransition(transition, activateScene);
        }
        
        public static void SetTransition(TransitionSettings transition, bool activateScene)
        {
            var transitionManager = TransitionManager.Instance();

            if (transitionManager == null)
            {
                return;
            }
            
            transitionManager.Transition(transition, 0);

            if (activateScene == false)
            {
                return;
            }

            if (transitionManager.RunningTransition == false)
            {
                ActivateScene();
                
                return;
            }
            
            transitionManager.OnTransitionCutPointReached += ActivateScene;
        }
        
        private static void ActivateScene()
        {
            var transitionManager = TransitionManager.Instance();

            if (transitionManager != null)
            {
                transitionManager.OnTransitionCutPointReached -= ActivateScene;
            }

            LoadingOperation.allowSceneActivation = true;
        }
    }
}
