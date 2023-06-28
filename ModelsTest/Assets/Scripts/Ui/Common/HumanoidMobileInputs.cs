namespace Ui.Common
{
    using System;
    using UnityEngine;

    [Serializable]
    public class HumanoidMobileInputs
    {
        [SerializeField]
        private GameObject gameObject;
        
        public void Initialize()
        {
            
#if UNITY_ANDROID || UNITY_IOS
            
            gameObject.SetActive(true);
            
#else
            
            gameObject.SetActive(false);
            
#endif
            
        }
        
        public void Deinitialize()
        {
            gameObject.SetActive(false);
        }
    }
}
