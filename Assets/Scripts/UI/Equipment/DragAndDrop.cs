using Main.Datas;
using Main.Misc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Main.UI.Equipment
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class DragAndDrop : MonoBehaviour, IPointerClickHandler
    {
        [Header("Components")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Header("References")]
        [SerializeField] private Inventory _inventory;
        [SerializeField] private CanvasHandle _canvasHandle;
        [SerializeField] private Canvas _canvas;

        [Header("Data")]
        [SerializeField] public SlotData _data;

        public SlotData Data { get => _data; set => _data = value; }


        public void Initialize()
        {

        }
        public void SetUp(ItemData data, int quantity)
        {
            _data = new SlotData(data, quantity);
            UpdateVisual();
            gameObject.SetActive(true);
        }
        public void Disable()
        {
            _canvasHandle.isDragging = false;
            gameObject.SetActive(false);
            Clear();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PointerEventData pointerEventData = new(EventSystem.current) { position = Mouse.current.position.ReadValue() };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerEventData, results);

            if (results.Count == 1)
            {
                OnDropItem();
                return;
            }

            for (int i = 0; i < results.Count; i++)
            {
                RaycastResult result = results[i];
                if (!result.gameObject.TryGetComponent<InventorySlot>(out var slot)) continue;
                SlotClick(eventData, slot);
                return;
            }
        }

        private void OnDropItem()
        {
            bool output = _inventory.DropItem(new SlotData(_data.itemData, _data.quantity));
            if (output) Disable();
        }
        private void SlotClick(PointerEventData eventData, InventorySlot slot)
        {
            if (slot.IsEmpty())
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                    DropOnSlot(slot);
                //TODO: 0 zrobic upuszczanie pojedynczo itemy i polowe trzymanego stacka 
            }
            else if (slot.ItemData == _data.itemData)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                    FillSlot(slot);
                else if (eventData.button == PointerEventData.InputButton.Right && Keyboard.current.leftCtrlKey.isPressed)
                    TakeHalfItems(slot);
                else if (eventData.button == PointerEventData.InputButton.Right)
                    TakeOneItem(slot);
            }
            else
            {
                slot.SwapItemsWithDragAndDrop();
            }
        }

        public void StartSimulateDrag()
        {
            StartCoroutine(OnDrag());
        }
        public IEnumerator OnDrag()
        {
            _canvasHandle.isDragging = true;

            while (_canvasHandle.isDragging)
            {
                transform.position = Utils.GetMouseWorldPosition();

                yield return null;
            }
        }

        private void DropOnSlot(InventorySlot slot)
        {
            slot.OnDrop(_data);
            Disable();
        }

        private void TakeOneItem(InventorySlot slot)
        {
            if (_data.quantity != _data.itemData.maxStackSize && slot.GetOneItem()) UpdateQuantity(1);
        }
        private void TakeHalfItems(InventorySlot slot)
        {
            if (slot.GetHalfItems(_data.quantity, out int halfQuantity)) UpdateQuantity(halfQuantity);
        }

        private void FillSlot(InventorySlot slot)
        {
            int need = slot.ItemData.maxStackSize - slot.ItemQuantity;
            int available = (_data.quantity - need < 0) ? _data.quantity : need;

            if (slot.ItemQuantity != slot.ItemData.maxStackSize)
            {
                UpdateQuantity(-available);
                slot.UpdateAmount(available);
            }
        }

        public void UpdateQuantity(int quantity = 0)
        {
            _data.quantity += quantity;
            _amountText.SetText(_data.quantity > 1 ? _data.quantity.ToString() : "");

            if (_data.quantity <= 0) Disable();
        }
        public void UpdateVisual()
        {
            _image.sprite = _data.itemData == null ? null : _data.itemData.icon;
            UpdateQuantity();
            if (_image.sprite == null) Disable();
        }

        public bool IsActive()
        {
            return gameObject.activeSelf;
        }
        public void Clear()
        {
            _amountText.SetText("");
            _image.sprite = null;

            _data.itemData = null;
            _data.quantity = 0;
        }
    }
}