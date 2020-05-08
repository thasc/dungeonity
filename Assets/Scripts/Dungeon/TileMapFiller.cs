using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon
{
    public class TileMapFiller : MonoBehaviour
    {
        public Tile FillWith;
        
        private Tilemap m_TileMap;

        private void Awake()
        {
            m_TileMap = GetComponent<Tilemap>();
        }

        public void Fill()
        {
            m_TileMap.FloodFill(new Vector3Int(0, 0, 0), FillWith);
        }
    }
}