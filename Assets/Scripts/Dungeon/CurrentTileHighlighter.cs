using Code;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon
{
    public class CurrentTileHighlighter : MonoBehaviour
    {
        public Tile HighlightTile;
    
        private Tilemap m_Tilemap;
        private Camera m_MainCamera;
        private Vector3Int m_HighlightedTile;

        private void Awake()
        {
            m_Tilemap = GetComponent<Tilemap>();
            m_MainCamera = Camera.main;
        }

        private void Update()
        {
            var tileUnderMouse = m_Tilemap.TileUnderMouse(m_MainCamera);

            if (tileUnderMouse != m_HighlightedTile)
            {
                m_Tilemap.SetTile(m_HighlightedTile, null);
                m_Tilemap.SetTile(tileUnderMouse, HighlightTile);
                m_HighlightedTile = tileUnderMouse;
            }
        }
    }
}
