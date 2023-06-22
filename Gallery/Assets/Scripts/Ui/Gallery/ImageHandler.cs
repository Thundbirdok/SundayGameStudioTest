namespace Ui.Gallery
{
    using System;
    using DG.Tweening;
    using Ui.Effects;
    using UnityEngine;
    using UnityEngine.UI;
    using Web;

    public class ImageHandler : MonoBehaviour
    {
        public event Action<Sprite> OnImageSelected;
        
        private int Number { get; set; } = 1;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Rotator loading;

        [SerializeField]
        private Image error;
        
        [SerializeField]
        private Button button;

        [SerializeField]
        private float tweenTime = 0.25f;
        
        private string _url;

        private Tween _showImage;
        private Tween _showLoading;
        private Tween _showError;

        public void Initialize(string url,int number)
        {
            Number = number;
            _url = url + Number + ".jpg";

            name = $"Image {Number}";

            image.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            error.gameObject.SetActive(false);
            loading.gameObject.SetActive(true);

            SetTweens();
            
            _showLoading.PlayForward();
            
            _ = TextureWebRequest.Get(_url, OnImageReceived, OnError);
        }

        private void OnEnable() => button.onClick.AddListener(ToView);

        private void OnDisable() => button.onClick.RemoveListener(ToView);

        private void OnDestroy() => KillTweens();

        private void OnImageReceived(Texture2D texture)
        {
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
            image.gameObject.SetActive(true);

            _showLoading.OnRewind(ShowImage).PlayBackwards();
        }

        private void OnError(string errorMessage)
        {
            image.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
            error.gameObject.SetActive(true);

            _showLoading.OnRewind(ShowError).PlayBackwards();

            Debug.Log("Can not get from " + _url + "\n" + errorMessage);
        }

        private void ToView() => OnImageSelected?.Invoke(image.sprite);

        private void SetTweens()
        {
            var loadingRectTransform = (loading.transform as RectTransform);
            _showLoading ??= loadingRectTransform.DOScale(1.0f, tweenTime).From(0).SetAutoKill(false);
            _showImage ??= image.rectTransform.DOScale(1.0f, tweenTime).From(0).SetAutoKill(false);
            _showError ??= error.rectTransform.DOScale(1.0f, tweenTime).From(0).SetAutoKill(false);
        }

        private void KillTweens()
        {
            _showLoading.Kill();
            _showImage.Kill();
            _showError.Kill();
        }

        private void ShowError()
        {
            loading.gameObject.SetActive(false);
            _showError.PlayForward();
        }

        private void ShowImage()
        {
            loading.gameObject.SetActive(false);
            _showImage.PlayForward();
        }
    }
}
