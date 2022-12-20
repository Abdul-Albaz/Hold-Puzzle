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
    public GameObject explosionParticle;
    public GameObject splatterParticle;
    public GameObject ringParticle;
    public Animator animator;

    public void Start()
    {
        animator.SetTrigger("BounceLeft");
        animator.SetTrigger("BounceRight");
        animator.SetTrigger("BounceLeft");
        animator.SetTrigger("BounceUp");

        GetComponent<SpriteRenderer>().sprite = manager.sprites[color];
       
        if (y > 0) neighbors.Add(manager.ballGrid[x, y - 1]);
        if (y < manager.width - 1) neighbors.Add(manager.ballGrid[x, y + 1]);
        if (x > 0) neighbors.Add(manager.ballGrid[x - 1, y]);
        if (x < manager.width - 1) neighbors.Add(manager.ballGrid[x + 1, y]); 
    }

    public async void Destroy()
    {
        float distance = Vector3.Magnitude(transform.position - PlayerManager.Instance.startPos);
        await Task.Delay((int)distance * duration);

        ParticleSystem();

        if (gameObject == null) return;
        else  Destroy(gameObject);
    }

    public void ParticleSystem()
    {
        ParticleSystem splatter = Instantiate(splatterParticle, transform.parent).GetComponent<ParticleSystem>();
        var mainColor = splatter.main;
        mainColor.startColor = GridManager.Instance.colorIndex[color];

        ParticleSystem ring = Instantiate(ringParticle, transform.parent).GetComponent<ParticleSystem>();
        var ringColor = ring.main;

        ringColor.startColor = GridManager.Instance.colorIndex[color];
        ParticleSystemRenderer particle = Instantiate(explosionParticle, transform.parent).GetComponent<ParticleSystemRenderer>();
        particle.material.color = GridManager.Instance.colorIndex[color];
    }
    
}
