using DG.Tweening;
using UnityEngine;

public class JumpingStar : MonoBehaviour
{
    private Transform target;
   // private LevelManager manager => LevelManager.Instance;

    public static void Init(GameObject prefab, Transform parent, Transform target)
    {
        JumpingStar star = Instantiate(prefab, parent).GetComponent<JumpingStar>();
        star.target = target;
        star.Jump();
    }

    private void Jump()
    {
        Taptic.Medium();
     // SoundManager.Play(AudioClips.star);
        GameManager.Instance.movesLeft.text = (int.Parse(GameManager.Instance.movesLeft.text) - 1).ToString();
        transform.DOJump(target.position, Random.Range(-2, 3), 1, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => {
        StarManager.Instance.increment();
        
        Destroy(gameObject);

        });
    }
}