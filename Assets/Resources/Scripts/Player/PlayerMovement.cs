using System.Collections;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    bool isMoving;

    private Vector3 startPos, targetPos;
    private float timeToMove = 0.15f;
    private Vector3 shootDiriction;

    public TileManager tileManager;
    public GameObject trail;
    public Vector3 lastPos;
    public bool keepMove;
    public Material[] ballColorMaterial;
    public string[] tags;

    Direction direction;
    MeshRenderer playerMat;
    int colorIndex;


    void Start()
    {
        direction = Direction.up;
        colorIndex = Random.Range(0, 3);
        playerMat = gameObject.GetComponent<MeshRenderer>();
        playerMat.material = ballColorMaterial[colorIndex];
        gameObject.tag = tags[colorIndex];

        trail.GetComponent<TrailRenderer>().endColor = ballColorMaterial[colorIndex].color;
        trail.GetComponent<TrailRenderer>().startColor = ballColorMaterial[colorIndex].color;
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if (transform.position.x == -1 && transform.position.y == -1) direction = Direction.up;
        if (transform.position.x == -1 && transform.position.y == (tileManager.gridSizeY + 1)) direction = Direction.right;
        if (transform.position.x == (tileManager.gridSizeX + 1) && transform.position.y == (tileManager.gridSizeY + 1)) direction = Direction.down;
        if (transform.position.x == (tileManager.gridSizeX + 1) && transform.position.y == -1) direction = Direction.left;


        if (Input.GetMouseButton(0) && !isMoving)
        {
            keepMove = false;
            switch (direction)
            {
                case Direction.up:shootDiriction = Vector3.right;
                    StartCoroutine(MovePlayer(Vector3.up));break;

                case Direction.down:shootDiriction = Vector3.left;
                    StartCoroutine(MovePlayer(Vector3.down));break;

                case Direction.left:shootDiriction = Vector3.up;
                    StartCoroutine(MovePlayer(Vector3.left));break;

                case Direction.right:shootDiriction = Vector3.down;
                    StartCoroutine(MovePlayer(Vector3.right)); break;

                default:
                    break;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && !isMoving)
        {
            lastPos = transform.position;
            keepMove = true;
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

        while (elapsedtime < timeToMove && keepMove == true)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedtime / timeToMove) *2);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    private IEnumerator callMovePlayer()
    {
        while (keepMove == true)
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
        trail.GetComponent<TrailRenderer>().endColor = ballColorMaterial[colorIndex].color;
        trail.GetComponent<TrailRenderer>().startColor = ballColorMaterial[colorIndex].color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.tag == "wall")
        {
            Debug.Log("DIFFRENT");
            keepMove = false;
            transform.position = transform.position;
            transform.position = lastPos;
        }

        else if (other.tag != gameObject.tag)
        {  
            Debug.Log("DIFFRENT");
            keepMove = false;
            transform.position = lastPos;
        }

        else  if (other.tag == gameObject.tag)
        {
            StartCoroutine(changeColor());
            
            if (other.transform.parent != null) Destroy(other.transform.parent.gameObject);  
            else Destroy(other.gameObject);
        }
    }
}
