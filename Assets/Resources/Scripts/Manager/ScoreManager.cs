using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    GameManager manager => GameManager.Instance;
    //LevelManager levelManager => LevelManager.Instance;

    [SerializeField] private GameObject scoreBar;
    //[SerializeField] private GameObject empty;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Transform scoreBox;
    [SerializeField] private Transform bestScoreBox;



    public Transform panel;
    public RectTransform panelRect;
    //public Image overlay;


    private int score = 0;
    private string scoreKey => $"BestScore{manager.numberOfRows}x{manager.numberOfCols}";

    private Tween scoreTween = null;
    private Tween bestScoreTween = null;

    private void Start()
    {
        //int pos = 120 + scoreText.text.Length * 25;
        //scoreBox.DOLocalMoveX(pos, 0);
    }

    internal void Reset()
    {
       // score = 0;
        //scoreBar.SetActive(manager.mode == GameModes.endless);

       // scoreText.text = score.ToString();
       // bestScoreText.text = PlayerPrefs.GetInt(scoreKey).ToString();
    }

    public void AddScore(int value)
    {
        Taptic.Light();
        score += value;
        Shake(scoreBox, scoreTween);
        scoreText.text = score.ToString();
        //int pos = 120 + scoreText.text.Length * 25;
        //scoreBox.DOLocalMoveX(pos, 0.2f);

        if (score > PlayerPrefs.GetInt(scoreKey))
        {
            PlayerPrefs.SetInt(scoreKey, score);
            bestScoreText.text = PlayerPrefs.GetInt(scoreKey).ToString();
            Shake(bestScoreBox, bestScoreTween);
        }
    }

    private void Shake(Transform transform, Tween tween)
    {
        bool shouldAnimate = false;

        if (tween == null) shouldAnimate = true;
        else if (!tween.IsPlaying()) shouldAnimate = true;
        if (shouldAnimate) tween = transform.DOScale(1.25f, 0.25f).OnComplete(() => transform.DOScale(1, 0.1f));
    }

    internal void Enter()
    {
        //if (levelManager.levelNumber < 2)
        //{
        GameManager.Instance.moveCompleted();
        return;

        var top = panel.GetComponent<HorizontalLayoutGroup>();
        top.padding.right = -100;
        top.padding.left = 465;
        LayoutRebuilder.MarkLayoutForRebuild(panelRect);

        //overlay.gameObject.SetActive(true);
        panel.gameObject.SetActive(true);
        //overlay.DOColor(new Color(0, 0, 0, 0.3f), levelManager.inDuration);

        panel.DOLocalMoveY(0, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            animatePadding();
            // overlay.DOColor(new Color(0, 0, 0, 0), levelManager.outDuration).SetDelay(levelManager.delay).OnComplete(() => overlay.gameObject.SetActive(false));
            panel.DOLocalMoveY(1150, 1f).SetEase(Ease.InBack).SetDelay(0.5f);

        });
    }

    internal void Exit()
    {
        panel.DOLocalMoveY(1800, 0.2f).SetEase(Ease.InBack).OnComplete(() => panel.gameObject.SetActive(false));
    }

    private void animatePadding()
    {
        var top = panel.GetComponent<HorizontalLayoutGroup>();
        DOTween.To(
          () => top.padding.top + 5,
          x =>
          {
              top.padding.top = x;
              LayoutRebuilder.MarkLayoutForRebuild(panelRect);
          },
          110,
         1f
          )
          .SetDelay(1f)
          .SetEase(Ease.InBack).OnComplete(() =>
          {
              DOTween.To(
              () => top.padding.left + 10,
              x =>
              {
                  top.padding.left = x;
                  LayoutRebuilder.MarkLayoutForRebuild(panelRect);
              },
              550,
              1f
              )
              .SetDelay(1f)
              .SetEase(Ease.InBack);

              DOTween.To(
              () => top.padding.right + 10,
              x =>
              {
                  top.padding.right = x;
                  LayoutRebuilder.MarkLayoutForRebuild(panelRect);
              },
              150,
              1f
              )
              .SetDelay(1f)
              .SetEase(Ease.InBack);
          });
    }

}
