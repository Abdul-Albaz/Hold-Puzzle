using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int row = 0;
    public int column = 0;
    public Material ballcolor;
    public List<Ball> ballNeighbors = new List<Ball>();
    

    public void Start()
    {
        ballcolor = this.GetComponent<Renderer>().material;
        ballNeighbors = NeighborsBall();

       // CheckNeighborsBall();
    }


    public List<Ball> NeighborsBall()
    {
        List<Ball> temp = new();

        if (row > 0) temp.Add(TileManager.Instance.ballGrid[row - 1, column]);

        if (row < TileManager.Instance.width - 1) temp.Add(TileManager.Instance.ballGrid[row + 1, column]);

        if (column > 0) temp.Add(TileManager.Instance.ballGrid[row, column - 1]);

        if (column < TileManager.Instance.width - 1) temp.Add(TileManager.Instance.ballGrid[row, column + 1]);

        return temp;


        
    }


    //void CheckNeighborsBall()
    //{
    //    for (int i = 0; i < ballNeighbors.Count; i++)
    //    {
    //        if (ballNeighbors[i].GetComponent<Material>().color == this.GetComponent<Material>().color)
    //        {
    //            print("work");
    //        }
    //    }
    //}



    // use this lines of code for matching balls

    private void OnTriggerStay(Collider other)
    {

        if (other.tag == gameObject.tag && other.name != "player")
        {
            int r = other.GetComponent<Ball>().row;
            int c = other.GetComponent<Ball>().column;

            if ((column == c && r <= (row + 1)) || ((row == r && c <= (column + 1))))
            {
                other.transform.parent = gameObject.transform;
            }


        }
    }
}
