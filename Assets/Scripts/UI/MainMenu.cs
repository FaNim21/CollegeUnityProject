using UnityEngine;

namespace Main.UI
{
    public class MainMenu : MonoBehaviour, IWindowControl
    {
        public bool IsActive => true;

        public void CloseWindow()
        {
            throw new System.NotImplementedException();
        }

        public void OpenWindow()
        {
            throw new System.NotImplementedException();
        }
    }
}
