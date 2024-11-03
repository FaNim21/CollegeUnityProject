using Main.Datas;
using Main.Misc;
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
        private SlotHandler _slotHandler;

        [Header("Values")]
        public int index;

        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _amountText;

        [Header("Data")]
        [SerializeField] private SlotData _data;

        public SlotData Data { get => _data; }

        public ItemData ItemData
        {
            get => _data.itemData;
            private set => _data.itemData = value;
        }
        public int ItemQuantity
        {
            get => _data.quantity;
            private set => _data.quantity = value;
        }
        public bool SlotHandlerExist
        {
            get => _slotHandler != null;
        }


        protected override void Start() 
        {
            _inventory = GameManager.instance.playerController.inventory;

            UpdateSlot();
        }
        public void SetSlotHandler(SlotHandler slotHandler)
        {
            _slotHandler = slotHandler;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerClickHandler);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
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
            StartDragAndDrop(_data.itemData, _data.quantity);
        }

        public void OnDrop(SlotData slotData, int quantity = -1)
        {
            _data = new SlotData(slotData.itemData, quantity == -1 ? slotData.quantity : quantity, index);
            UpdateSlot();
        }

        private void TakeHalfItems()
        {
            //TODO: 0 problem z braniem polowy itemow poniewaz zmienia sie wartosc dziwnie?
            int halfQuantity = Mathf.Max(1, ItemQuantity / 2);
            ItemData data = ItemData;
            UpdateAmount(-halfQuantity);
            StartDragAndDrop(data, halfQuantity, false);
        }

        public bool GetOneItem()
        {
            if (ItemQuantity == 0) return false;

            UpdateAmount(-1);
            if (ItemQuantity == 0) Clear();
            return true;
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
            _data = newData;
            if (SlotHandlerExist)
            {
                //TODO: 0 trzeba zrobic wykrywanie, ktora baze na slocie czyscie wiec moze jednak zrobie structure slot?
                //_structure.
            }
            UpdateSlot();
        }

        public bool IsEmpty()
        {
            return ItemData == null;
        }

        private void StartDragAndDrop(ItemData data, int quantity, bool clearing = true)
        {
            _inventory.dragAndDrop.SetUp(data, quantity);
            if (clearing) DeepClear();
            _inventory.dragAndDrop.StartSimulateDrag();
        }

        public void SwapItems(InventorySlot slot)
        {
            if (slot.IsEmpty() && IsEmpty()) return;

            (_data, slot._data) = (slot._data, _data);

            UpdateSlot();
            slot.UpdateSlot();
        }
        public void SwapItemsWithDragAndDrop()
        {
            if (!_inventory.dragAndDrop.IsActive() || IsEmpty()) return;

            (_data, _inventory.dragAndDrop.Data) = (_inventory.dragAndDrop.Data, _data);

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
                    emptySlot = _inventory.GetEmptySlotInEq(_data, out onlyAutoCompleted);
                    break;
                case "Background":
                    emptySlot = _inventory.GetEmptySlotInQuickBar(_data, out onlyAutoCompleted);
                    break;
            }

            if (emptySlot == null && onlyAutoCompleted)
            {
                Clear();
                return;
            }
            else if (emptySlot == null) return;

            emptySlot.OnDrop(_data);
            Clear();
        }

        public void Clear()
        {
            if (_data.itemData == null) return;

            Utils.Log($"Clear {_data.itemData.name}");
            _data = null;
            _data = new SlotData();

            UpdateSlot();
        }

        public void DeepClear()
        {
            if (_data.itemData == null) return;

            Utils.Log($"Deep clear {_data.itemData.name}");
            _data.itemData = null;
            _data.quantity = 0;

            UpdateSlot();
        }
    }
}
