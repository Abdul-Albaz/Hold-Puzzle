using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UIElements;

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


    Direction direction;
    SpriteRenderer spriteRenderer;
    int color;

    void Start()
    {

        trail.gameObject.SetActive(false);
        direction = Direction.up;
        color = Random.Range(0, manager.sprites.Length);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = manager.sprites[color];

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
            MovePlayer(direction);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Up");
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (transform.position.x == -1 && transform.position.y == -1) return;
            else if (transform.position.x == -1 && transform.position.y == manager.gridSizeY) return;
            else if (transform.position.x == manager.gridSizeX && transform.position.y == -1) return;
            else if (transform.position.x == manager.gridSizeX && transform.position.y == manager.gridSizeY) return;

            lastPos = transform.position;

            shootDirection = direction == Direction.up ? Direction.right : direction == Direction.right ? Direction.down : direction == Direction.left ? Direction.up : Direction.left;
            ShootPlayer();
        }
    }

    private void MovePlayer(Direction direction)
    {
        trail.gameObject.SetActive(false);
        Vector3 moveDirection = direction == Direction.up ? Vector3.up : direction == Direction.right ? Vector3.right : direction == Direction.down ? Vector3.down : Vector3.left;
        isMoving = true;
        startPos = transform.position;
        targetPos = startPos + moveDirection;
        float distance = Vector3.Magnitude(targetPos - startPos);

        transform.DOMove(targetPos, distance / speed).SetEase(Ease.Linear).OnComplete(() => isMoving = false);
    }

    private void ShootPlayer()
    {
        trail.gameObject.SetActive(true);
        if (isMoving) return;
        isMoving = true;
        startPos = transform.position;

        Vector3 shootDirVector = shootDirection == Direction.up ? Vector3.up : shootDirection == Direction.right ? Vector3.right : shootDirection == Direction.down ? Vector3.down : Vector3.left;

        var targetBall = GetTargetBall();

        if (targetBall == null)
        { 
            transform.DOMove(startPos + shootDirVector * (manager.gridSizeX + 1), manager.gridSizeX / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                isMoving = false;
                Debug.Log(" manager.gridSizeX / speed " + manager.gridSizeX / speed);
                trail.gameObject.SetActive(false);
            });

            return;
        }


        if (targetBall.color != color)
        {
            
            float distanceTarget = Vector3.Magnitude(targetBall.transform.position - startPos);
            trail.transform.DOScale(0.5f, 0.2f).SetEase(Ease.InBounce);

            PlayerBounce(targetBall, distanceTarget);
            BallBounce(targetBall , distanceTarget);

            return;
        }

        Ball distantBall = targetBall;
        manager.ballsToDestroy.Clear();
        manager.CheckBalls(targetBall);

        while (targetBall != null)
        {
            targetBall = GetTargetBall();
            if (targetBall == null) break;

            if (targetBall.color != color) break;
            manager.CheckBalls(targetBall);
        }

        foreach (Ball ball in manager.ballsToDestroy)
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

        manager.DestroyBalls();
        manager.winLevel();
        trail.gameObject.SetActive(true);

        float distance = Vector3.Magnitude(distantBall.transform.position - startPos);

        transform.DOMove(distantBall.transform.position, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (BallShouldMoveForward(distantBall))
            {
                transform.DOMove(startPos + shootDirVector * (manager.gridSizeX + 1), manager.gridSizeX / speed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    trail.gameObject.SetActive(false);
                    manager.DestroyBalls();
                    isMoving = false;
                });
            }

            else
            {
                transform.DOMove(startPos, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    trail.gameObject.SetActive(false);
                    isMoving = false;
                    changeColor();
                });
            }
        });
    }

    private bool BallShouldMoveForward(Ball ball)
    {
        switch (shootDirection)
        {
            case Direction.right:

                Debug.Log("right free");

                for (int i = ball.x + 1; i < manager.gridSizeX; i++)
                {
                    Ball b = manager.ballGrid[i, (int)transform.position.y];
                    if (b == null || b.color == color) continue;

                    return false;
                }
                break;


            case Direction.left:

                Debug.Log("left free");

                for (int i = ball.x - 1; i >= 0; i--)
                {
                    Ball b = manager.ballGrid[i, (int)transform.position.y];
                    if (b == null || b.color == color) continue;
 
                    return false;
                }
                break;


            case Direction.up:

                Debug.Log("up free");

                for (int i = ball.y + 1; i < manager.gridSizeY; i++)
                {
                    Ball b = manager.ballGrid[(int)transform.position.x, i];
                    if (b == null || b.color == color) continue;
                   
                    return false;
                }
                break;


            case Direction.down:

                Debug.Log("down free");

                for (int i = ball.y - 1; i >= 0; i--)
                {
                    Ball b = manager.ballGrid[(int)transform.position.x, i];
                    if (b == null || b.color == color) continue;
                   
                    return false;
                }
                break;
        }
        return true;
    }

    private Ball GetTargetBall()
    {

        switch (shootDirection)
        {
            case Direction.up:
                for (int y = 0; y < manager.gridSizeY; y++)
                {
                    Ball ball = manager.ballGrid[(int)transform.position.x, y];
                    if (ball != null && !manager.ballsToDestroy.Contains(ball))
                        
                        return ball;  
                }
     
                break;

            case Direction.right:
                for (int x = 0; x < manager.gridSizeX; x++)
                {
                    Ball ball = manager.ballGrid[x, (int)transform.position.y];
                    if (ball != null && !manager.ballsToDestroy.Contains(ball))
                       
                    return ball;
    
                }
                break;

            case Direction.down:
                for (int y = manager.gridSizeY - 1; y >= 0; y--)
                {
                    Ball ball = manager.ballGrid[(int)transform.position.x, y];
                    if (ball != null && !manager.ballsToDestroy.Contains(ball))
                       
                    return ball;
                  
                }
                break;

            case Direction.left:
                for (int x = manager.gridSizeX - 1; x >= 0; x--)
                {
                    Ball ball = manager.ballGrid[x, (int)transform.position.y];
                    if (ball != null && !manager.ballsToDestroy.Contains(ball))
                        
                    return ball;

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


    public void BallBounce(Ball ball, float distanceTarget)
    {
        ball.transform.DOScale(0.8f, distanceTarget/6f).SetEase(Ease.InBounce).OnComplete(() =>
        {
            ball.transform.DOScale(1f, distanceTarget / 6f).SetEase(Ease.OutBounce);
        });
    }


    public void PlayerBounce(Ball targetBall, float distanceTarget)
    {
        transform.DOMove(targetBall.transform.position, distanceTarget / speed).SetEase(Ease.Linear).OnComplete(() =>
        {

            transform.DOScaleY(1.29f, 0.2f).SetEase(Ease.InBounce);
            transform.DOScaleX(0.8f, 0.2f).SetEase(Ease.InBounce);

            transform.DOMove(startPos, distanceTarget / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
                isMoving = false;
                changeColor();
            });
        });
    }


}
