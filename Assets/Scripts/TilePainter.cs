using System;
using Code;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    public Tile OpenFloorTile;
    public Tile ClosedFloorTile;
    public Tilemap WallTilemap;
    public UndoManager UndoManager;
    
    private Tilemap m_FloorTilemap;
    private Camera m_MainCamera;
    
    private static readonly Vector3Int s_DragStartNullObject = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    private Vector3Int m_DragStart = s_DragStartNullObject;
    private Vector3Int m_LastDragPosition = s_DragStartNullObject;
    private RectangleRenderer m_DragSelectionRenderer;

    public void ApplyTiles(RectInt bounds, Tile[,] tilesToApply)
    {
        for (var y = bounds.yMin; y < bounds.yMax; y++)
        for (var x = bounds.xMin; x < bounds.xMax; x++)
        {
            var where = new Vector3Int(x, y, 0);
            FillTiles(tilesToApply[x - bounds.xMin, y - bounds.yMin], where, where, true);
        }
    }

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
        
        var tileToApply = Input.GetKey(KeyCode.LeftAlt) ? ClosedFloorTile : OpenFloorTile;
        var endPosition = m_FloorTilemap.TileUnderMouse(m_MainCamera);
        FillTiles(tileToApply, m_DragStart, endPosition);

        m_DragStart = s_DragStartNullObject;
        m_LastDragPosition = s_DragStartNullObject;
    }

    private void FillTiles(Tile tileToApply, Vector3Int startPosition, Vector3Int endPosition, bool undoing = false)
    {
        var cornerOne = new Vector2Int(Math.Min(startPosition.x, endPosition.x), Math.Min(startPosition.y, endPosition.y));
        var cornerTwo = new Vector2Int(Math.Max(startPosition.x, endPosition.x) + 1, Math.Max(startPosition.y, endPosition.y) + 1);

        if (!undoing) PushToUndoManager(cornerOne, cornerTwo);
        
        m_FloorTilemap.BetterBoxFill(tileToApply, startPosition, endPosition);

        // might want to suspend this until after the undo is done since it does one tile at a time
        for (var y = cornerOne.y - 1; y < cornerTwo.y + 1; y++)
        for (var x = cornerOne.x - 1; x < cornerTwo.x + 1; x++)
        {
            if (x >= 0 && x <= m_FloorTilemap.size.x && y >= 0 && y <= m_FloorTilemap.size.y)
            {
                UpdateTile(x, y);
            }
        }
    }

    private void PushToUndoManager(Vector2Int cornerOne, Vector2Int cornerTwo)
    {
        var selectionWidth = cornerTwo.x - cornerOne.x;
        var selectionHeight = cornerTwo.y - cornerOne.y;
        
        var currentTiles = new Tile[selectionWidth, selectionHeight];
        for (var y = 0; y < selectionHeight; y++)
        for (var x = 0; x < selectionWidth; x++)
        {
            if (x >= 0 && x <= m_FloorTilemap.size.x && y >= 0 && y <= m_FloorTilemap.size.y)
            {
                currentTiles[x, y] = m_FloorTilemap.GetTile<Tile>(new Vector3Int(cornerOne.x + x, cornerOne.y + y, 0));
            }
        }

        UndoManager.Push(new RectInt(cornerOne.x, cornerOne.y,cornerTwo.x - cornerOne.x, cornerTwo.y - cornerOne.y), currentTiles);
    }

    private void UpdateTile(int x, int y)
    {
        var tileIndex = 0;
        
        var thisTile = new Vector3Int(x, y, 0);

        if (!IsTileOpen(thisTile))
        {
            WallTilemap.SetTile(thisTile, null);
            return;
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

        var tile = Resources.Load<Tile>($"GeneratedTiles/wallsandcorners_{tileIndex}");
        WallTilemap.SetTile(thisTile, tile);
    }

    private bool IsTileOpen(Vector3Int tilePos)
    {
        return m_FloorTilemap.GetTile<Tile>(tilePos) == OpenFloorTile;
    }
}
