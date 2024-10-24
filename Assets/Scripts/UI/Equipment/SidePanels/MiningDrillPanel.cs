using UnityEngine;
using UnityEngine.UI;

namespace Main.UI.Equipment.SidePanels
{
    public class MiningDrillPanel : MonoBehaviour, ISideInventory
    {
        private MiningDrill _drill;

        [Header("Components")]
        [SerializeField] private InventorySlot _fuelSlot;
        [SerializeField] private InventorySlot _outputSlot;
        [SerializeField] private Slider _mineProgressSlider;
        [SerializeField] private Slider _fuelProgressSlider;


        private void LoadData(MiningDrill drill)
        {
            _drill = drill;

            //_fuelSlot.AddItem(null);
        }

        private void Clear()
        {
            _mineProgressSlider.value = 0;
            _fuelProgressSlider.value = 1;
        }

        public void OpenWindow(Structure structure)
        {
            LoadData((MiningDrill)structure);
            gameObject.SetActive(true);
        }
        public void CloseWindow()
        {
            gameObject.SetActive(false);
            Clear();
        }

        public void OpenWindow()
        {
            throw new System.NotImplementedException();
        }
    }
}
