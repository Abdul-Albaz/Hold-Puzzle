using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GridManager manager => GridManager.Instance;

    public int x = 0;
    public int y = 0;
    public int color;

    public int duration;

    public List<Ball> ballsToDestroy = new ();
    public List<Ball> neighbors = new ();


    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = manager.sprites[color];
       
        if (y > 0) neighbors.Add(manager.ballGrid[x, y - 1]);

        if (y < manager.width - 1) neighbors.Add(manager.ballGrid[x, y + 1]);

        if (x > 0) neighbors.Add(manager.ballGrid[x - 1, y]);

        if (x < manager.width - 1) neighbors.Add(manager.ballGrid[x + 1, y]);
    }

    public async void Destroy()
    {
        Vector3 targetPos = PlayerMovement.Instance.targetBall.transform.position;
        float distance = Vector3.Magnitude(targetPos - transform.position);

        await Task.Delay((int)distance * duration);

        Destroy(gameObject);
    }

}
