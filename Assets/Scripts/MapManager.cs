using Main.Buildings;
using Main.Misc;
using System;
using System.Collections.Generic;
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

    [SerializeField] private List<Structure> _placedStructures = new();

    public int mapSize;

    public Vector2 halfSize;
    public Vector2 leftTop;
    public Vector2 leftBottom;
    public Vector2 rightTop;
    public Vector2 rightBottom;

    [SerializeField] private Vector2 boundsOffset;

    private float _halfMapSize;


    private void Awake()
    {
        _halfMapSize = mapSize / 2;
    }

    public OreType GetOreOnTile(Vector2 position) 
    {
        Vector3Int gridPosition = map.WorldToCell(position);
        TileBase tile = map.GetTile(gridPosition);

        if (Enum.TryParse(typeof(OreType), tile.name, true, out var result))
            return (OreType)result;
        return OreType.None;
    }

    public Vector3Int GetTilePosition(Vector2 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);
        return gridPosition;
    }

    public void AddStructure(Structure structure)
    {
        _placedStructures.Add(structure);
    }
    public void RemoveStructure(Structure structure)
    {
        _placedStructures.Remove(structure);
    }

    public bool CheckForStructure(Vector2 mousePosition, Vector2 checkSize)
    {
        halfSize = checkSize;
        leftTop = mousePosition - new Vector2(halfSize.x, 0) + new Vector2(0, halfSize.y);
        leftBottom = mousePosition - halfSize;
        rightTop = mousePosition + halfSize;
        rightBottom = mousePosition + new Vector2(halfSize.x, 0) - new Vector2(0, halfSize.y);

        for (int i = 0; i < _placedStructures.Count; i++)
        {
            var current = _placedStructures[i];
            Vector2 position = current.transform.position;

            if (leftTop.x <= position.x + current.halfSize.x &&
                leftTop.x >= position.x - current.halfSize.x &&
                leftTop.y <= position.y + current.halfSize.y &&
                leftTop.y >= position.y - current.halfSize.y)
            {
                return true;
            }
            if (leftBottom.x <= position.x + current.halfSize.x &&
                leftBottom.x >= position.x - current.halfSize.x &&
                leftBottom.y <= position.y + current.halfSize.y &&
                leftBottom.y >= position.y - current.halfSize.y)
            {
                return true;
            }
            if (rightTop.x <= position.x + current.halfSize.x &&
                rightTop.x >= position.x - current.halfSize.x &&
                rightTop.y <= position.y + current.halfSize.y &&
                rightTop.y >= position.y - current.halfSize.y)
            {
                return true;
            }
            if (rightBottom.x <= position.x + current.halfSize.x &&
                rightBottom.x >= position.x - current.halfSize.x &&
                rightBottom.y <= position.y + current.halfSize.y &&
                rightBottom.y >= position.y - current.halfSize.y)
            {
                return true;
            }
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(leftTop, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(leftBottom, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(rightTop, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rightBottom, 0.5f);
    }

    /*public bool CheckForStructure(Vector2 mousePosition, Vector2 checkSize)
    {
        for (int i = 0; i < _placedStructures.Count; i++)
        {
            var current = _placedStructures[i];
            Vector2 position = current.transform.position;

            if (mousePosition.x < position.x + current.size.x &&
                mousePosition.x + checkSize.x > position.x &&
                mousePosition.y < position.y + current.size.y &&
                mousePosition.y + checkSize.y > position.y)
            {
                return true;
            }
        }

        return false;
    }*/

    public bool IsInBounds(Vector2 position, Vector2 offset)
    {
        return position.x + offset.x < _halfMapSize &&
               position.x - offset.x > -_halfMapSize &&
               position.y + offset.y < _halfMapSize &&
               position.y - offset.y > -_halfMapSize;
    }
}
