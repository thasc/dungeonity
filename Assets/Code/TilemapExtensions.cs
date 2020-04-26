using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Code
{
    public static class TilemapExtensions
    {
        public static void BetterBoxFill(this Tilemap self,
            TileBase tile,
            Vector3Int startPosition,
            Vector3Int endPosition)
        {
            var minX = Math.Min(startPosition.x, endPosition.x);
            var minY = Math.Min(startPosition.y, endPosition.y);
            var maxX = Math.Max(startPosition.x, endPosition.x);
            var maxY = Math.Max(startPosition.y, endPosition.y);
            
            for (var xx = minX; xx <= maxX; xx++)
            {
                for (var yy = minY; yy <= maxY; yy++)
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