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
                //Ball cellBall = Instantiate(playerPrefab, orginSpawnPoint + new Vector3(i , j, 0), Quaternion.identity).GetComponent<Ball>();
                //Ball cellBall = Instantiate(playerPrefab, tileGrid[x,y].transform).GetComponent<Ball>();

                //int color = Random.Range(0, sprites.Length);

                //cellBall.x = x;
                //cellBall.y = y;
                //cellBall.color = color;

                //ballGrid[x, y] = cellBall;
            }
        }


    }

    public void GetBallsToDestroy(Ball ball)
    {
        //ballsToDestroy.AddRange(ball.ballsToDestroy);
        ball.FindBallsToDestroy(ball);
    }

    public void CheckBalls(Ball ball)
    {
        ballsToDestroy.Clear();
        ballsToDestroy.Add(ball);
        GetBallsToDestroy(ball);
    }

    public async void DestroyBalls()
    {
        foreach (Ball ballToDestroy in ballsToDestroy)
        {
            Destroy(ballToDestroy.gameObject);
            await Task.Delay(100);
        }
    }

    //private void TryStartMovement(Direction direction)
    //{
    //    _Direction = direction;

    //    Tile nextTile = CurrentTile.Neighbours[_Direction];

    //    if (nextTile == null) // if nextTile is null, is means that we are at the edge of the level. Don't move in that direction.
    //    {
    //        return;
    //    }
    //    else if (nextTile.AccessibilityState == AccessibilityState.Blocked)
    //    {
    //        return;
    //    }

    //    CreateTilePath();
    //    SetIsMoving(true);
    //}

}
