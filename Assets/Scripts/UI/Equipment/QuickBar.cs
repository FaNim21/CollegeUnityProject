using Main.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Main.UI.Equipment
{
    public class QuickBar : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Inventory _inventory;


        public void SetSelection(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                int choice = int.Parse(context.control.name) - 1;

                //TODO: 1 Tu musi byc wiadome to czy jak myszka jest na slocie to zeby zrobic zamiane, a po za slotem zaznaczanie przez cyfre
                if (player.canvasHandle.isPointerOverGameObject)
                {
                    if (_inventory.dragAndDrop.gameObject.activeSelf) return;

                    PointerEventData pointerEventData = new(EventSystem.current) { position = Mouse.current.position.ReadValue() };
                    List<RaycastResult> results = new();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    for (int i = 0; i < results.Count; i++)
                    {
                        if (!results[i].gameObject.TryGetComponent<InventorySlot>(out var slot)) continue;

                        _inventory.slots[choice].SwapItems(slot);
                        break;
                    }
                }
                else
                {
                    _inventory.SelectSlot(choice);
                }
            }
        }
    }
}
