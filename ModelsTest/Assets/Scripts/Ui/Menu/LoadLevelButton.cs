using UnityEngine;

namespace Ui.Menu
{
    using System;
    using KevinCastejon.MoreAttributes;
    using UnityEngine.UI;

    public class LoadLevelButton : MonoBehaviour
    {
        public event Action<string> OnLoadSceneClick;
        
        [SerializeField]
        private Button button;
        
        [SerializeField, Scene]
        private string scene = "Level1";

        private void OnEnable()
        {
            button.onClick.AddListener(InvokeLoadScene);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(InvokeLoadScene);
        }
        
        private void InvokeLoadScene() => OnLoadSceneClick?.Invoke(scene);
    }
}
