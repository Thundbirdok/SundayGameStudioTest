using UnityEngine;

namespace Ui.Common
{
    using System;

    [Serializable]
    public class CarMobileInputs
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
    }
}
