namespace Ui.Common
{
    using UnityEngine;
    using UnityEngine.UI;

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
