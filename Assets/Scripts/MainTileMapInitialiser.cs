using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainTileMapInitialiser : MonoBehaviour
{
    public Tile InitialTile;
    
    private DungeonStateManager m_DungeonStateManager;

    private void Awake()
    {
        m_DungeonStateManager = GameObject.FindWithTag("DungeonStateManager")?
                                    .GetComponent<DungeonStateManager>() ??
                                throw new ArgumentNullException(nameof(m_DungeonStateManager));
    }

    private void Start()
    {
        var tileMap = GetComponent<Tilemap>();
        tileMap.origin = Vector3Int.zero;
        tileMap.size = new Vector3Int(m_DungeonStateManager.Width, m_DungeonStateManager.Height, 0);
        tileMap.ResizeBounds();
        tileMap.FloodFill(new Vector3Int(0, 0, 0), InitialTile);
    }
}
