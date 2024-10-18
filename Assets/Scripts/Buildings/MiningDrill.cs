using Main;
using Main.Player;
using System.Collections;
using UnityEngine;

public class MiningDrill : Structure
{
    private PlayerController _player;
    private MapManager _mapManager;

    [Header("Components")]
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
        UpdateProgressBar();
    }
    private void Start()
    {
        _mapManager = GameManager.instance.mapManager;
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
        UpdateProgressBar();

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

    private void UpdateProgressBar()
    {
        Vector3 barScale = _progressBar.localScale;
        barScale.x = _timer / _mineTime;
        _progressBar.localScale = barScale;
    }

    public override void OnCollect()
    {

    }
}
