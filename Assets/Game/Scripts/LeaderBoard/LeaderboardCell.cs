using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardCell : MonoBehaviour
{
    public int rank;
    internal string username;
    internal Countries country;
    internal float height => GetComponent<RectTransform>().rect.height;
    [SerializeField] private Text rankText, usernameText;
    [SerializeField] private Image countryImage;


    public static LeaderboardCell Init(GameObject prefab, int rank, Transform parent)
    {
        var player = Instantiate(prefab, parent).GetComponent<LeaderboardCell>();
        player.setRank(rank);
        player.setName();
        player.setCountry();
        if (rank == LeaderboardManager.Instance.rank) player.Close();

        LeaderboardManager.Instance.players.Add(player);

        return player;
    }

    private void setRank(int rank)
    {
        this.rank = rank;
        rankText.text = $"{rank}";
    }

    private void setCountry()
    {
        country = getRandomCountry();
        countryImage.sprite = Resources.Load<Sprite>($"flags/{country}");
    }

    private void setName()
    {
        username = getRandomName();
        usernameText.text = username;
    }

    private static string getRandomName()
    {
        var names = LeaderboardManager.Instance.names;
        var name = names[UnityEngine.Random.Range(0, names.Length)];
        while (LeaderboardManager.Instance.usedNames.Contains(name)) name = names[UnityEngine.Random.Range(0, names.Length)];
        LeaderboardManager.Instance.usedNames.Add(name);
        return name;
    }

    private static Countries getRandomCountry()
    {
        var countries = Enum.GetValues(typeof(Countries)) as Countries[];
        return countries[UnityEngine.Random.Range(0, countries.Length)]; ;
    }

    internal void Close()
    {
        foreach (Transform obj in transform) obj.gameObject.SetActive(false);
        GetComponent<Image>().enabled = false;
    }
}

