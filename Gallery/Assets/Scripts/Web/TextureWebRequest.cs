namespace Web
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class TextureWebRequest
    {
        private class WebRequestMonoBehaviour : MonoBehaviour {}
        
        private static WebRequestMonoBehaviour _webRequestMonoBehaviour;

        private static void Init()
        {
            if (_webRequestMonoBehaviour != null)
            {
                return;
            }

            var gameObject = new GameObject(nameof(WebRequestMonoBehaviour));
            _webRequestMonoBehaviour = gameObject.AddComponent<WebRequestMonoBehaviour>();
        }

        public static void Get
        (
            string url,
            Action<Texture2D> onSuccess,
            Action<string> onError
        )
        {
            Init();
            _webRequestMonoBehaviour.StartCoroutine(GetCoroutine(url, onSuccess, onError));
        }

        private static IEnumerator GetCoroutine
        (
            string url,
            Action<Texture2D> onSuccess,
            Action<string> onError
        )
        {
            using var unityWebRequest = UnityWebRequestTexture.GetTexture(url);

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(unityWebRequest.error);
                
                yield break;
            }

            if (unityWebRequest.downloadHandler is DownloadHandlerTexture textureHandler)
            {
                onSuccess?.Invoke(textureHandler.texture);
            }
        }
    }
}
