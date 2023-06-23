using UnityEngine;

namespace Ui.Menu
{
    using DG.Tweening;
    using UnityEngine.UI;

    public class SettingsUi : MonoBehaviour
    {
        [SerializeField]
        private Button openClose;

        [SerializeField]
        private GameObject settings;
        
        [SerializeField]
        private PopupMover settingsMover;

        private bool _isOpen;
        
        private bool _isInitialized;
        
        private Sequence _sequence;
        
        private void OnEnable()
        {
            settings.SetActive(false);
            
            openClose.onClick.AddListener(OpenClose);
        }

        private void OnDisable()
        {
            openClose.onClick.RemoveListener(OpenClose);
        }

        private void OpenClose()
        {
            Initialize();
            
            if (_isOpen)
            {
                Close();
                
                return;
            }

            Open();
        }

        private void Open()
        {
            _isOpen = true;
            
            settings.SetActive(true);

            _sequence.PlayForward();
        }

        private void Close()
        {
            _isOpen = false;
            
            _sequence.PlayBackwards();
        }
        
        private void Initialize()
        {
            if (_isInitialized)
            {
                return;   
            }

            settingsMover.Initialize();

            SetSequence();
            
            _isInitialized = true;
        }
        
        private void OnSequenceComplete()
        {
            if (_isOpen == false)
            {
                settings.SetActive(false);
            }
        }

        private void SetSequence()
        {
            _sequence = DOTween.Sequence();
            
            _sequence
                .Append(settingsMover.Tween)
                .OnPause(OnSequenceComplete);
            
            _sequence.SetAutoKill(false);
            _sequence.Pause();
        }
    }
}
