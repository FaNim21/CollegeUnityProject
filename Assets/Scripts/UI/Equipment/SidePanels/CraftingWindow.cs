using UnityEngine;

namespace Main.UI.Equipment.SidePanels
{
    public class CraftingWindow : MonoBehaviour, ISideInventory
    {
        public bool IsActive => gameObject.activeSelf;

        [Header("Components")]
        [SerializeField] private CanvasHandle _canvasHandle;


        private void Awake()
        {
            _canvasHandle.AddWindowToControl(this);
        }

        public void ToggleWindow()
        {
            gameObject.SetActive(!IsActive);
        }
    }
}
