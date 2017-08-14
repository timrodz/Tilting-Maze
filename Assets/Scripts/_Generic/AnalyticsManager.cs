using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public enum eCustomEvent
{
    SwipeLeft,
    SwipeRight
}

public class AnalyticsManager
{
    private static AnalyticsManager _instance;
    public static AnalyticsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Instance = new AnalyticsManager();
            }

            return _instance;
        }

        protected set
        {
            _instance = value;
        }
    }

    public void RegisterCustomEventLevelComplete(int moveCount, float levelTime)
    {

        Debug.Log("==== Analytics - Registering level complete - Level ID: " + GameManager.Instance.levelID.ToString() + " - Moves: " + moveCount.ToString() + " - time: " + levelTime.ToString());

        Analytics.CustomEvent("Level complete", new Dictionary<string, object>
        { { "Level", GameManager.Instance.levelID },
            { "Moves", moveCount },
            { "Time", levelTime }
        });

    }

    public void RegisterCustomEventSwipe(eCustomEvent customEvent)
    {

        Debug.Log("==== Analytics - Registering swipe: " + customEvent.ToString());

        switch (customEvent)
        {
            case eCustomEvent.SwipeLeft:
            case eCustomEvent.SwipeRight:
                {
                    Analytics.CustomEvent("Input", new Dictionary<string, object>
                    { { "Level", GameManager.Instance.levelID },
                        { "Swipe", customEvent }
                    });
                }
                break;
        }

    }

    public void RegisterCustomEventTriggerEnter(string gameObject)
    {

        Debug.Log("==== Analytics - Registering Trigger enter: " + gameObject);

        Analytics.CustomEvent("Trigger Enter", new Dictionary<string, object>
        { { "Level", GameManager.Instance.levelID },
            { "Name", gameObject }
        });

    }

    public void RegisterCustomEventTriggerExit(string gameObject)
    {

        Debug.Log("==== Analytics - Registering Trigger exit: " + gameObject);

        Analytics.CustomEvent("Trigger Exit", new Dictionary<string, object>
        { { "Name", gameObject },
            { "Level", GameManager.Instance.levelID }
        });

    }

}