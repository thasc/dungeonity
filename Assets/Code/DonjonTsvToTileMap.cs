using System;
using System.IO;
using System.Linq;
using UnityEngine.Tilemaps;

public static class DonjonTsvToTileMap
{
    public static Tile[,] Process(string tsvFilePath, Tile openSpace, Tile closedSpace)
    {
        return Process(tsvFilePath, key => string.IsNullOrEmpty(key) ? closedSpace : openSpace);
    }
    
    public static Tile[,] Process(string tsvFilePath, Func<string, Tile> tileSelector)
    {
        if (!File.Exists(tsvFilePath))
        {
            throw new ArgumentException($"No file found at {tsvFilePath}, check your input/permissions");
        }
        
        var tsvFileContent = File.ReadAllLines(tsvFilePath);

        if (tsvFileContent.Length == 0)
        {
            throw new ArgumentException($"File at {tsvFilePath} was empty, check your input");
        }

        // every single line has exactly the same number of tab chars in it, equal to the tile width of the dungeon
        var tilesWide = tsvFileContent[0].Count(c => c == '\t');
        var tilesHeight = tsvFileContent.Length;

        var tileMap = new Tile[tilesWide, tilesHeight];

        for (var tileY = 0; tileY < tsvFileContent.Length; tileY++)
        {
            var tileX = 0;
            var cursorStart = 0;
            
            for (var cursorEnd = 0; cursorEnd < tsvFileContent[tileY].Length; cursorEnd++)
            {
                var charUnderCursor = tsvFileContent[tileY][cursorEnd];
                if (charUnderCursor == '\t') // end of current tile 
                {
                    var tileKey = tsvFileContent[tileY].Substring(cursorStart, cursorEnd - cursorStart);
                    // empty contents == empty tile, otherwise most things are open floors as far as we're concerned
                    tileMap[tileX, tsvFileContent.Length - 1 - tileY] = tileSelector(tileKey);
                    
                    // advance to next tile
                    cursorStart = cursorEnd + 1;
                    tileX++;
                }
            }
        }

        return tileMap;
    }
}
