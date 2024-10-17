using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum OreTypes
{
    None,
    Ferrium,
    Cryserium,
}

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;


    public OreTypes GetOreOnTile(Vector2 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);
        TileBase tile = map.GetTile(gridPosition);

        if (Enum.TryParse(typeof(OreTypes), tile.name, true, out var result))
            return (OreTypes)result;
        return OreTypes.None;
    }
}
