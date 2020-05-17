using System;
using UnityEngine;

namespace Dungeon
{
    public class RectangleRenderer : MonoBehaviour
    {
        public Sprite WhitePixel;
        public float LineWidth = 0.05f;
        
        private SpriteRenderer m_NorthLine;
        private SpriteRenderer m_EastLine;
        private SpriteRenderer m_SouthLine;
        private SpriteRenderer m_WestLine;

        private void Awake()
        {
            m_NorthLine = CreateLineSpriteRenderer();
            m_EastLine = CreateLineSpriteRenderer();
            m_SouthLine = CreateLineSpriteRenderer();
            m_WestLine = CreateLineSpriteRenderer();
            
            m_NorthLine.enabled = false;
            m_EastLine.enabled = false;
            m_SouthLine.enabled = false;
            m_WestLine.enabled = false;
        }

        private SpriteRenderer CreateLineSpriteRenderer()
        {
            var lineObject = new GameObject();
            lineObject.transform.parent = gameObject.transform;
            var lineObjectSpriteRenderer = lineObject.AddComponent<SpriteRenderer>();
            lineObjectSpriteRenderer.sprite = WhitePixel;
            lineObjectSpriteRenderer.color = Color.blue;
            lineObjectSpriteRenderer.sortingLayerID = SortingLayer.NameToID("TileHighlighting");
            return lineObjectSpriteRenderer;
        }

        public void Draw(Vector3Int from, Vector3Int to)
        {
            var minX = Math.Min(from.x, to.x);
            var minY = Math.Min(from.y, to.y);
            var maxX = Math.Max(from.x+1, to.x+1);
            var maxY = Math.Max(from.y + 1, to.y + 1);

            var width = maxX - minX;
            var height = maxY - minY;
            
            m_NorthLine.enabled = true;
            m_EastLine.enabled = true;
            m_SouthLine.enabled = true;
            m_WestLine.enabled = true;

            var northLineTransform = m_NorthLine.transform;
            var eastLineTransform = m_EastLine.transform;
            var southLineTransform = m_SouthLine.transform;
            var westLineTransform = m_WestLine.transform;
            
            northLineTransform.position = new Vector3(minX + width/2f, minY + LineWidth/2f);
            eastLineTransform.position = new Vector3(maxX - LineWidth/2f, minY + height/2f);
            southLineTransform.position = new Vector3(minX + width/2f, maxY - LineWidth/2f);
            westLineTransform.position = new Vector3(minX + LineWidth/2f, minY + height/2f);
            
            northLineTransform.localScale = new Vector3(width, LineWidth);
            eastLineTransform.localScale = new Vector3(LineWidth, height);
            southLineTransform.localScale = new Vector3(width, LineWidth);
            westLineTransform.localScale = new Vector3(LineWidth, height);
        }

        public void Clear()
        {
            m_NorthLine.enabled = false;
            m_EastLine.enabled = false;
            m_SouthLine.enabled = false;
            m_WestLine.enabled = false;
        }
    }
}