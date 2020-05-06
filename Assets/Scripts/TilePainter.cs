using System;
using Code;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    public Tile OpenFloorTile;
    public Tilemap WallTilemap;
    
    private Tilemap m_FloorTilemap;
    private Camera m_MainCamera;
    
    private static readonly Vector3Int s_DragStartNullObject = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    private Vector3Int m_DragStart = s_DragStartNullObject;
    private Vector3Int m_LastDragPosition = s_DragStartNullObject;
    private RectangleRenderer m_DragSelectionRenderer;

    private void Awake()
    {
        m_FloorTilemap = GetComponent<Tilemap>() ??
                    throw new InvalidOperationException(
                        $"{nameof(TilePainter)} must be the sibling of a {nameof(Tilemap)}");
        m_MainCamera = Camera.main;
        
        m_DragSelectionRenderer = gameObject.AddComponent<RectangleRenderer>();
        m_DragSelectionRenderer.LineWidth = 0.05f;
    }

    private void FixedUpdate()
    {
        if (m_DragStart != s_DragStartNullObject)
        {
            var tileUnderMouse = m_FloorTilemap.TileUnderMouse(m_MainCamera);
            if (tileUnderMouse != m_LastDragPosition)
            {
                m_DragSelectionRenderer.Draw(m_DragStart, tileUnderMouse);
                m_LastDragPosition = tileUnderMouse;
            }
        }
    }

    private void OnMouseDown()
    {
        m_DragStart = m_FloorTilemap.TileUnderMouse(m_MainCamera);
    }

    private void OnMouseUp()
    {
        m_DragSelectionRenderer.Clear();
        var endPosition = m_FloorTilemap.TileUnderMouse(m_MainCamera);
        m_FloorTilemap.BetterBoxFill(OpenFloorTile, m_DragStart, endPosition);

        var cornerOne = new Vector2Int(Math.Min(m_DragStart.x, endPosition.x), Math.Min(m_DragStart.y, endPosition.y));
        var cornerTwo = new Vector2Int(Math.Max(m_DragStart.x, endPosition.x), Math.Max(m_DragStart.y, endPosition.y));
        
        Debug.Log($"Writing tiles from {cornerOne} to {cornerTwo}");
        
        for (var y = cornerOne.y - 1; y < cornerTwo.y + 2; y++)
        for (var x = cornerOne.x - 1; x < cornerTwo.x + 2; x++)
        {
            if (x >= 0 && x <= m_FloorTilemap.size.x && y >= 0 && y <= m_FloorTilemap.size.y)
            {
                UpdateTile(x, y);
            }
        }
        
        m_DragStart = s_DragStartNullObject;
        m_LastDragPosition = s_DragStartNullObject;
    }

    private void UpdateTile(int x, int y)
    {
        var tileIndex = 0;
        
        var thisTile = new Vector3Int(x, y, 0);

        if (!IsTileOpen(thisTile))
        {
            return; // only want to adjust walls for open tiles, for this exact moment
            // might change later when we start painting tiles closed
        }

        var northWestOpen = IsTileOpen(thisTile + Vector3Int.up + Vector3Int.left);
        var northOpen = IsTileOpen(thisTile + Vector3Int.up);
        var northEastOpen = IsTileOpen(thisTile + Vector3Int.up + Vector3Int.right);
        
        var westOpen = IsTileOpen(thisTile + Vector3Int.left);
        var eastOpen = IsTileOpen(thisTile + Vector3Int.right);
        
        var southWestOpen = IsTileOpen(thisTile + Vector3Int.down + Vector3Int.left);
        var southOpen = IsTileOpen(thisTile + Vector3Int.down);
        var southEastOpen = IsTileOpen(thisTile + Vector3Int.down + Vector3Int.right);

        if (northWestOpen) tileIndex += 1;
        if (northOpen) tileIndex += 2;
        if (northEastOpen) tileIndex += 4;
        if (westOpen) tileIndex += 8;
        if (eastOpen) tileIndex += 16;
        if (southWestOpen) tileIndex += 32;
        if (southOpen) tileIndex += 64;
        if (southEastOpen) tileIndex += 128;

        var tile = AssetDatabase.LoadAssetAtPath<Tile>($"Assets/Tiles/Generated/wallsandcorners_{tileIndex}.asset");
        WallTilemap.SetTile(thisTile, tile);
    }

    private bool IsTileOpen(Vector3Int tilePos)
    {
        return m_FloorTilemap.GetTile<Tile>(tilePos) == OpenFloorTile;
    }
}
