using Main.Misc;
using Main.Player;
using Main.UI;
using Main.UI.Equipment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerController _player;
        private CameraController _cameraController;
        private CanvasHandle _canvasHandle;
        private Camera _camera;
        public Structure sturct;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isDestroying;


        private void Awake()
        {
            _player = GetComponent<PlayerController>();
            _canvasHandle = _player.canvasHandle;
            _camera = Camera.main;
            _cameraController = _camera.GetComponent<CameraController>();
        }

        public void OnInventory(InputAction.CallbackContext callback)
        {
            if (callback.phase != InputActionPhase.Started) return;

            _canvasHandle.ToggleWindow<Inventory>();
        }

        public void OnLeftClick(InputAction.CallbackContext callback)
        {
            if (callback.phase != InputActionPhase.Performed) return;
            if (_canvasHandle.isPointerOverGameObject) return;

            var structure = GetStructureOnMouse();
            if (structure == null) return;

            structure.OpenGUI();
        }

        public void OnRightClick(InputAction.CallbackContext callback)
        {
            if (_player.canvasHandle.isPointerOverGameObject) return;

            if (callback.phase == InputActionPhase.Started)
            {
                var structure = GetStructureOnMouse();
                if (structure == null) return;
                sturct = structure;

                //TODO: 0 Zrobic nieszczenie ze zbieranie zawartosci
                Utils.Log($"Started destroying: {structure.name}");
            }
            else if (callback.phase == InputActionPhase.Performed)
            {
                Utils.Log("Performing");
            }
            else if (callback.phase == InputActionPhase.Canceled)
            {
                Utils.Log("Canceled");
            }
        }

        public void OnScroll(InputAction.CallbackContext callback)
        {
            if (callback.phase == InputActionPhase.Started) return;

            float value = callback.ReadValue<float>();
            _cameraController.ChangeZoom(value);
        }

        private Structure GetStructureOnMouse()
        {
            var raycastHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
            if (!raycastHit.collider) return null;

            var structure = raycastHit.collider.GetComponentInParent<Structure>();
            return structure;
        }
    }
}