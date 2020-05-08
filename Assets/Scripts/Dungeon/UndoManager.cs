using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon
{
    public class UndoManager : MonoBehaviour
    {
        // implements a rolling LIFO queue (losing oldest) with a specific capacity (the size of m_UndoQueue)

        public TilePainter TilePainter;
    
        private UndoJob[] m_UndoQueue;
        private int m_TopCursor;
        private int m_ItemsCount;

        private void Awake()
        {
            m_UndoQueue = new UndoJob[20];
            m_TopCursor = 0;
            m_ItemsCount = 0;
        }

        public void Push(RectInt bounds, Tile[,] tilesObliterated)
        {
            m_UndoQueue[m_TopCursor] = new UndoJob
            {
                Bounds = bounds,
                TilesToApply = tilesObliterated
            };

            IncrementCursor();
            m_ItemsCount = Math.Min(m_ItemsCount + 1, m_UndoQueue.Length);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
            {
                Pop();
            }
        }

        private void Pop()
        {
            if (m_ItemsCount == 0) return; // nothing to pop

            var undoJobIndex = m_TopCursor - 1;
            if (undoJobIndex < 0) undoJobIndex += m_UndoQueue.Length;
            var undoJob = m_UndoQueue[undoJobIndex];
        
            TilePainter.ApplyTiles(undoJob.Bounds, undoJob.TilesToApply, false);

            DecrementCursor();
            m_ItemsCount--;
        }

        private void IncrementCursor()
        {
            m_TopCursor = (m_TopCursor + 1) % m_UndoQueue.Length;
        }

        private void DecrementCursor()
        {
            m_TopCursor--;
            if (m_TopCursor < 0) m_TopCursor += m_UndoQueue.Length;
        }

        private class UndoJob
        {
            public RectInt Bounds { get; set; }
            public Tile[,] TilesToApply { get; set; }
        }
    }
}
