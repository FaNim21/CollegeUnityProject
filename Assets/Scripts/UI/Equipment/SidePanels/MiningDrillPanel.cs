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


        private void Start()
        {
        }
        private void Update()
        {
            UpdateDrillInformations();
        }

        private void UpdateDrillInformations()
        {
            _mineProgressSlider.value = _drill.MineProgress;
            _fuelProgressSlider.value = _drill.FuelProgress;
        }

        private void LoadData(MiningDrill drill)
        {
            _drill = drill;
            _fuelSlot.SetSlotHandler(_drill.fuelHandler);
            _outputSlot.SetSlotHandler(_drill.outputHandler);
        }

        private void Clear()
        {
            _mineProgressSlider.value = 0;
            _fuelProgressSlider.value = 1;
            _drill = null;
        }

        public void OpenWindow(Structure structure)
        {
            Clear();

            LoadData((MiningDrill)structure);
            gameObject.SetActive(true);
        }
        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
