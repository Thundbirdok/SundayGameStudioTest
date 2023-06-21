namespace Web
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class TextureWebRequest
    {
        public async static Task Get
        (
            string url,
            Action<Texture2D> onSuccess,
            Action<string> onError
        )
        {
            using var unityWebRequest = UnityWebRequestTexture.GetTexture(url);

            var request = unityWebRequest.SendWebRequest();

            while (request.isDone == false)
            {
                if (Application.isPlaying == false)
                {
                    unityWebRequest.Abort();
                    
                    return;
                }

                await Task.Yield();
            }

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(unityWebRequest.error);
            }

            if (unityWebRequest.downloadHandler is DownloadHandlerTexture textureHandler)
            {
                onSuccess?.Invoke(textureHandler.texture);
            }
        }
    }
}
