using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum OreType
{
    None,
    Ferrium,
    Cryserium,
}

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;


    public OreType GetOreOnTile(Vector2 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);
        TileBase tile = map.GetTile(gridPosition);

        if (Enum.TryParse(typeof(OreType), tile.name, true, out var result))
            return (OreType)result;
        return OreType.None;
    }
}
