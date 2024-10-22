using UnityEngine;

namespace Main.UI.Equipment.SidePanels
{
    public class MiningDrillPanel : MonoBehaviour, ISideInventory
    {
        public bool IsActive => gameObject.activeSelf;


        //[Header("Components")]


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
