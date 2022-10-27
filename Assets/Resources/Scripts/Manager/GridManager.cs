using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GridManager : Singleton<GridManager>
{
    public int gridSizeX;
    public int gridSizeY;

    public GameObject playerPrefab;
    public GameObject pathTile;
    public Sprite[] sprites;

    public GameObject[] walls;

    public List<Ball> ballsToDestroy = new List<Ball>();

    public Vector3 orginSpawnPoint;

    public Tile[,] tileGrid;
    public Ball[,] ballGrid;

    public  int width;
    public int height;

    void Awake()
    {
        width = gridSizeX;
        height = gridSizeY;

        tileGrid = new Tile[width, height];
        ballGrid = new Ball[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newTileGO = Instantiate(pathTile, orginSpawnPoint + new Vector3(x, y, 0), Quaternion.identity);
                newTileGO.transform.parent = transform; 
                Tile newTile = newTileGO.GetComponent<Tile>();
                tileGrid[x, y] = newTile;
                newTile.x = x;
                newTile.y = y;   
            }
        }
    }

    void Start()
    {

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Ball cellBall = Instantiate(playerPrefab, tileGrid[x, y].transform).GetComponent<Ball>();

                int color = Random.Range(0, sprites.Length);

                cellBall.x = x;
                cellBall.y = y;
                cellBall.color = color;

                ballGrid[x, y] = cellBall;
            }
        }


    }

    public void CheckBalls(Ball ball)
    {
        ballsToDestroy.Clear();
        FindBallsToDestroy(ball);
    }

    public void FindBallsToDestroy(Ball ball)
    {
        ballsToDestroy.Add(ball);

        foreach (Ball neighbor in ball.neighbors)
        {
            if (neighbor.color == ball.color & !ballsToDestroy.Contains(neighbor))
            {
                FindBallsToDestroy(neighbor);
            }
        }
    }

    public void DestroyBalls()
    {
        foreach (Ball ball in ballsToDestroy)
        {
            ball.Destroy();
        }
    }

}
