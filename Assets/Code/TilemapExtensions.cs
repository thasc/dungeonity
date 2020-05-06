using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public static class TilemapExtensions
    {
        public static void BetterBoxFill(this Tilemap self,
            TileBase tile,
            Vector2Int cornerOne,
            Vector2Int cornerTwo)
        {
            for (var xx = cornerOne.x; xx <= cornerTwo.x; xx++)
            {
                for (var yy = cornerOne.y; yy <= cornerTwo.y; yy++)
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
    }
}