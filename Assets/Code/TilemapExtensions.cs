using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public static class TilemapExtensions
    {
        public static void BetterBoxFill(this Tilemap self, TileBase tile, RectInt bounds)
        {
            for (var xx = bounds.xMin; xx < bounds.xMax; xx++)
            {
                for (var yy = bounds.yMin; yy < bounds.yMax; yy++)
                {
                    self.SetTile(new Vector3Int(xx, yy, 0), tile);
                }
            }
        }
        
        public static Vector3Int TileUnderMouse(this Tilemap self, Camera camera)
        {
            var mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
            return self.WorldToCell(mouseWorldPos);
        }

        public static void SetSize(this Tilemap self, Vector2Int size)
        {
            self.origin = Vector3Int.zero;
            self.size = new Vector3Int(size.x, size.y, 0);
            self.ResizeBounds();
        }
    }
}