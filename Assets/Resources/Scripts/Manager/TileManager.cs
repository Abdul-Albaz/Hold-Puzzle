using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileManager : Singleton<TileManager>
{
    public int gridSizeX;
    public int gridSizeY;

    public GameObject[] playerPrefab;
    public GameObject pathTile;

    public GameObject[] walls;
    
    public Vector3 orginSpawnPoint;

    public Tile[,] tileGrid;
    public Ball[,] ballGrid;

    public  int width;
    public int height;

    void Awake()
    {
        width = gridSizeX +1;
        height = gridSizeY + 1;

        tileGrid = new Tile[width, height];
        ballGrid = new Ball[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newTileGO = Instantiate(pathTile, orginSpawnPoint + new Vector3(x, y, 0), Quaternion.identity);
                Tile newTile = newTileGO.GetComponent<Tile>();
                tileGrid[x, y] = newTile;
                newTile.x = x;
                newTile.y = y;   
            }
        }
    }

    void Start()
    {

        for (int i = 0; i <= gridSizeX; i++)
        {
            for (int j = 0; j <= gridSizeY; j++)
            {
                int rand = Random.Range(0, 3);
                GameObject cellBall = Instantiate(playerPrefab[rand], orginSpawnPoint + new Vector3(i , j, 0), Quaternion.identity);
                
                cellBall.GetComponent<Ball>().row = j;
                cellBall.GetComponent<Ball>().column = i;

                ballGrid[j, i] = cellBall.GetComponent<Ball>();

                print(ballGrid[j, i]);
            }
        }


        //Instantiate(walls[0], new Vector3(-2f, PlayerMovement.Instance.tileManager.gridSizeY-4f, 0f), Quaternion.identity);
        //Instantiate(walls[1], new Vector3(PlayerMovement.Instance.tileManager.gridSizeX - 1f ,- 4f , 0f), Quaternion.AxisAngle(Vector3.zero,90));
        //Instantiate(walls[2], new Vector3(PlayerMovement.Instance.tileManager.gridSizeX + 1, PlayerMovement.Instance.tileManager.gridSizeX + 1, 0f), Quaternion.identity);
        //Instantiate(walls[2], new Vector3(PlayerMovement.Instance.tileManager.gridSizeX + 1, PlayerMovement.Instance.tileManager.gridSizeX -1 , 0f), Quaternion.identity);
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
