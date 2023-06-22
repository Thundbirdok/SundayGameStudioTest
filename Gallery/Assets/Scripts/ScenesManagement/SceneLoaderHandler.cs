namespace ScenesManagement
{
    using System.Threading.Tasks;
    using UnityEngine.SceneManagement;

    public static class SceneLoaderHandler
    {
        public static string Scene { get; private set; }

        private const string LOADING_SCENE = "Loading";
        
        public async static Task Load(string sceneName)
        {
            Scene = sceneName;
            
            var operation = SceneManager.LoadSceneAsync(LOADING_SCENE);

            while (operation.isDone == false)
            {
                await Task.Yield();
            }
        }
    }
}
