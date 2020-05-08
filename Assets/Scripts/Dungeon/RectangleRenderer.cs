using System;
using UnityEngine;

namespace Dungeon
{
    public class RectangleRenderer : MonoBehaviour
    {
        public float LineWidth
        {
            get => m_LineRenderer.startWidth;
            set 
            {
                m_LineRenderer.startWidth = value;
                m_LineRenderer.endWidth = value;
            }
        }

        private LineRenderer m_LineRenderer;

        private void Awake()
        {
            m_LineRenderer = gameObject.AddComponent<LineRenderer>();
            m_LineRenderer.loop = true;
            m_LineRenderer.positionCount = 4;
            m_LineRenderer.startWidth = 0.1f;
            m_LineRenderer.endWidth = 0.1f;
            m_LineRenderer.sortingLayerID = SortingLayer.NameToID("TileHighlighting");
            m_LineRenderer.enabled = false;
        }

        public void Draw(Vector3Int from, Vector3Int to)
        {
            m_LineRenderer.enabled = true;
        
            var minX = Math.Min(from.x, to.x);
            var minY = Math.Min(from.y, to.y);
            var maxX = Math.Max(from.x+1, to.x+1);
            var maxY = Math.Max(from.y+1, to.y+1);
            
            var topLeft = new Vector3(minX, minY, 0);
            var topRight = new Vector3(maxX, minY, 0);
            var bottomRight = new Vector3(maxX, maxY, 0);
            var bottomLeft = new Vector3(minX, maxY, 0);
            
            m_LineRenderer.SetPositions(new[]{topLeft, topRight, bottomRight, bottomLeft});
        }
    
        public void Clear()
        {
            m_LineRenderer.SetPositions(Array.Empty<Vector3>());
            m_LineRenderer.enabled = false;
        }
    }
}