using Main.Datas;
using Main.Misc;
using Main.Player;
using Main.UI.Equipment;
using Main.UI.Equipment.SidePanels;
using Main.Visual;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Main.Buildings
{
    public class MiningDrill : Structure
    {
        private PlayerController _player;

        [Header("Components")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _fuelAmountText;
        [SerializeField] private Transform _fuelUseBar;
        [SerializeField] private Transform _progressBar;

        [SerializeField] private GameObject _notOnOreInfo;
        [SerializeField] private GameObject _emptyFuelInfo;
        [SerializeField] private GameObject _fullOutputInfo;

        [Header("Values")]
        public ItemData outputItem;
        public SlotHandler fuelHandler;
        public SlotHandler outputHandler;
        [SerializeField] private float _maxStock;
        [SerializeField] private float _mineTime;

        [Header("Debug")]
        [SerializeField, ReadOnly] private OreType _ore;
        [SerializeField, ReadOnly] private int _amount;
        [SerializeField, ReadOnly] private float _fuelValue;
        [SerializeField, ReadOnly] private float _fuelValueMax = 40;
        [SerializeField, ReadOnly] private bool _canMine;
        [SerializeField, ReadOnly] private bool _isMining;
        [SerializeField, ReadOnly] private bool _isFuelUsed;
        [SerializeField, ReadOnly] private float _timer;

        public float MineProgress => _timer / _mineTime;
        public float FuelProgress => _fuelValue / _fuelValueMax;


        protected override void Awake()
        {
            base.Awake();

            Utils.UpdateBar(_progressBar, 0);
            _fuelAmountText.SetText("0");
            Utils.UpdateBar(_fuelUseBar, 0);

            outputHandler.structureGuid = guid;
            fuelHandler.structureGuid = guid;
        }
        protected override void Start()
        {
            base.Start();
            _canvas.worldCamera = Camera.main;
            _player = GameManager.instance.playerController;
        }

        private void Update()
        {
            if (!_canMine || _inPlacementMode) return;

            UseFuel();
            Mine();
        }

        private void UseFuel()
        {
            if (_isFuelUsed || fuelHandler.data.itemData == null) return;
            if (fuelHandler.data.itemData.type != ItemType.Fuel || fuelHandler.data.quantity == 0) return;

            if (_emptyFuelInfo.activeSelf) _emptyFuelInfo.SetActive(false);
            _fuelValue = fuelHandler.data.itemData.fuelPower;
            _fuelValueMax = fuelHandler.data.itemData.fuelPower;
            _isFuelUsed = true;
            fuelHandler.UpdateAmount(-1);
        }

        private void Mine()
        {
            if (_fuelValue <= 0f)
            {
                _emptyFuelInfo.SetActive(true);
                _isFuelUsed = false;
                return;
            }

            if (!_isMining) return;
            if (outputHandler.data.quantity >= _maxStock)
            {
                _fullOutputInfo.SetActive(true);
                return;
            }
            if (_fullOutputInfo.activeSelf) _fullOutputInfo.SetActive(false);

            _timer += Time.deltaTime;
            _fuelValue -= Time.deltaTime;
            Utils.UpdateBar(_progressBar, MineProgress);
            Utils.UpdateBar(_fuelUseBar, FuelProgress);
            _fuelAmountText.SetText(fuelHandler.data.quantity.ToString());

            if (_timer >= _mineTime)
            {
                _timer = 0;
                outputHandler.CreateItem(outputItem, 1);
            }
        }

        public override void ExitPlacementMode()
        {
            base.ExitPlacementMode();
            StartCoroutine(PlaceDrill());
        }

        public IEnumerator PlaceDrill()
        {
            _ore = _mapManager.GetOreOnTile(transform.position);
            if (_ore == OreType.None)
            {
                _notOnOreInfo.SetActive(true);
                yield break;
            }

            ItemData data = GameManager.instance.GetItemData(_ore.ToString());
            outputItem = data;

            yield return new WaitForSeconds(1f);
            _isMining = true;
            _canMine = true;
        }

        public override void OnCollect(Inventory inventory)
        {
            Destroy(gameObject);

            if (fuelHandler.data.itemData != null)
            {
                bool success = inventory.AddToInventory(fuelHandler.data.itemData, fuelHandler.data.quantity);
                if (success)
                {
                    Popup.Create(transform.position + new Vector3(0, 0.5f), $"Collected {fuelHandler.data.quantity} {fuelHandler.data.itemData.name}", Color.black);
                }
            }

            if (outputHandler.data.itemData != null)
            {
                bool success = inventory.AddToInventory(outputHandler.data.itemData, outputHandler.data.quantity);
                if (success)
                {
                    Popup.Create(transform.position + new Vector3(0, 1f), $"Collected {outputHandler.data.quantity} {outputHandler.data.itemData.name}", Color.black);
                }
            }

            Popup.Create(transform.position, "Collected Mining Drill", Color.black);
            base.OnCollect(inventory);
        }
        public override void OpenGUI()
        {
            _player.inventory.OpenSidePanel<MiningDrillPanel>(this);
        }
    }
}
