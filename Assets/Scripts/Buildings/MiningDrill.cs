using Main;
using System.Collections;
using UnityEngine;

public class MiningDrill : Structure
{
    private MapManager mapManager;

    [Header("Components")]
    [SerializeField] private Transform _progressBar;
    [SerializeField] private Transform _healthBar;

    [Header("Values")]
    [SerializeField] private float _mineTime;
    public int amount;

    [Header("Debug")]
    [SerializeField, ReadOnly] private OreType _ore;
    [SerializeField, ReadOnly] private bool _isMining;
    [SerializeField, ReadOnly] private float _timer;


    private void Awake()
    {
        UpdateProgressBar();
    }
    private void Start()
    {
        mapManager = GameManager.instance.mapManager;
        StartCoroutine(PlaceDrill());
    }

    private void Update()
    {
        Mine();
    }

    private void Mine()
    {
        if (!_isMining) return;

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
        _ore = mapManager.GetOreOnTile(transform.position);
        if (_ore == OreType.None) yield break;
        yield return new WaitForSeconds(1f);
        _isMining = true;
    }

    private void UpdateProgressBar()
    {
        Vector3 barScale = _progressBar.localScale;
        barScale.x = _timer / _mineTime;
        _progressBar.localScale = barScale;
    }
}
