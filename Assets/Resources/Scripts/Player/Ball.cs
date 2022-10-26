using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GridManager manager => GridManager.Instance;

    public int x = 0;
    public int y = 0;
    public int color;

    public List<Ball> ballsToDestroy = new ();
    public List<Ball> neighbors = new ();


    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = manager.sprites[color];
        FindNeighborBalls();
    }

    public void FindNeighborBalls()
    {
        if (y > 0) neighbors.Add(manager.ballGrid[y - 1, x]);

        if (y < manager.width - 1) neighbors.Add(manager.ballGrid[y + 1, x]);

        if (x > 0) neighbors.Add(manager.ballGrid[y, x - 1]);

        if (x < manager.width - 1) neighbors.Add(manager.ballGrid[y, x + 1]);
    }

    public void FindBallsToDestroy(Ball ball)
    {
        foreach(Ball neighbor in ball.neighbors)
        {
            if (neighbor.color == color & !manager.ballsToDestroy.Contains(neighbor))
            {
                manager.ballsToDestroy.Add(neighbor);
                FindBallsToDestroy(neighbor);
            }
        }
    }

}
