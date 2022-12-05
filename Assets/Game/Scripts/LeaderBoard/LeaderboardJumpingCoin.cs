using DG.Tweening;
using UnityEngine;

public class LeaderboardJumpingCoin : MonoBehaviour
{
    private Transform target;

    public static void Init(GameObject prefab, Transform parent, Transform target)
    {
        LeaderboardJumpingCoin coin = Instantiate(prefab, parent).GetComponent<LeaderboardJumpingCoin>();
        coin.target = target;
        coin.Jump();
    }

    private void Jump()
    {
        //SoundManager.Play(AudioClips.coin);
        transform.DOJump(target.position, Random.Range(-2, 3), 1, 0.5f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            Taptic.Medium();
            LeaderboardManager.Instance.coinLabel.text = (int.Parse(LeaderboardManager.Instance.coinLabel.text) + 1).ToString();
            Destroy(gameObject);
        });
    }
}
