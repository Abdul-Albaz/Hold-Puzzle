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

        if (x > 0) neighbors.Add(GridManager.Instance.tileGrid[x - 1, y]);
        if (x < GridManager.Instance.width - 1) neighbors.Add(GridManager.Instance.tileGrid[x + 1, y]);
        if (y > 0) neighbors.Add(GridManager.Instance.tileGrid[x, y - 1]);
        if (y < GridManager.Instance.width - 1) neighbors.Add(GridManager.Instance.tileGrid[x, y + 1]); 
    }
}
