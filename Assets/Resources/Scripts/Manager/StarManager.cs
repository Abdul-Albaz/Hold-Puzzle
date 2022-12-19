using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarManager : Singleton<StarManager>
{
    public int count
    {
        get => PlayerPrefs.GetInt("stars", 0);
        set => PlayerPrefs.SetInt("stars", value);
    }

    public int latestTransactionAmount
    {
        get => PlayerPrefs.GetInt("starsLastTransaction", 0);
        set => PlayerPrefs.SetInt("starsLastTransaction", value);
    }

    [SerializeField] private Text label;
    [SerializeField] private Transform target;
    private GameObject starPrefab;

    void Start()
    {
      // label.text = count.ToString();
     //  starPrefab = Resources.Load<GameObject>("starPrefab");
    }

    public async void add(int value)
    {

        latestTransactionAmount = value;
        count += value;

        
        label.text = count.ToString();
       
       
        for (int i = 0; i < value; i++)
        {
            JumpingStar.Init(starPrefab,UIManager.Instance.MovesBox.transform, target);
            await Task.Delay(500);   
        }

    }

    public void subtract(int value)
    {
        latestTransactionAmount = value;
        count -= value;
    }

    public async void increment()
    {
        var temp = int.Parse(label.text);
        label.text = (temp).ToString();

        if (int.Parse(label.text) == count)
        {
            await Task.Delay(500);
            CoinManager.Instance.exit();
            exit();
            UIManager.Instance.MovesBoxExit();

            //await Task.Delay(600);
            //UIManager.Instance.setTransition(Views.leaderboard);
        }

    }

    public void enter()
    {
        transform.DOLocalMoveX(GameManager.Instance.aspect ? -717 : -800, 0.5f).SetEase(Ease.OutSine);
    }

    public void exit()
    {
        transform.DOLocalMoveX(-1255, 0.5f).SetEase(Ease.InSine);
    }
}
