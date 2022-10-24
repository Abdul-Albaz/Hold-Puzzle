using UnityEngine;

public class CellBall : MonoBehaviour
{
    public int row = 0;
    public int column = 0;

    private void OnTriggerStay(Collider other)
    {     
            if (other.tag == gameObject.tag && other.name != "player")
            {
                int r = other.GetComponent<CellBall>().row;
                int c = other.GetComponent<CellBall>().column;
            
            if ((column == c && r <= (row + 1)) || ((row == r && c <= (column + 1))))
            {           
                other.transform.parent = gameObject.transform;
            }
        }  
    }    
}
