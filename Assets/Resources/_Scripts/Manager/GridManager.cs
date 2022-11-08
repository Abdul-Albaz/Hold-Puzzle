using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;


public class GridManager : Singleton<GridManager>
{
    public int gridSizeX;
    public int gridSizeY;

    public GameObject playerPrefab;
    public GameObject ballPrefab;
    public GameObject tilePrefab;
    public Sprite[] sprites;
    
    [SerializeField]
    public Color[] colorIndex;

    public List<Ball> ballsToDestroy = new List<Ball>();
    public List<Ball> balls = new List<Ball>();

    public Vector3 orginSpawnPoint;

    public Tile[,] tileGrid;
    public Ball[,] ballGrid;

    public  int width;
    public int height;
    public int score;

    void Awake()
    {
        width = gridSizeX;
        height = gridSizeY;

        tileGrid = new Tile[width, height];
        ballGrid = new Ball[width, height];

        GameObject playerObject = Instantiate(playerPrefab, orginSpawnPoint + new Vector3(-1, -1, 0), Quaternion.identity);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {               
                GameObject newTileGO = Instantiate(tilePrefab, orginSpawnPoint + new Vector3(x, y, 0), Quaternion.identity);
                newTileGO.transform.parent = transform; 
                Tile newTile = newTileGO.GetComponent<Tile>();
                tileGrid[x, y] = newTile;
                newTile.x = x;
                newTile.y = y;

            }
        }

        Camera.main.transform.position = new Vector3(transform.position.x + gridSizeX / 2, (transform.position.y + gridSizeY / 2), -10);
        Camera.main.orthographicSize = gridSizeX + (Screen.height / Screen.width > 1.77 ? 1.5f : 0);
    }

    void Start()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Ball cellBall = Instantiate(ballPrefab, tileGrid[x, y].transform).GetComponent<Ball>();

                int color = Random.Range(0, sprites.Length);

                cellBall.x = x;
                cellBall.y = y;
                cellBall.color = color;

                ballGrid[x, y] = cellBall;

                balls.Add(cellBall);
                
            }
        }
    }


    public void CheckBalls(Ball ball)
    {
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
            if (ball == null)
            {
                Debug.Log("ball is null");
                return;
            }


            else
            {
                score++;
                Debug.Log("Destroy");
                ball.Destroy();
                
            }

            balls.Remove(ball);
        }



      
    }


    public void winLevel()
    {
        if (balls.Count==0)
        {
            Debug.Log("You Win");
            SoundManager.Play(AudioClips.victory);
        }
    }

}
