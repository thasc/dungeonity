using System;
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
    }
}