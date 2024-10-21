using UnityEngine;

namespace Main.UI.Equipment.SidePanels
{
    public class MiningDrillPanel : MonoBehaviour, ISideInventory
    {
        public bool IsActive => gameObject.activeSelf;

        //[Header("Components")]


        public void ToggleWindow()
        {
            gameObject.SetActive(!IsActive);
        }
    }
}
