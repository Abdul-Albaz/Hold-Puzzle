using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopChartManager : Singleton<TopChartManager>
{
    internal Countries country;
    public Image flagImage1;
    public Image flagImage2;
    public Image flagImage3;

    List<LeaderboardCell> players => LeaderboardManager.Instance.players;

    public void set()
    {
        country = getRandomCountry(1);
        flagImage1.GetComponent<Image>().sprite = LeaderboardManager.Instance.rank == 1 ? ProfileManager.Instance.avatarSprite : Resources.Load<Sprite>($"flags/{country}");

        country = getRandomCountry(2);
        flagImage2.GetComponent<Image>().sprite = LeaderboardManager.Instance.rank == 2 ? ProfileManager.Instance.avatarSprite : Resources.Load<Sprite>($"flags/{country}");

        country = getRandomCountry(3);
        flagImage3.GetComponent<Image>().sprite = LeaderboardManager.Instance.rank == 3 ? ProfileManager.Instance.avatarSprite : Resources.Load<Sprite>($"flags/{country}");
    }

    private Countries getRandomCountry(int rank)
    {   
        foreach(var p in players) if (p.rank == rank) return p.country;
        var countries = Enum.GetValues(typeof(Countries)) as Countries[];
        Countries temp = countries[UnityEngine.Random.Range(0, countries.Length)];
        return temp;
    }
}
