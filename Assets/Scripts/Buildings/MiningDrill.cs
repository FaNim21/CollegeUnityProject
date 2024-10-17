using Main;
using Main.Misc;
using UnityEngine;

public class MiningDrill : Structure
{
    private MapManager mapManager;


    private void Start()
    {
        mapManager = GameManager.instance.mapManager;

        var ore = mapManager.GetOreOnTile(transform.position);
        Utils.Log(ore);
    }
}
