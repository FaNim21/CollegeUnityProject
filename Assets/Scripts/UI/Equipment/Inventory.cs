using Main.Datas;
using Main.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI.Equipment
{
    [System.Serializable]
    public class SlotData
    {
        public ItemData itemData;
        public int quantity;
        public int slotIndex = -1;

        public SlotData(ItemData itemData = null, int quantity = 0, int slotIndex = -1)
        {
            this.itemData = itemData;
            this.quantity = quantity;
            this.slotIndex = slotIndex;
        }
    }

    public class Inventory : MonoBehaviour, IWindowControl
    {
        public bool IsActive => inventoryObject.activeSelf;


        [Header("References")]
        [SerializeField] private CanvasHandle _canvasHandle;
        public PlayerController player;
        public DragAndDrop dragAndDrop;

        [Header("Components")]
        public Slider destructionProgressSlider;

        [Header("Objects")]
        public GameObject inventoryObject;
        public Transform sidePanelParent;
        private List<ISideInventory> _sideInventories = new();
        private bool _isSidePanelOpen;

        [Header("Slots")]
        public List<InventorySlot> slots = new();


        private void Awake()
        {
            _canvasHandle.AddWindowToControl(this);
            _canvasHandle.AddWindowToEscapeControl(this);

            for (int i = 0; i < sidePanelParent.childCount; i++)
            {
                var child = sidePanelParent.GetChild(i);
                if (child.TryGetComponent<ISideInventory>(out var panel))
                {
                    _sideInventories.Add(panel);
                }
            }
        }
        private void Start()
        {
            dragAndDrop.Initialize();
        }

        public bool DropItem(SlotData slotData)
        {
            return false;
        }

        public bool AddToInventory(ItemData itemToAdd, int amountToAdd)
        {
            for (var i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot.ItemData == null) continue;
                if (slot.ItemData != itemToAdd || slot.ItemQuantity + amountToAdd > itemToAdd.maxStackSize) continue;

                slot.UpdateAmount(amountToAdd);
                return true;
            }

            //if (items.Count >= slots.Count) return false;

            var newData = new SlotData(itemToAdd, amountToAdd);
            //items.Add(newData);

            for (int i = 0; i < slots.Count; i++)
            {
                var currentSlot = slots[i];
                if (currentSlot.ItemData != null) continue;

                currentSlot.AddItem(newData);
                return true;
            }

            return false;
        }

        public InventorySlot GetEmptySlotInQuickBar(SlotData slotData, out bool fullComplete)
        {
            fullComplete = AutoCompleteItems(slots, slotData, 8);
            if (slotData.quantity == 0) return null;

            for (int i = 0; i < 8; i++)
            {
                var current = slots[i];
                if (current.ItemData == null) return current;
            }
            return null;
        }
        public InventorySlot GetEmptySlotInEq(SlotData slotData, out bool fullComplete)
        {
            fullComplete = AutoCompleteItems(slots, slotData, slots.Count, 8);
            if (slotData.quantity == 0) return null;

            for (int i = 8; i < slots.Count; i++)
            {
                var current = slots[i];
                if (current.ItemData == null) return current;
            }
            return null;
        }

        private bool AutoCompleteItems(List<InventorySlot> collection, SlotData oldSlotData, int size = -1, int index = 0)
        {
            int count = size == -1 ? collection.Count : size;
            for (int i = index; i < count; i++)
            {
                var current = collection[i];

                if (current.ItemData == oldSlotData.itemData)
                {
                    int need = current.ItemData.maxStackSize - current.ItemQuantity;
                    int available = (oldSlotData.quantity - need < 0) ? oldSlotData.quantity : need;

                    if (current.ItemQuantity != current.ItemData.maxStackSize)
                    {
                        current.UpdateAmount(available);
                        oldSlotData.quantity -= available;
                    }
                }
            }
            if (oldSlotData.quantity == 0) return true;
            return false;
        }

        public void OpenSidePanel<T>()
        {
            for (int i = 0; i < _sideInventories.Count; i++)
            {
                var current = _sideInventories[i];
                if (current is T && !current.IsActive)
                {
                    _isSidePanelOpen = false;
                    current.ToggleWindow();
                    return;
                }
            }
        }

        public void OpenWindow()
        {
            inventoryObject.SetActive(true);
        }
        public void ToggleWindow()
        {
            if(!IsActive) player.canvasHandle.CloseUIWindow(false);
            inventoryObject.SetActive(!IsActive);

            if (_isSidePanelOpen)
            {
                //TODO: 0 zamykac boczny panel
                _isSidePanelOpen = false;
            }
        }
    }
}
