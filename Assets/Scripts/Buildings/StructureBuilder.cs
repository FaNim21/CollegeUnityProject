using Main.UI.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Main.Buildings
{
    public class StructureBuilder : MonoBehaviour
    {
        private MapManager _mapManager;
        private Camera _camera;

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _builderModeText;
        [SerializeField] private Image _builderModeBackgroundImage;

        [Header("Values")]
        [SerializeField] private Vector2 _offset;

        [Header("Debug")]
        [SerializeField, ReadOnly] private bool _isStructureSpawned;
        [SerializeField, ReadOnly] private bool _isPlacing;
        [SerializeField, ReadOnly] private bool _isInBuildMode;
        [SerializeField, ReadOnly] private Structure _structure;
        [SerializeField, ReadOnly] private SlotHandler _slotHandler;
        [SerializeField, ReadOnly] private Vector2 _previousPosition;


        private void Start()
        {
            _mapManager = GetComponent<MapManager>();
            _camera = Camera.main;
        }

        public void Initialize(SlotHandler slotHandler)
        {
            _slotHandler = slotHandler;

            if (_isInBuildMode)
            {
                SpawnStructure();
            }

            _isPlacing = true;
        }

        private void Update()
        {
            if (!_isPlacing || !_isInBuildMode) return;

            Vector2 mousePosition = (Vector2)_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + _offset;
            mousePosition = new Vector2(Mathf.RoundToInt(mousePosition.x), Mathf.RoundToInt(mousePosition.y)) + _offset;

            if (mousePosition != _previousPosition)
            {
                bool overlapping = _mapManager.CheckForStructure(mousePosition - _structure.placementPosition, _structure.trueSize);
                if (overlapping) return;
            }

            _structure.transform.position = mousePosition - _structure.placementPosition;
            _previousPosition = mousePosition;
        }

        private void SpawnStructure()
        {
            if (_slotHandler == null || _slotHandler.data.itemData == null || _slotHandler.data.itemData.type != Datas.ItemType.Structure) return;

            _structure = Instantiate(_slotHandler.data.itemData.structure, new Vector2(500, 500), Quaternion.identity);
            _structure.EnterPlacementMode();
            _isStructureSpawned = true;
            _isPlacing = true;
            _previousPosition = Vector2.zero;
        }

        public void Place()
        {
            if (!_mapManager.IsInBounds(_structure.transform.position, Vector2.zero)) return;

            bool stillExists = _slotHandler.UpdateAmount(-1, false);

            _isPlacing = false;
            _isStructureSpawned = false;

            _structure.ExitPlacementMode();
            _mapManager.AddStructure(_structure);
            _structure = null;

            if (stillExists)
            {
                Initialize(_slotHandler);
            }
        }

        public void SwitchBuildMode()
        {
            _isInBuildMode = !_isInBuildMode;

            string mode = "Building: <color=red>OFF</color>";
            if (_isInBuildMode)
            {
                mode = "Building: <color=green>ON</color>";
                if (!_isStructureSpawned)
                {
                    SpawnStructure();
                }
            }
            else
            {
                Cancel(false);
            }

            _builderModeBackgroundImage.gameObject.SetActive(_isInBuildMode);
            _builderModeText.SetText(mode);
        }

        public void Cancel(bool force = true)
        {
            if (force)
            {
                _slotHandler = null;
            }
            _isPlacing = false;
            _isStructureSpawned = false;
            if (_structure != null)
            {
                Destroy(_structure.gameObject);
                _structure = null;
            }
        }

        public bool IsPlacing()
        {
            return _isPlacing;
        }

        public bool IsInBuildMode()
        {
            return _isInBuildMode;
        }
    }
}
