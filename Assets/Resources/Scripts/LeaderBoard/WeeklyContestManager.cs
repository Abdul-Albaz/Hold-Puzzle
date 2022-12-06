using UnityEngine;
using UnityEngine.UI;

public class WeeklyContestManager : MonoBehaviour
{
    public double countdownTime = 600;

    Text countdownText;
    double countdownInternal;
    bool countdownOver = false;
    
    void Start()
    {
        countdownText = GetComponent<Text>();
        countdownInternal = countdownTime; 
    }

    void FixedUpdate()
    {
        if (countdownInternal > 0)
        {
            countdownInternal -= Time.deltaTime;
            var now = System.DateTime.Now;
            System.DateTime endOfWeek = now.AddDays(((int)now.DayOfWeek) + 1);
            endOfWeek = new(endOfWeek.Year, endOfWeek.Month, endOfWeek.Day, 0, 0, 0);
            //endOfWeek = new(2022, 7, 30, 17, 40, 0);
            var deltaTime = Mathf.Max(0, (float)endOfWeek.Subtract(System.DateTime.Now).TotalSeconds);
            countdownText.text = FormatTime(deltaTime);
        }
        else if (!countdownOver) countdownOver = true;
        
    }

    string FormatTime(double time)
    {
        int intTime = (int)time;
        int days = intTime / 86400;
        int hours = intTime / 3600;
        int hoursFormatted = hours % 24;
        int minutes = intTime / 60;
        int minutesFormatted = minutes % 60;
        int seconds = intTime;
        int secondsFormatted = intTime % 60;

        if (days > 0) return string.Format("{0:0}d {1:0}h", days, hoursFormatted);
        else if (hours > 0) return string.Format("{0:0}h:{1:0}m", hours, minutesFormatted);
        else if (minutes > 0) return string.Format("{0:0}m:{1:0}s", minutes, secondsFormatted);
        else return string.Format("{0:0}s", seconds);
    }
}