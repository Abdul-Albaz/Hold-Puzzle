using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : Singleton<CoinManager>
{
    public int count
    {
        get => PlayerPrefs.GetInt("coins", 0);
        set => PlayerPrefs.SetInt("coins", value);
    }

    public int latestTransactionAmount
    {
        get => PlayerPrefs.GetInt("coinsLastTransaction", 0);
        set => PlayerPrefs.SetInt("coinsLastTransaction", value);
    }

    [SerializeField] private Text label;
    [SerializeField] private Transform target;
    private GameObject coinPrefab;

    void Start()
    {
        //label.text = count.ToString();
       // coinPrefab = Resources.Load<GameObject>("coinPrefab");
    }

    public async void add(int value)
    {
        latestTransactionAmount = value;
        count += value;

        for (int i = 0; i < value; i++)
        {
            //LeaderboardJumpingCoin.Init(coinPrefab, UIManager.Instance.MovesBox.transform, target);
            await Task.Delay(100);
        }
    }

    public void subtract(int value)
    {
        latestTransactionAmount = value;
        count -= value;
    }

    public void increment()
    {
        var temp = int.Parse(label.text);
        label.text = (temp + 1).ToString();
    }

    public void enter()
    {
        //if (GameManager.Instance.level.level <= 3) return;
        transform.DOLocalMoveX(GameManager.Instance.aspect ? 723 : 800, 0.5f).SetEase(Ease.OutSine);
        label.text = count.ToString();
    }

    public void exit()
    {
        transform.DOLocalMoveX(1255, 0.5f).SetEase(Ease.InSine);
    }
}
