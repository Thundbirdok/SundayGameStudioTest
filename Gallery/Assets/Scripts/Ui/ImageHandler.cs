using UnityEngine;

namespace Ui
{
    using UnityEngine.UI;
    using Web;

    public class ImageHandler : MonoBehaviour
    {
        [SerializeField]
        private int number = 1;

        [SerializeField]
        private RawImage image;

        [SerializeField]
        private Rotator loading;
        
        private string _url = "http://data.ikppbb.com/test-task-unity-data/pics/";
        
        private void Start() => TextureWebRequest.Get(GetUrl(), OnImageReceived, OnError);

        private void OnImageReceived(Texture2D texture)
        {
            loading.gameObject.SetActive(false);
            
            image.texture = texture;
        }

        private void OnError(string error)
        {
            loading.gameObject.SetActive(false);
            
            Debug.Log("Can not get from " + GetUrl() + "\n" + error);
        }

        private string GetUrl() => _url + number + ".jpg";
    }
}
