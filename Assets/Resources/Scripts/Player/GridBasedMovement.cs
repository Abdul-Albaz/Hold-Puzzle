using System.Collections;
using UnityEngine;

public class GridBasedMovement : MonoBehaviour
{
    bool isMoving;
    private Vector3 startPos, targetPos;
    private float timeToMove = 0.15f;
    public TileManager tileManager;

    private Vector3 shootDiriction;
    public Vector3 lastPos;

    public bool keepM;

    public Material[] ballColorMaterial;
    MeshRenderer playerMat;
    public string[] tags;
    int colorIndex;

    bool goLeft,goRight,goUp,goDown;
    
    void Start()
    {
        colorIndex = Random.Range(0, 3);
        playerMat = gameObject.GetComponent<MeshRenderer>();
        playerMat.material = ballColorMaterial[colorIndex];
        gameObject.tag = tags[colorIndex];
    }


    void Update()
    {
        if (transform.position.x == -1 && transform.position.y == -1)
        { 
            goUp = true;
            goDown = false;
            goRight = false;
            goLeft = false; 
        }
      
        if (transform.position.x == -1 && transform.position.y == (tileManager.gridSizeY + 1))
        { 
            goUp = false;
            goDown = false;
            goRight = true;
            goLeft = false;     
        }

        if (transform.position.x == (tileManager.gridSizeX + 1) && transform.position.y == (tileManager.gridSizeY + 1))
        {
            goUp = false;
            goDown = true;
            goRight = false;
            goLeft = false;   
        }


        if (transform.position.x == (tileManager.gridSizeX + 1) && transform.position.y == -1)
        {
            goUp = false;
            goDown = false;
            goRight = false;
            goLeft = true;   
        }


        if (Input.GetMouseButton(0) && !isMoving)
        {

            if (goUp == true)
            {
                keepM = false;
                shootDiriction = Vector3.right;
                StartCoroutine(MovePlayer(Vector3.up));

            }

            else if (goDown == true)
            {
                    keepM = false;
                    shootDiriction = Vector3.left;
                    StartCoroutine(MovePlayer(Vector3.down));
            }

            else if (goRight == true)
            {
                keepM = false;
                shootDiriction = Vector3.down;
                StartCoroutine(MovePlayer(Vector3.right));
            }

            else if (goLeft == true)
            {
                keepM = false;
                shootDiriction = Vector3.up;
                StartCoroutine(MovePlayer(Vector3.left));
            }
        }

         if (Input.GetKey(KeyCode.LeftShift) && !isMoving)
        {
            lastPos = transform.position;
            keepM = true;

            StartCoroutine(callMovePlayer());  
        }

    }

    private IEnumerator MovePlayer(Vector3 direction)
    {  
        isMoving = true;
        float elapsedtime = 0;
        startPos = transform.position;
        targetPos = startPos + direction;

        while (elapsedtime < timeToMove)
        {
           
          transform.position =   Vector3.Lerp(startPos, targetPos, (elapsedtime / timeToMove));
          elapsedtime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }

    private IEnumerator ShootPlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedtime = 0;
        startPos = transform.position;
        targetPos = startPos + direction;

        while (elapsedtime < timeToMove && keepM == true)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedtime / timeToMove) *2);
            elapsedtime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
    }

    private IEnumerator callMovePlayer()
    {
        while (keepM == true)
        {
            StartCoroutine(ShootPlayer(shootDiriction));
            yield return null;
        }
    }

    private IEnumerator changeColor()
    {
        colorIndex = Random.Range(0, 3);
        yield return new WaitForSeconds(0.8f);
        
        playerMat.material = ballColorMaterial[colorIndex];
        gameObject.tag = tags[colorIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag != gameObject.tag)
        {  
            Debug.Log("DIFFRENT");
            keepM = false;
            transform.position = lastPos;
        }

        else  if (other.tag == gameObject.tag)
        {
            StartCoroutine(changeColor());
            
            if (other.transform.parent != null)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
