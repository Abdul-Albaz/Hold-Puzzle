using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class PlayerManager : Singleton<PlayerManager>
{
    public bool isMoving;
    public bool isInTouch = false;
    
    public int moveCounter;

    public Vector3 startPos, targetPos;
    private int posX,posY, posZ;
    public Direction shootDirection;
    [SerializeField] private float speed = 8;
    private GridManager manager => GridManager.Instance;
    public Vector3 lastPos;
    public GameObject trail;
    private Tween tween;
    Direction direction;
    SpriteRenderer spriteRenderer;
    int color;
    void Start()
    {
        startPos = transform.position;
        moveCounter = 0;
        trail.gameObject.SetActive(false);
        direction = Direction.up;
        color = Random.Range(0, manager.sprites.Length);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = manager.sprites[color];

        changeColor();
    }

    void Update()
    {
         isInTouch = Input.touchCount > 0;
        
         if (isMoving) return;
         if (!isInTouch)  return;
        // if(Input.GetMouseButtonUp(0)) isInTouch = false;
        // if(Input.GetMouseButtonDown(0)) isInTouch = true;true

        

        Debug.Log("shoot update");

        if (transform.position.x == -1 && transform.position.y < manager.gridSizeY) {direction = Direction.up; } 
        else if (transform.position.x < manager.gridSizeX && transform.position.y == manager.gridSizeY) {direction = Direction.right; }
        else if (transform.position.x == manager.gridSizeX && transform.position.y > -1) {direction = Direction.down; }
        else  {direction = Direction.left; } 
        
        Debug.Log("isMoving " + isMoving);

        
        MovePlayer();
    }

    private void MovePlayer()
    {
        isMoving = true;
      
        Debug.Log("shoot MovePlayer");
        
        trail.SetActive(false);
        Vector3 moveDirection = direction == Direction.up ? Vector3.up : direction == Direction.right ? Vector3.right : direction == Direction.down ? Vector3.down : Vector3.left;
        startPos = transform.position;
        targetPos = startPos + moveDirection;

        float distance = Vector3.Magnitude( targetPos - startPos );
        
        tween = transform.DOMove(targetPos, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (!isInTouch) MoveAndPop();
            else isMoving = false;
            
            Debug.Log("isMoving " + isMoving);
        });
        
      
    }

    private void MoveAndPop()
    {
        Debug.Log("shoot MoveAndPop");
        
        if (transform.position.x == -1 && transform.position.y == -1) return;
        if (transform.position.x == -1 && transform.position.y == manager.gridSizeY) return;
        if (transform.position.x == manager.gridSizeX && transform.position.y == -1)  return;
        if (transform.position.x == manager.gridSizeX && transform.position.y == manager.gridSizeY) return;

        moveCounter++;
        Debug.Log("shoot shootDirection Move And Pop "+ shootDirection);
        shootDirection = direction == Direction.up ? Direction.right : direction == Direction.right ? Direction.down : direction == Direction.left ? Direction.up : Direction.left;
        Debug.Log("shoot Move And Pop");
        ShootPlayer();
    }
    private void ShootPlayer()
    {
        // TODO NEED TO MAKE IT BETTER

        // isMoving = true;
        Debug.Log("shootDirection : "+shootDirection);
        Debug.Log("shoot ShootPlayer");
        trail.SetActive(true);

        posX = Mathf.RoundToInt(transform.position.x);
        posY = Mathf.RoundToInt(transform.position.y);
        posZ = Mathf.RoundToInt(transform.position.z);

        startPos = new Vector3(posX, posY , posZ);

        Vector3 shootDirVector = shootDirection == Direction.up ? Vector3.up : shootDirection == Direction.right ? Vector3.right : shootDirection == Direction.down ? Vector3.down : Vector3.left;
        
         var targetBall = GetTargetBall();
       
        if (targetBall is null )
        { 
            Debug.Log("shoot null");
            SoundManager.Play(AudioClips.move);
            transform.DOMove(startPos + shootDirVector * (manager.gridSizeX + 1), manager.gridSizeX / speed).SetEase(Ease.Linear).OnComplete(() =>
            {
                isMoving = false;
                Debug.Log(" manager.gridSizeX / speed " + manager.gridSizeX / speed);
                trail.SetActive(false);
            });
            Taptic.Medium();
            return;
        }

        Debug.Log("shoot ball color : " + targetBall.GetComponent<SpriteRenderer>().sprite.name);
        if (targetBall.color != color)
        {
            Debug.Log("shoot not same color");
            float distanceTarget = Vector3.Magnitude(targetBall.transform.position - startPos);
            trail.transform.DOScale(0.5f, 0.2f).SetEase(Ease.InBounce);
            AnimateSquish(targetBall, distanceTarget, shootDirection);
            AnimateBouncingBall(targetBall);
            Taptic.Medium();
            return;
        }

        Ball distantBall = targetBall;
        manager.ballsToDestroy.Clear();
        manager.CheckBalls(targetBall);

        while (targetBall  is not  null)
        {
            Taptic.Medium();
            targetBall = GetTargetBall();
            if (targetBall is null) break;

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
        SoundManager.Play(AudioClips.button);

        manager.winLevel();
        trail.gameObject.SetActive(true);

        float distance = Vector3.Magnitude(distantBall.transform.position - startPos);

        tween =  transform.DOMove(distantBall.transform.position, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (BallShouldMoveForward(distantBall))
            {
                int dist = shootDirection == Direction.left ? manager.gridSizeX - distantBall.x : shootDirection == Direction.right ? distantBall.x : shootDirection == Direction.down ? manager.gridSizeY - distantBall.y : distantBall.y;
                print("dist " + dist);

                    transform.DOMove(startPos + shootDirVector * (manager.gridSizeX + 1), manager.gridSizeX / (speed * dist)).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        trail.SetActive(false);
                        manager.DestroyBalls();
                        // isMoving = false;
                    });
            }

            else
            {
                tween =  transform.DOMove(startPos, distance / speed).SetEase(Ease.Linear).OnComplete(() =>
                {
                    trail.SetActive(false);
                    isMoving = false;
                    //changeColor();
                });
            }
        });
    }

    private bool BallShouldMoveForward(Ball ball)
    {
        isMoving = false;
        switch (shootDirection)
        {
            case Direction.right:

                Debug.Log("right free");

                for (int i = ball.x + 1; i < manager.gridSizeX; i++)
                {
                    Ball b = manager.ballGrid[i, (int)transform.position.y];
                    if (b is null || b.color == color) continue;

                    return false;
                }
                break;


            case Direction.left:

                Debug.Log("left free");

                for (int i = ball.x - 1; i >= 0; i--)
                {
                    Ball b = manager.ballGrid[i, (int)transform.position.y];
                    if (b is null || b.color == color) continue;
 
                    return false;
                }
                break;


            case Direction.up:

                Debug.Log("up free");

                for (int i = ball.y + 1; i < manager.gridSizeY; i++)
                {
                    Ball b = manager.ballGrid[(int)transform.position.x, i];
                    if (b is null || b.color == color) continue;
                   
                    return false;
                }
                break;


            case Direction.down:

                Debug.Log("down free");

                for (int i = ball.y - 1; i >= 0; i--)
                {
                    Ball b = manager.ballGrid[(int)transform.position.x, i];
                    if (b is null || b.color == color) continue;
                   
                    return false;
                }
                break;
        }
        return true;
    }

    private Ball GetTargetBall()
    {
        Debug.Log("get targetball ");
        
        switch (shootDirection)
        {
            case Direction.up:
                
                Debug.Log("get targetball up ");
                for (int y = 0; y < manager.gridSizeY; y++)
                {
                  
                    Debug.Log("get ______________ " + (int)transform.position.x);
                   
                    
                    Ball ball = manager.ballGrid[(int)transform.position.x, y];
                    print("get  manager.ballGrid");
                    
                    if (ball is null)
                    {
                        print("get ball null");
                        continue;
                    }
                    
                    if (manager.ballsToDestroy.Contains(ball)) continue;
                    Debug.Log("get return ball up "+ ball.GetComponent<SpriteRenderer>().sprite.name);  return ball;
                }
                Debug.Log("get ballsToDestroy ");
                break;
            
            

            case Direction.right:
                
                Debug.Log("get targetball right ");
                for (int x = 0; x < manager.gridSizeX; x++)
                {
                    Debug.Log($"get ______________ {x}, {(int)transform.position.y}");
                    
                    
                    Ball ball = manager.ballGrid[x, (int)transform.position.y];
                    if (ball is null) 
                    {
                        print("get ball null");
                        continue;
                    }
                    
                    Debug.Log($"ball: {ball.x}, {ball.y}");
                    if (manager.ballsToDestroy.Contains(ball)) continue;
                    Debug.Log("get return ball right " + ball.GetComponent<SpriteRenderer>().sprite.name);  return ball;
                }
                Debug.Log("get ballsToDestroy ");
                break;

            case Direction.down:
                
                Debug.Log("get targetball down");
                for (int y = manager.gridSizeY - 1; y >= 0; y--)
                {
                    Debug.Log("get ______________ " + (int)transform.position.x);
                    
                    
                    Ball ball = manager.ballGrid[(int)transform.position.x, y];
                    if (ball is null) 
                    {
                        print("get ball null");
                        continue;
                    }
                    if (manager.ballsToDestroy.Contains(ball)) continue;
                    Debug.Log("get return ball down " + ball.GetComponent<SpriteRenderer>().sprite.name);  return ball;
                    
                }
                Debug.Log("get ballsToDestroy ");
                break;
            

            case Direction.left:
                
                Debug.Log("get targetball left");
                for (int x = manager.gridSizeX - 1; x >= 0; x--)
                {
                    Debug.Log("get ______________ " + (int)transform.position.y);
                 
                    
                    Ball ball = manager.ballGrid[x, (int)transform.position.y];
                    if (ball is null) 
                    {
                        print("get ball null");
                        continue;
                    }
                    if (manager.ballsToDestroy.Contains(ball)) continue;
                    Debug.Log("get return ball left" + ball.GetComponent<SpriteRenderer>().sprite.name) ;  return ball;
                   
                }
                Debug.Log("get ballsToDestroy ");
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
        //Todo changeColor();
    }


    public void AnimateBouncingBall(Ball ball)
    {

        int ballDist = shootDirection == Direction.left ? manager.gridSizeX - ball.x: shootDirection == Direction.right ? ball.x : shootDirection == Direction.down ? manager.gridSizeY - ball.y : ball.y;

        tween =  ball.transform.DOScale(0.8f, ballDist / speed).SetDelay(ballDist / speed).SetEase(Ease.InBounce).OnComplete(() =>
        {
            tween =  ball.transform.DOScale(1f, 0.1f).SetEase(Ease.OutBounce);
        });

    }

    public void AnimateSquish(Ball targetBall, float distanceTarget, Direction direction)
    {
        Vector3 target = targetBall.color == color ? targetBall.transform.position : targetBall.transform.position + (shootDirection == Direction.left ? Vector3.right *0.75f : shootDirection == Direction.right ? Vector3.left * 0.75f : shootDirection == Direction.down ? Vector3.up * 0.75f : Vector3.down * 0.75f);

        tween = transform.DOMove(target, distanceTarget / speed).SetEase(Ease.Linear).OnComplete(() =>
        {
            SoundManager.Play(AudioClips.noAvailableMove);

            tween = transform.DOMove(startPos, distanceTarget / speed).SetEase(Ease.Linear).SetDelay(0.1f).OnComplete(() =>
            { 
                tween = transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce);
                isMoving = false;
                
                //changeColor();
            });

            if (direction == Direction.up || direction == Direction.down)
            {
                tween =  transform.DOScaleY(0.8f, 1 / speed).SetEase(Ease.InBounce).OnComplete(() =>
                {
                    tween =    transform.DOScaleY(1f, 1 / speed).SetEase(Ease.InBounce);
                });

            } else
            {
                tween = transform.DOScaleX(0.8f, 1 / speed).SetEase(Ease.InBounce).OnComplete(() =>
                {
                    tween =   transform.DOScaleX(1f, 1 / speed).SetEase(Ease.InBounce);
                });
            }
        });

        if (direction == Direction.up || direction == Direction.down)
        {
            tween =    transform.DOScaleX(0.8f, distanceTarget / speed).SetEase(Ease.InBounce);
            tween = transform.DOScaleY(1.29f, distanceTarget / speed).SetEase(Ease.InBounce);
        }
        else
        {
            tween =   transform.DOScaleX(1.29f, distanceTarget / speed).SetEase(Ease.InBounce);
            tween = transform.DOScaleY(0.8f, distanceTarget / speed).SetEase(Ease.InBounce);
        }
    }


}
