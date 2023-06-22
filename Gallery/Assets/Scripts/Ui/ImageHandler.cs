using UnityEngine;

namespace Ui
{
    using ScenesManagement;
    using UnityEngine.UI;
    using Web;

    public class ImageHandler : MonoBehaviour
    {
        private int Number { get; set; } = 1;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Rotator loading;

        [SerializeField]
        private Button button;

        private const string VIEW_SCENE_NAME = "View";

        private string _url;

        public void Initialize(string url,int number)
        {
            Number = number;
            _url = url + Number + ".jpg";

            name = $"Image {Number}";
            
            button.gameObject.SetActive(false);
            
            _ = TextureWebRequest.Get(_url, OnImageReceived, OnError);
        }

        private void OnEnable()
        {
            button.onClick.AddListener(View);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(View);
        }

        private void OnImageReceived(Texture2D texture)
        {
            loading.gameObject.SetActive(false);

            var sprite = Sprite.Create
            (
                texture,
                new Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100.0f
            );
            
            image.sprite = sprite;
            image.preserveAspect = true;
            
            button.gameObject.SetActive(true);
        }

        private void OnError(string error)
        {
            loading.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            
            Debug.Log("Can not get from " + _url + "\n" + error);
        }

        private void View()
        {
            SpriteToView.Sprite = image.sprite;
            _ = SceneLoaderHandler.Load(VIEW_SCENE_NAME);
        }
    }
}
