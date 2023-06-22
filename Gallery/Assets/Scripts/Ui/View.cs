using UnityEngine;

namespace Ui
{
    using System;
    using ScenesManagement;
    using UnityEngine.UI;

    public class View : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private BackButton backButton;

        [SerializeField]
        private CanvasScaler canvasScaler;
        
        [SerializeField]
        private Vector2Int portraitReferenceResolution = new Vector2Int(1080, 1920);
        
        [SerializeField]
        private Vector2Int landscapeReferenceResolution = new Vector2Int(1920, 1080);
        
        private const string GALLERY_SCENE_NAME = "Gallery";

        private DeviceOrientation _deviceOrientation;

        private void OnEnable()
        {
            image.sprite = SpriteToView.Sprite;
            image.preserveAspect = true;

            Screen.orientation = ScreenOrientation.AutoRotation;
            
            backButton.onClick.AddListener(Back);
        }

        private void OnDisable()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            
            backButton.onClick.RemoveListener(Back);
        }

        private void OnRectTransformDimensionsChange()
        {
            if (Input.deviceOrientation == _deviceOrientation)
            {
                return;
            }

            _deviceOrientation = Input.deviceOrientation;

            switch (_deviceOrientation)
            {
                case DeviceOrientation.Portrait:
                case DeviceOrientation.PortraitUpsideDown:
                    canvasScaler.referenceResolution = portraitReferenceResolution;
                    canvasScaler.matchWidthOrHeight = 0;

                    break;

                case DeviceOrientation.LandscapeLeft:
                case DeviceOrientation.LandscapeRight:
                    canvasScaler.referenceResolution = landscapeReferenceResolution;
                    canvasScaler.matchWidthOrHeight = 1;
                    
                    break;

                case DeviceOrientation.Unknown:
                    break;

                case DeviceOrientation.FaceUp:
                    break;

                case DeviceOrientation.FaceDown:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void Back()
        {
            _ = SceneLoaderHandler.Load(GALLERY_SCENE_NAME);
        }
    }
}
