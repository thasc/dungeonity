﻿using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapInitialiser : MonoBehaviour
{
    public Tile InitialTile;
    
    private DungeonStateManager m_DungeonStateManager;
    private Tilemap m_TileMap;
    private BoxCollider2D m_Collider;

    private void Awake()
    {
        m_DungeonStateManager = GameObject.FindWithTag("DungeonStateManager")?
                                    .GetComponent<DungeonStateManager>() ??
                                throw new ArgumentNullException(nameof(m_DungeonStateManager));
        m_TileMap = GetComponent<Tilemap>();
        m_Collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        ResizeTileMap(m_DungeonStateManager.Width, m_DungeonStateManager.Height);
        m_TileMap.FloodFill(new Vector3Int(0, 0, 0), InitialTile);
    }

    private void ResizeTileMap(int width, int height)
    {
        m_TileMap.origin = Vector3Int.zero;
        
        m_TileMap.size = new Vector3Int(width, height, 0);
        m_TileMap.ResizeBounds();

        var tileMapSize = new Vector2(m_TileMap.size.x, m_TileMap.size.y);

        if (m_Collider != null)
        {
            m_Collider.size = tileMapSize;
            m_Collider.offset = tileMapSize / 2;
        }
    }
}