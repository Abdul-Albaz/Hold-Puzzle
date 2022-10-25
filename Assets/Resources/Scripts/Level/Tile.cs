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

        if (x > 0) neighbors.Add(TileManager.Instance.tileGrid[x - 1, y]);
        if (x < TileManager.Instance.width - 1) neighbors.Add(TileManager.Instance.tileGrid[x + 1, y]);
        if (y > 0) neighbors.Add(TileManager.Instance.tileGrid[x, y - 1]);
        if (y < TileManager.Instance.width - 1) neighbors.Add(TileManager.Instance.tileGrid[x, y + 1]); 
    }
}
