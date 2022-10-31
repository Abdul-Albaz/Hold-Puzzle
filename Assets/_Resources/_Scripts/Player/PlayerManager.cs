using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : Singleton<PlayerManager>
{
    bool isMoving;

    public Vector3 startPos, targetPos;
    private Direction shootDirection;
    private float timeToMove = 0.2f;
    [SerializeField] private float speed = 5;

    public GridManager manager => GridManager.Instance;
    public Vector3 lastPos;
    public GameObject trail;
    public bool keepMove;

    Direction direction;
    SpriteRenderer spriteRenderer;
    public Ball targetBall;
    int color;

    void Start()
    {
        direction = Direction.up;
        color = Random.Range(0, manager.sprites.Length);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite= manager.sprites[color];

        switch (spriteRenderer.sprite.name)
        {
            case "red":
                color = 0;
                break;
            case "green":
                color = 1;
                break;
            case "orange":
                color = 2;
                break;
            case "purple":
                color = 3;
                break;
            case "blue":
                color = 4;
                break;
        }
        
        Debug.Log("spriteRenderer.name " + spriteRenderer.name);

        trail.GetComponent<TrailRenderer>().endColor = GridManager.Instance.colorIndex[color];
        trail.GetComponent<TrailRenderer>().startColor = GridManager.Instance.colorIndex[color];
    }

    void Update()
    {
        if (transform.position.x == -1 && transform.position.y < manager.gridSizeY) direction = Direction.up;
        else if (transform.position.x < manager.gridSizeX && transform.position.y == manager.gridSizeY) direction = Direction.right;
        else if (transform.position.x == manager.gridSizeX && transform.position.y > -1) direction = Direction.down;
        else direction = Direction.left;

        if (isMoving) return;

        if (Input.GetMouseButton(0))
        {
            keepMove = false;
            MovePlayer(direction);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (transform.position.x == -1 && transform.position.y == -1) return;
            else if (transform.position.x == -1 && transform.position.y == manager.gridSizeY) return;
            else if (transform.position.x == manager.gridSizeX && transform.position.y == -1) return;
            else if (transform.position.x == manager.gridSizeX && transform.position.y == manager.gridSizeY) return;

            lastPos = transform.position;
            keepMove = true;

            shootDirection = direction == Direction.up ? Direction.right : direction == Direction.right ? Direction.down : direction == Direction.left ? Direction.up : Direction.left;
            ShootPlayer();
        }

        
    }

    private void MovePlayer(Direction direction)
    {
        Vector3 moveDirection = direction == Direction.up ? Vector3.up : direction == Direction.right ? Vector3.right : direction == Direction.down ? Vector3.down : Vector3.left;
        isMoving = true;
        startPos = transform.position;
        targetPos = startPos + moveDirection;
        float distance = Vector3.Magnitude(targetPos - startPos);


        transform.DOMove(targetPos, distance / speed).SetEase(Ease.Linear).OnComplete(() => isMoving = false);
    }

    private void ShootPlayer()
    {
        if (isMoving) return;
        isMoving = true;
        startPos = transform.position;

        Vector3 shootDirVector = shootDirection == Direction.up ? Vector3.up : shootDirection == Direction.right ? Vector3.right : shootDirection == Direction.down ? Vector3.down : Vector3.left;

        Ball targetBall = GetTargetBall();
        this.targetBall = targetBall;

        if (targetBall == null)
        {
            transform.DOMove(startPos + shootDirVector * (manager.gridSizeX + 1), manager.gridSizeX / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                keepMove = false;
                isMoving = false;
            });
            return;
        }

        if (targetBall.color != color)
        {

            float distance = Vector3.Magnitude(targetBall.transform.position - startPos);

            transform.DOMove(targetBall.transform.position, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOMove(startPos, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    isMoving = false;
                    changeColor();
                });
            });


            //isMoving = false;
            //return;
        }

        Ball distantBall = targetBall;

        manager.CheckBalls(targetBall);

        foreach(Ball ball in manager.ballsToDestroy)
        {
            switch (shootDirection)
            {
                case Direction.up:
                    if (ball.x == distantBall.x & ball.y > distantBall.y) distantBall = ball;
                    break;
                case Direction.right:
                    if (ball.x > distantBall.x & ball.y == distantBall.y) distantBall = ball;
                    break;
                case Direction.down:
                    if (ball.x == distantBall.x & ball.y < distantBall.y) distantBall = ball;
                    break;
                case Direction.left:
                    if (ball.x < distantBall.x & ball.y == distantBall.y) distantBall = ball;
                    break;
            }
        }

        if (targetBall.color == color)
        {
            manager.DestroyBalls();

            //isMoving = false;
            //return;


            print("ball should move to: " + distantBall.transform.position + " color " + distantBall.color);

            float distance = Vector3.Magnitude(distantBall.transform.position - startPos);

            transform.DOMove(distantBall.transform.position, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOMove(startPos, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    isMoving = false;
                    changeColor();
                });
            });
        }
    }

    private Ball GetTargetBall()
    {
        
        switch (shootDirection)
        {
            case Direction.up:
                for(int y = 0; y < manager.gridSizeY; y++)
                {
                    Ball ball = manager.ballGrid[(int)transform.position.x, y];
                    if (ball != null) return ball;
                }
                break;

            case Direction.right:
                for (int x = 0; x < manager.gridSizeX; x++)
                {
                    Ball ball = manager.ballGrid[x, (int)transform.position.y];
                    if (ball != null) return ball;
                }
                break;

            case Direction.down:
                for (int y = manager.gridSizeY - 1; y >= 0; y--)
                {
                    Ball ball = manager.ballGrid[(int)transform.position.x, y];
                    if (ball != null) return ball;
                }
                break;

            case Direction.left:
                for (int x = manager.gridSizeX - 1; x >= 0; x--)
                {
                    Ball ball = manager.ballGrid[x, (int)transform.position.y];
                    if (ball != null) return ball;
                }
                break;
        }

        return null;
    }


    private async void changeColor()
    {
        color = Random.Range(0, manager.sprites.Length);
        await Task.Delay(500);

        spriteRenderer.sprite = manager.sprites[color];

        // Trail Color
        switch (spriteRenderer.sprite.name)
        {
            case "red":
                color = 0;
                break;
            case "green":
                color = 1;
                break;
            case "orange":
                color = 2;
                break;
            case "purple":
                color = 3;
                break;
            case "blue":
                color = 4;
                break;
        }


        trail.GetComponent<TrailRenderer>().endColor = GridManager.Instance.colorIndex[color];
        trail.GetComponent<TrailRenderer>().startColor = GridManager.Instance.colorIndex[color];
    }

    private void HandleCollision(Ball ball)
    {
        //changeColor();
    }

}
