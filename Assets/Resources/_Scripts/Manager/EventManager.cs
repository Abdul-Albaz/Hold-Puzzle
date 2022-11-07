
//using Firebase.Analytics;
using System;

public class EventManager : Singleton<EventManager>
{
    //GameManager manager => GameManager.Instance;
    //Parameter moveCountparameter => new("moveCount", GridManager.Instance);
    //Parameter modeparameter => new("mode", manager.mode.ToString());

    //public void logEvent(EventActions eventId)
    //{
    //    FirebaseAnalytics.LogEvent(eventId.ToString(), modeparameter, moveCountparameter);
    //    FB.LogAppEvent(eventId.ToString());
    //}

    //public void logEvent(EventActions eventId, int item)
    //{
    //    FirebaseAnalytics.LogEvent(eventId.ToString(), new(FirebaseAnalytics.ParameterLevel, item), modeparameter, moveCountparameter);
    //    FB.LogAppEvent(eventId.ToString());
    //}

    //public void logEvent(EventActions eventId, Combos item)
    //{
    //    FirebaseAnalytics.LogEvent(eventId.ToString(), new(FirebaseAnalytics.ParameterItemId, item.ToString()), modeparameter, moveCountparameter);
    //    FB.LogAppEvent(eventId.ToString());
    //}

    //public void logEvent(EventActions eventId, PowerUps item)
    //{
    //    FirebaseAnalytics.LogEvent(eventId.ToString(), new(FirebaseAnalytics.ParameterItemId, item.ToString()), modeparameter, moveCountparameter);
    //    FB.LogAppEvent(eventId.ToString());
    //}

    public static EventActions Parse(string name)
    {
        foreach (EventActions item in Enum.GetValues(typeof(EventActions)))
        {
            if (Enum.GetName(typeof(EventActions), item) == name) return item;
        }
        return EventActions.levelCompleted; 
    }
}

public enum EventActions {

    levelStarted,
    levelCompleted,
    levelFailed,
    tutorialCompleted,
    comboCompleted,
    powerUpTried,
    powerUpUsed,
    levelCompleted10,
    levelCompleted20,
    levelCompleted30,
    levelCompleted40,
    levelCompleted50,
    levelCompleted60,
    levelCompleted70,
    levelCompleted80,
    levelCompleted90,
    levelCompleted100


    //levelCompleted1, levelCompleted2, levelCompleted3, levelCompleted4, levelCompleted5, levelCompleted6, levelCompleted7, levelCompleted8, levelCompleted9,          
    //levelCompleted11, levelCompleted12, levelCompleted13, levelCompleted14, levelCompleted15, levelCompleted16, levelCompleted17, levelCompleted18, levelCompleted19, 
    //levelCompleted21, levelCompleted22, levelCompleted23, levelCompleted24, levelCompleted25, levelCompleted26, levelCompleted27, levelCompleted28, levelCompleted29, 
    //levelCompleted31, levelCompleted32, levelCompleted33, levelCompleted34, levelCompleted35, levelCompleted36, levelCompleted37, levelCompleted38, levelCompleted39, 
    //levelCompleted41, levelCompleted42, levelCompleted43, levelCompleted44, levelCompleted45, levelCompleted46, levelCompleted47, levelCompleted48, levelCompleted49, 
    //levelCompleted51, levelCompleted52, levelCompleted53, levelCompleted54, levelCompleted55, levelCompleted56, levelCompleted57, levelCompleted58, levelCompleted59, 
    //levelCompleted61, levelCompleted62, levelCompleted63, levelCompleted64, levelCompleted65, levelCompleted66, levelCompleted67, levelCompleted68, levelCompleted69, 
    //levelCompleted71, levelCompleted72, levelCompleted73, levelCompleted74, levelCompleted75, levelCompleted76, levelCompleted77, levelCompleted78, levelCompleted79, 
    //levelCompleted81, levelCompleted82, levelCompleted83, levelCompleted84, levelCompleted85, levelCompleted86, levelCompleted87, levelCompleted88, levelCompleted89, 
    //levelCompleted91, levelCompleted92, levelCompleted93, levelCompleted94, levelCompleted95, levelCompleted96, levelCompleted97, levelCompleted98, levelCompleted99, 

}