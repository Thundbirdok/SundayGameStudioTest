using UnityEngine.UI;

namespace Ui
{
    using UnityEngine;

    public class BackButton : Button
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                onClick?.Invoke();
            }
        }
    }
}
