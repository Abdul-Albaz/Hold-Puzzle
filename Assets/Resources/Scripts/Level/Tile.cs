using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public List<Tile> neighbors;

    void Start()
    {
        neighbors = new List<Tile>();
        if (x > 0) neighbors.Add(TileManager.tileGrid[x - 1, y]);
        if (x < TileManager.width - 1) neighbors.Add(TileManager.tileGrid[x, x + 1]);
        if (y > 0) neighbors.Add(TileManager.tileGrid[x, y - 1]);
        if (y < TileManager.width - 1) neighbors.Add(TileManager.tileGrid[x, y + 1]);
    }
}
