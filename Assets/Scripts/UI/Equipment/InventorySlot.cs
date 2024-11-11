using Main.Datas;
using Main.Misc;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Main.UI.Equipment
{
    public class InventorySlot : Slot
    {
        private Inventory _inventory;

        [Header("Values")]
        public int index;

        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private GameObject _selectedImage;

        [Header("Data")]
        public SlotHandler slotHandler;  //tak mi tez sie nie podoba ze to jest public xd ale mam malo czasu na zabawe przy bezpieczenstwie i czytelnosci kodu :d

        public SlotData Data { get => slotHandler.data; set => slotHandler.data = value; }

        public ItemData ItemData
        {
            get => Data.itemData;
            private set => Data.itemData = value;
        }
        public int ItemQuantity
        {
            get => Data.quantity;
            private set => Data.quantity = value;
        }


        protected override void Start() 
        {
            _inventory = GameManager.instance.playerController.inventory;
            slotHandler.Setup(this);

            UpdateSlot();
        }
        public void SetSlotHandler(SlotHandler slotHandler)
        {
            this.slotHandler = slotHandler;
            this.slotHandler.Setup(this);

            UpdateSlot();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_inventory.builder.IsInBuildMode()) return;
            if (eventData.pointerEnter == null || IsEmpty()) return;

            if (Keyboard.current.leftShiftKey.isPressed && eventData.button == PointerEventData.InputButton.Left)
            {
                SwapItemToOtherInventory();
                return;
            }
            if (Keyboard.current.leftShiftKey.isPressed) return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                TakeHalfItems();
                return;
            }

            if (ItemData == null && _inventory.dragAndDrop.gameObject.activeSelf) return;
            StartDragAndDrop(Data.itemData, Data.quantity);
        }

        public void OnDrop(SlotData slotData, int quantity = -1)
        {
            Data = new SlotData(slotData.itemData, quantity == -1 ? slotData.quantity : quantity, index);
            UpdateSlot();
        }

        private void TakeHalfItems()
        {
            int halfQuantity = Mathf.Max(1, ItemQuantity / 2);
            ItemData data = ItemData;
            UpdateAmount(-halfQuantity);
            StartDragAndDrop(data, halfQuantity, false);
        }

        public void UpdateSlot()
        {
            _image.sprite = ItemData == null ? null : ItemData.icon;
            _image.gameObject.SetActive(_image.sprite != null);
            UpdateAmount();
        }
        public void UpdateAmount(int amount = 0)
        {
            ItemQuantity += amount;
            _amountText.SetText(ItemQuantity > 1 ? ItemQuantity.ToString() : "");

            if (ItemQuantity <= 0) Clear();
        }

        public void AddItem(SlotData newData)
        {
            Data = newData;
            UpdateSlot();
        }

        public bool IsEmpty()
        {
            return ItemData == null;
        }

        private void StartDragAndDrop(ItemData data, int quantity, bool clearing = true)
        {
            _inventory.dragAndDrop.SetUp(data, quantity, this);
            if (clearing) DeepClear();
            _inventory.dragAndDrop.StartSimulateDrag();
        }

        public void SwapItems(InventorySlot slot)
        {
            if (slot.IsEmpty() && IsEmpty()) return;

            slotHandler.Swap(slot.slotHandler);

            UpdateSlot();
            slot.UpdateSlot();
        }
        public void SwapItemsWithDragAndDrop()
        {
            if (!_inventory.dragAndDrop.IsActive() || IsEmpty()) return;

            (Data, _inventory.dragAndDrop.Data) = (_inventory.dragAndDrop.Data, Data);

            UpdateSlot();
            _inventory.dragAndDrop.UpdateVisual();
        }
        private void SwapItemToOtherInventory()
        {
            if (!_inventory.isWindowOpened) return;

            InventorySlot emptySlot = null;
            string currentSlotPositon = transform.parent.name;
            bool onlyAutoCompleted = false;

            switch (currentSlotPositon)
            {
                case "QuickBar":
                    emptySlot = _inventory.GetEmptySlotInEq(Data, out onlyAutoCompleted);
                    break;
                case "Background":
                    emptySlot = _inventory.GetEmptySlotInQuickBar(Data, out onlyAutoCompleted);
                    break;
            }

            if (emptySlot == null && onlyAutoCompleted)
            {
                Clear();
                return;
            }
            else if (emptySlot == null) return;

            emptySlot.OnDrop(Data);
            Clear();
        }

        public bool IsCorrectWindowOpened(Guid guid)
        {
            if (!_inventory.isWindowOpened) return false;
            return _inventory.IsStructureWindowOpened(guid);
        }

        public void Clear()
        {
            if (slotHandler.data.itemData == null) return;

            slotHandler.Clear();
            UpdateSlot();
        }

        public void DeepClear()
        {
            if (slotHandler.data.itemData == null) return;

            slotHandler.DeepClear();
            UpdateSlot();
        }

        public void Select()
        {
            _selectedImage.SetActive(true);
        }
        public void UnSelect()
        {
            _selectedImage.SetActive(false);
        }
    }
}
