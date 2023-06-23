namespace Ui.Common
{
    using UnityEngine;
    using UnityEngine.UI;

    public class BackButton : Button
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                onClick?.Invoke();
            }
        }
    }
}
