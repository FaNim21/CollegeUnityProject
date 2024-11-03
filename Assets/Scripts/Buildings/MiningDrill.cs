using Main;
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
        private MapManager _mapManager;

        [Header("Components")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TextMeshProUGUI _fuelAmountText;
        [SerializeField] private Transform _fuelUseBar;
        [SerializeField] private Transform _progressBar;

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
        private void Start()
        {
            _canvas.worldCamera = Camera.main;
            _mapManager = GameManager.instance.mapManager;
            _player = GameManager.instance.playerController;
            StartCoroutine(PlaceDrill()); //TEMP
        }

        private void Update()
        {
            if (!_canMine) return;

            UseFuel();
            Mine();
        }

        private void UseFuel()
        {
            if (_isFuelUsed || fuelHandler.data.itemData == null) return;
            if (fuelHandler.data.itemData.type != ItemType.Fuel || fuelHandler.data.quantity == 0) return;

            _fuelValue = fuelHandler.data.itemData.fuelPower;
            _fuelValueMax = fuelHandler.data.itemData.fuelPower;
            _isFuelUsed = true;
            fuelHandler.UpdateAmount(-1);
        }

        private void Mine()
        {
            if (_fuelValue <= 0f)
            {
                _isFuelUsed = false;
                return;
            }

            if (!_isMining || outputHandler.data.quantity >= _maxStock) return;

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

        public IEnumerator PlaceDrill()
        {
            _ore = _mapManager.GetOreOnTile(transform.position);
            if (_ore == OreType.None) yield break;

            ItemData data = GameManager.instance.GetItemData(_ore.ToString());
            outputItem = data;

            yield return new WaitForSeconds(1f);
            _isMining = true;
            _canMine = true;
        }

        public override void OnCollect()
        {
            Popup.Create(transform.position, "Zebrano ?????", Color.black);
        }
        public override void OpenGUI()
        {
            _player.inventory.OpenSidePanel<MiningDrillPanel>(this);
        }
    }
}
