using Main.Buildings;
using Main.Datas;
using Main.Misc;
using Main.UI;
using Main.UI.Equipment;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Main.Player
{
    public class InputHandler : MonoBehaviour
    {
        private enum ObtainType
        {
            Structure,
            Ore
        }

        private PlayerController _player;
        private CameraController _cameraController;
        private CanvasHandle _canvasHandle;
        private MapManager _mapManager;
        private StructureBuilder _structureBuilder;
        private Inventory _inventory;
        private Camera _camera;

        public Slider obtainingSlider;

        [Header("Debug")]
        [SerializeField, ReadOnly] private ObtainType _obtainType;
        [SerializeField, ReadOnly] private Structure _obtainingStructure;
        [SerializeField, ReadOnly] private OreType _obtainingOre;
        [SerializeField, ReadOnly] private Vector3Int _obtainingOrePosition;
        [SerializeField, ReadOnly] private bool _isObtaining;
        [SerializeField, ReadOnly] private float _obtainingTimeAmount;
        [SerializeField, ReadOnly] private float _obtainingTimer;


        private void Start()
        {
            _player = GetComponent<PlayerController>();
            _canvasHandle = _player.canvasHandle;
            _camera = Camera.main;
            _cameraController = _camera.GetComponent<CameraController>();
            _mapManager = GameManager.instance.mapManager;
            _inventory = _player.inventory;
            _structureBuilder = _inventory.builder;
        }
        private void Update()
        {
            if (!_isObtaining) return;

            _obtainingTimer += Time.deltaTime;
            obtainingSlider.value = _obtainingTimer / _obtainingTimeAmount; 

            if (_obtainType == ObtainType.Structure)
            {
                var structure = GetStructureOnMouse();
                if (structure == null || !structure.Equals(_obtainingStructure))
                {
                    RestartObtaining();
                }
            }
            else if (_obtainType == ObtainType.Ore)
            {
                Vector2 mousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                var currentTilePosition = _mapManager.GetTilePosition(mousePosition);
                if (currentTilePosition != _obtainingOrePosition)
                {
                    Utils.Log($"ore1: {_obtainingOrePosition} -- ore2: {currentTilePosition}");
                    RestartObtaining();
                }
            }

            if (_obtainingTimer >= _obtainingTimeAmount)
            {
                Obtain();
            }
        }

        private void Obtain()
        {
            if (_obtainType == ObtainType.Structure)
            {
                _obtainingStructure.OnCollect(_inventory);
                _mapManager.RemoveStructure(_obtainingStructure);
            }
            else if (_obtainType == ObtainType.Ore)
            {
                ItemData data = GameManager.instance.GetItemData(_obtainingOre.ToString());
                _inventory.AddToInventory(data, 1);
            }

            RestartObtaining();
        }

        public void OnInventory(InputAction.CallbackContext callback)
        {
            if (_canvasHandle.isCanvasEnabled) return;
            if (callback.phase != InputActionPhase.Started) return;
            if (_structureBuilder.IsInBuildMode()) return;

            _canvasHandle.ToggleWindow<Inventory>();
        }

        public void OnLeftClick(InputAction.CallbackContext callback)
        {
            if (_canvasHandle.isCanvasEnabled) return;
            if (callback.phase != InputActionPhase.Performed) return;
            if (_canvasHandle.isPointerOverGameObject) return;

            if (_structureBuilder.IsPlacing() && _structureBuilder.IsInBuildMode())
            {
                //TODO: 0 check for collisiion with other structures
                _structureBuilder.Place();
                return;
            }

            if (_structureBuilder.IsInBuildMode()) return;

            var structure = GetStructureOnMouse();
            if (structure == null) return;

            structure.OpenGUI();
        }

        public void OnRightClick(InputAction.CallbackContext callback)
        {
            if (_canvasHandle.isCanvasEnabled) return;
            if (_player.canvasHandle.isPointerOverGameObject) return;
            if (_structureBuilder.IsInBuildMode()) return;

            if (callback.phase == InputActionPhase.Started)
            {
                RestartObtaining();

                var structure = GetStructureOnMouse();
                if (structure != null)
                {
                    _obtainType = ObtainType.Structure;
                    _obtainingStructure = structure;
                    _obtainingTimeAmount = 0.5f;

                    StartObtaining();
                    Utils.Log($"Started obtaining structure: {structure.name}");
                    return;
                }

                Vector2 mousePosition = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                var ore = _mapManager.GetOreOnTile(mousePosition);
                if (ore != OreType.None)
                {
                    _obtainType = ObtainType.Ore;
                    _obtainingOre = ore;
                    _obtainingOrePosition = _mapManager.GetTilePosition(mousePosition);
                    _obtainingTimeAmount = 2.5f;

                    StartObtaining();
                    Utils.Log($"Started obtaining structure: {_obtainingOre}");
                    return;
                }

                Utils.Log("Couldnt obtain anything");
            }
            else if (callback.phase == InputActionPhase.Canceled)
            {
                Utils.Log("Canceled obtaining");
                RestartObtaining();
            }
        }

        public void OnScroll(InputAction.CallbackContext callback)
        {
            if (_canvasHandle.isCanvasEnabled) return;
            if (callback.phase == InputActionPhase.Started) return;

            float value = callback.ReadValue<float>();
            _cameraController.ChangeZoom(value);
        }

        public void OnChangeBuildMode(InputAction.CallbackContext callback)
        {
            if (_canvasHandle.isCanvasEnabled) return;
            if (callback.phase != InputActionPhase.Performed) return;

            _structureBuilder.SwitchBuildMode();

            if (_structureBuilder.IsInBuildMode())
            {
                _inventory.dragAndDrop.Cancel();
                _inventory.CloseWindow();
                RestartObtaining();
            }
        }

        public void OnEscape(InputAction.CallbackContext callback)
        {
            if (callback.phase != InputActionPhase.Performed) return;

            _canvasHandle.ToggleWindow<PauseMenu>();
        }

        private Structure GetStructureOnMouse()
        {
            var raycastHit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
            if (!raycastHit.collider) return null;

            var structure = raycastHit.collider.GetComponentInParent<Structure>();
            if (structure == null) return null;
            if (!structure.canBeObtained) return null;
            return structure;
        }

        private void StartObtaining()
        {
            _isObtaining = true;
            obtainingSlider.gameObject.SetActive(true);
        }
        private void RestartObtaining()
        {
            _isObtaining = false;
            _obtainingStructure = null;
            _obtainingTimer = 0;
            obtainingSlider.gameObject.SetActive(false);
        }
    }
}