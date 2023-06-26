using UnityEngine;

namespace Ui.Common
{
    public class CarInputs : MonoBehaviour
    {
        private void Awake()
        {
            #if UNITY_ANDROID || UNITY_IOS
            
            gameObject.SetActive(true);
            
            #else
            
            gameObject.SetActive(false);
            
            #endif
        }
    }
}
