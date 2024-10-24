using UnityEngine;

namespace Main.UI.Equipment
{
    public class CraftingWindow : MonoBehaviour, IWindowControl
    {
        public bool IsActive => gameObject.activeSelf;

        [Header("Components")]
        [SerializeField] private CanvasHandle _canvasHandle;


        private void Awake()
        {
            _canvasHandle.AddWindowToControl(this);
        }


        public void OpenWindow()
        {
            gameObject.SetActive(true);
        }
        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
