using Main;
using Main.Player;
using Main.UI.Equipment.SidePanels;
using Main.Visual;
using System.Collections;
using TMPro;
using UnityEngine;

public class MiningDrill : Structure
{
    private PlayerController _player;
    private MapManager _mapManager;

    [Header("Components")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _fuelAmountText;
    [SerializeField] private Transform _fuelUseBar;
    [SerializeField] private Transform _progressBar;
    [SerializeField] private Transform _healthBar;

    [Header("Values")]
    [SerializeField] private float _maxStock;
    [SerializeField] private float _mineTime;
    [SerializeField] private float _fuelPerMine;

    [Header("Debug")]
    [SerializeField, ReadOnly] private OreType _ore;
    [SerializeField, ReadOnly] private int amount;
    [SerializeField, ReadOnly] private int _fuelAmount;
    [SerializeField, ReadOnly] private bool _canMine;
    [SerializeField, ReadOnly] private bool _isMining;
    [SerializeField, ReadOnly] private float _timer;


    private void Awake()
    {
        UpdateBar(_progressBar, 0);
        _fuelAmountText.SetText("0");
        UpdateBar(_fuelUseBar, 0);
    }
    private void Start()
    {
        _canvas.worldCamera = Camera.main;
        _mapManager = GameManager.instance.mapManager;
        _player = GameManager.instance.playerController;
        StartCoroutine(PlaceDrill());
    }

    private void Update()
    {
        Mine();
    }

    private void Mine()
    {
        if (!_canMine || !_isMining || amount == _maxStock) return;

        _timer += Time.deltaTime;
        UpdateBar(_progressBar, _timer / _mineTime);

        if (_timer >= _mineTime)
        {
            _timer = 0;
            amount++;
        }
    }

    public IEnumerator PlaceDrill()
    {
        _ore = _mapManager.GetOreOnTile(transform.position);
        if (_ore == OreType.None) yield break;
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
        _player.inventory.OpenWindow();
        _player.inventory.OpenSidePanel<MiningDrillPanel>();
    }
}
