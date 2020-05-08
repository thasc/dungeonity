using Code;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Dungeon
{
    public class DungeonInitialiser : MonoBehaviour
    {
        public Tilemap[] AllTileMaps;
        public Tilemap FloorTileMap;
        public Tile OpenFloorTile;
        public Tile ClosedFloorFile;
        
        private DungeonStateManager m_DungeonStateManager;
        private TilePainter m_FloorTilePainter;

        private void Awake()
        {
            m_DungeonStateManager = GameObject.FindWithTag("DungeonStateManager")?.GetComponent<DungeonStateManager>();
            m_FloorTilePainter = FloorTileMap.gameObject.GetComponent<TilePainter>();
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(m_DungeonStateManager.DonjonTsvFilePath))
            {
                InitialiseBlank();
            }
            else
            {
                InitialiseFromDonjon();
            }
        }

        private void InitialiseBlank()
        {
            var size = new Vector2Int(m_DungeonStateManager.Width, m_DungeonStateManager.Height);

            foreach (var tileMap in AllTileMaps)
            {
                tileMap.SetSize(size);
                
                var associatedCollider = tileMap.gameObject.GetComponent<BoxCollider2D>();
                if (associatedCollider != null)
                {
                    associatedCollider.size = size;
                    associatedCollider.offset = new Vector2(size.x, size.y) / 2;
                }

                tileMap.gameObject.GetComponent<TileMapFiller>()?.Fill();
            }
        }

        private void InitialiseFromDonjon()
        {
            var donjon = DonjonTsvToTileMap.Process(m_DungeonStateManager.DonjonTsvFilePath,
                OpenFloorTile,
                ClosedFloorFile);
            
            var size = new Vector2Int(donjon.GetLength(0), donjon.GetLength(1));
            
            foreach (var tileMap in AllTileMaps)
            {
                tileMap.SetSize(size);
                
                var associatedCollider = tileMap.gameObject.GetComponent<BoxCollider2D>();
                if (associatedCollider != null)
                {
                    associatedCollider.size = size;
                    associatedCollider.offset = new Vector2(size.x, size.y) / 2;
                }

                tileMap.gameObject.GetComponent<TileMapFiller>()?.Fill();
            }
            
            m_FloorTilePainter.ApplyTiles(new RectInt(Vector2Int.zero, size), donjon, false);
        }
    }
}