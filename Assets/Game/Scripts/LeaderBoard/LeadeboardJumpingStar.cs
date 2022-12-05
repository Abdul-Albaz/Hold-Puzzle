using DG.Tweening;
using UnityEngine;

public class LeadeboardJumpingStar : MonoBehaviour
{
    private Transform target;

    public static void Init(GameObject prefab, Transform parent, Transform target)
    {
        LeadeboardJumpingStar star = Instantiate(prefab, parent).GetComponent<LeadeboardJumpingStar>();
        star.target = target;
        star.Jump();
    }

    private void Jump()
    {
        Taptic.Medium();
//        SoundManager.Play(AudioClips.star);
        transform.DOJump(target.position, Random.Range(-2, 3), 1, 0.5f).SetEase(Ease.InOutSine).OnComplete(() => Destroy(gameObject));
    }
}