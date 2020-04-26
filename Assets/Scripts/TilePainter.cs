using System;
using Code;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePainter : MonoBehaviour
{
    public Tile ToPaint;
    
    private Tilemap m_Tilemap;
    private Camera m_MainCamera;
    
    private static readonly Vector3Int s_DragStartNullObject = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    private Vector3Int m_DragStart = s_DragStartNullObject;
    private Vector3Int m_LastDragPosition = s_DragStartNullObject;
    private RectangleRenderer m_DragSelectionRenderer;

    private void Awake()
    {
        m_Tilemap = GetComponent<Tilemap>() ??
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
            var tileUnderMouse = m_Tilemap.TileUnderMouse(m_MainCamera);
            if (tileUnderMouse != m_LastDragPosition)
            {
                m_DragSelectionRenderer.Draw(m_DragStart, tileUnderMouse);
                m_LastDragPosition = tileUnderMouse;
            }
        }
    }

    private void OnMouseDown()
    {
        m_DragStart = m_Tilemap.TileUnderMouse(m_MainCamera);
    }

    private void OnMouseUp()
    {
        m_DragSelectionRenderer.Clear();
        m_Tilemap.BetterBoxFill(ToPaint, m_DragStart, m_Tilemap.TileUnderMouse(m_MainCamera));
        m_DragStart = s_DragStartNullObject;
        m_LastDragPosition = s_DragStartNullObject;
    }
}
