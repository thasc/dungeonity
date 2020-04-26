using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainTilemapInitialiser : MonoBehaviour
{
    public Vector2Int Size;
    public Tile InitialTile;

    private void Start()
    {
        var tilemap = GetComponent<Tilemap>();
        tilemap.size = new Vector3Int(Size.x, Size.y, 0);
        tilemap.FloodFill(new Vector3Int(0, 0, 0), InitialTile);
    }
}
