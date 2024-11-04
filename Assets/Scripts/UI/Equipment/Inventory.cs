using Main.Buildings;
using Main.Datas;
using Main.Misc;
using Main.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main.UI.Equipment
{
    [Serializable]
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
        public SlotData(SlotData slotData)
        {
            itemData = slotData.itemData;
            quantity = slotData.quantity;
            slotIndex = slotData.slotIndex;
        }

        public bool IsEmpty()
        {
            return itemData == null;
        }
    }

    public class Inventory : MonoBehaviour, IWindowControl
    {
        public bool IsActive => background.activeSelf;


        [Header("References")]
        [SerializeField] private CanvasHandle _canvasHandle;
        public PlayerController player;
        public DragAndDrop dragAndDrop;

        [Header("Objects")]
        public GameObject background;
        public GameObject craftingBackground;
        public Transform sidePanelParent;
        private List<ISideInventory> _sideInventories = new();

        [Header("Slots")]
        public int slotCount;
        public int quickbarSlotsCount = 5;
        public List<InventorySlot> slots = new();

        [Header("Debug")]
        public bool isWindowOpened;
        [SerializeField, ReadOnly] private ISideInventory _openedSideInventory;
        [SerializeField, ReadOnly] private Guid _openedStructureGuid;


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

            for (int i = 0; i < slotCount; i++)
            {
                var slot = Instantiate(GameManager.instance.inventorySlot, background.transform);
                slot.index = i;
                slots.Add(slot);
            }
        }
        private void Start()
        {
            dragAndDrop.Initialize();
        }

        public bool DropItem(SlotData slotData)
        {
            //Wyrzucac item na ziemie
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
            fullComplete = AutoCompleteItems(slots, slotData, quickbarSlotsCount);
            if (slotData.quantity == 0) return null;

            for (int i = 0; i < quickbarSlotsCount; i++)
            {
                var current = slots[i];
                if (current.ItemData == null) return current;
            }
            return null;
        }
        public InventorySlot GetEmptySlotInEq(SlotData slotData, out bool fullComplete)
        {
            fullComplete = AutoCompleteItems(slots, slotData, slots.Count, quickbarSlotsCount);
            if (slotData.quantity == 0) return null;

            for (int i = quickbarSlotsCount; i < slots.Count; i++)
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

        public void OpenSidePanel<T>(Structure structure)
        {
            craftingBackground.SetActive(false);
            for (int i = 0; i < _sideInventories.Count; i++)
            {
                var current = _sideInventories[i];
                //TODO: 0 zrobic zeby mozna bylo otwierac inne struktury, ale tego samego typu
                //if (_openedSideInventory != null && _openedSideInventory is not T) return;
                if (current is T)
                {
                    OpenWindow();
                    current.OpenWindow(structure);
                    if (_openedSideInventory != null)
                    {
                        _openedSideInventory.CloseWindow();
                    }
                    _openedSideInventory = current;
                    _openedStructureGuid = structure.guid;
                    return;
                }
            }
        }

        public void OpenInventory()
        {
            craftingBackground.SetActive(true);
            OpenWindow();
        }
        public void OpenWindow()
        {
            isWindowOpened = true;
            background.SetActive(true);
        }
        public void ToggleWindow()
        {
            if (!IsActive)
            {
                player.canvasHandle.CloseUIWindow(false);
                OpenInventory();
                return;
            }
            CloseWindow();
        }
        public void CloseWindow()
        {
            isWindowOpened = false;
            background.SetActive(false);
            craftingBackground.SetActive(false);

            if (_openedSideInventory != null)
            {
                _openedSideInventory.CloseWindow();
                _openedSideInventory = null;
                _openedStructureGuid = Guid.Empty;
            }
        }

        public bool IsStructureWindowOpened(Guid guid)
        {
            //Utils.Log($"guid1: {guid}, guid2: {_openedStructureGuid}");
            return guid.Equals(_openedStructureGuid);
        }
    }
}
