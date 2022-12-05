using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class LeaderboardManager : Singleton<LeaderboardManager>
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform target;
    [SerializeField] internal GameObject youCellPrefab;
    [SerializeField] internal GameObject userCellPrefab;

    public TextAsset Username;
    internal string[] names;
    internal List<string> usedNames = new();
    public GameObject topChart;

    public GameObject scrollView;
    [SerializeField] public Text coinLabel;
    [SerializeField] public Text starLabel;

    [SerializeField] private Transform nextLevelButton;
    [SerializeField] private Transform flare;

    public Tween flareTween;

    public LeaderboardYouCell userPanel;

    public int numberOfPlayers = 30;
    internal int rank
    {
        get => PlayerPrefs.GetInt("rank", 1000);
        set => PlayerPrefs.SetInt("rank", value);
    }

    internal int rankPrevious = 1000;
    internal string startOfTheWeek
    {
        get => PlayerPrefs.GetString("startOfTheWeek", "2022-08-15");
        set => PlayerPrefs.SetString("startOfTheWeek", value);
    }
    public List<LeaderboardCell> players = new();

    public void Enter()
    {
        nextLevelButton.localPosition += Vector3.down * 300;
        flareTween = flare.DOLocalRotate(new(0, 0, 360), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        CheckForNewWeek();   

        coinLabel.text = CoinManager.Instance.count.ToString();
        starLabel.text = StarManager.Instance.latestTransactionAmount.ToString();

        var delta = UnityEngine.Random.Range(10, rank == 1 ? 150 : 50);
        rank += delta;
        rankPrevious = rank;
        delta = Math.Min(rank - 1, UnityEngine.Random.Range(30, 70));
        rank -= delta;
        numberOfPlayers = Math.Max(20, delta); // + (rank < 3 ? rank : 3);

        players.Clear();
        names = Username.text.Split(",", StringSplitOptions.None);

        Transform user = Instantiate(youCellPrefab, transform.GetChild(1).GetChild(0)).transform;
        if (rank <= 3) user.localPosition = new(user.localPosition.x, user.localPosition.y + (110 / rank), user.localPosition.z);
        Instantiate(topChart, transform.GetChild(2));

        for (int i = Math.Max(1, rank - 3); i < rankPrevious; i++)
            LeaderboardCell.Init(userCellPrefab, i, transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0));

        TopChartManager.Instance.set();
        animate();
    }

    private void CheckForNewWeek()
    {
        DateTime weekStart = DateTime.Parse(startOfTheWeek);
        if ((DateTime.Today - weekStart).Days > 7)
        {
            rank = 1000;
            startOfTheWeek = $"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day}";
        }
    }

    public async void animate()
    {
        CoinManager.Instance.count += StarManager.Instance.latestTransactionAmount;
        scrollView.transform.localPosition = new(scrollView.transform.localPosition.x, numberOfPlayers * players[0].height, 0);
        await Task.Delay(250);
        for (int i = 0; i < StarManager.Instance.latestTransactionAmount; i++)
        {
            LeaderboardJumpingCoin.Init(coinPrefab, parent, target);
            await Task.Delay(100);
        }
        await Task.Delay(250);
        nextLevelButton.DOLocalMoveY(nextLevelButton.localPosition.y + 300, 0.25f).SetEase(Ease.OutSine);
        userPanel.animateRank();
        tick();
        scrollView.transform.DOLocalMoveY(-players[0].height * (7 - Math.Min(4, rank)) + ((rank < 3 ? 110 / rank : 0) + 23.5f), 2.5f).SetEase(Ease.OutCubic).OnComplete(() => userPanel.animateScale());
        for (int i = 0; i < StarManager.Instance.latestTransactionAmount; i++)
        {
            LeadeboardJumpingStar.Init(starPrefab, parent, userPanel.star.transform);
            await Task.Delay(2000 / StarManager.Instance.latestTransactionAmount);
        }
    }

    private async void tick(int delay = 30)
    {
         //if (GameManager.Instance.state == LevelState.waiting) return;
        //if (delay > 500) return;
        //if (delay == 30) await Task.Delay(1000);
       // SoundManager.Play(AudioClips.tick);
        Taptic.Light();
        await Task.Delay(delay);
        try { tick((int)(delay * 1.2)); } catch { }
    }

    private void OnDisable()
    {
        try
        {
            if(flareTween != null) flareTween.Kill();
            foreach (Transform child in transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0)) Destroy(child.gameObject);
            Destroy(transform.GetChild(2).GetChild(0).gameObject);
            Destroy(transform.GetChild(1).GetChild(0).GetChild(1).gameObject);
        } catch { }
        
    }
}

public enum Countries
{
    lv, cn, co, vn, td, lu, cz, cm, ar, bh, cl, lt, tr, ua, no, mc, ch, ci, us, tw, ye, nl, at, ck, au, ma, nz, pl, rs, ge, gr, hk, kr, dk, il, ru, kw, gb, sg, se, jp, fr, ph, qa, pe, pr, si, es, fi, ee, hu, id, eg, fj, ie, ro, sk, pt, hr, jm, ec, fo, sl, de, it, ml, cg, ax, ve, mx, cr, my, th, br, be, ca, ng, mk, ne, cu, bg, uy
}