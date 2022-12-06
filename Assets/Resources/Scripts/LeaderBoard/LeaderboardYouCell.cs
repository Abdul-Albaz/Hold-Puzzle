using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LeaderboardYouCell : MonoBehaviour
{
    [SerializeField] private Text rank;
    [SerializeField] private Image avatar;
    [SerializeField] public GameObject star;
    private int currentRank;
    private ProfileManager manager => ProfileManager.Instance;

    void Start()
    {
        if (manager.avatarNumber < 0) manager.avatarNumber = Random.Range(1, 5);
        LeaderboardManager.Instance.userPanel = this;
        transform.localScale = Vector3.one * 1.1f;
        currentRank = LeaderboardManager.Instance.rankPrevious;
        rank.text = $"{currentRank}";
        avatar.sprite = manager.avatarSprite;
    }

    public void animateRank() {
        DOTween.To(
            () => currentRank - 1,
            x =>
            {
                currentRank = x;
                rank.text = $"{x}";
            },
            LeaderboardManager.Instance.rank, 2.5f
            )
            .SetEase(Ease.OutCubic);
            //.SetDelay(1f);
    }

    public void animateScale() => transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutSine);
}
