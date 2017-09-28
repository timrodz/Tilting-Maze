using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public enum CustomEvent
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
#if !UNITY_EDITOR
        Debug.Log("==== Analytics - Registering level complete - Level ID: " + GameManager.Instance.GetLevelID().ToString() + " - Moves: " + moveCount.ToString() + " - time: " + levelTime.ToString());

        Analytics.CustomEvent("Level complete", new Dictionary<string, object>
        { { "Level", GameManager.Instance.GetLevelID() },
            { "Moves", moveCount },
            { "Time", levelTime }
        });
#endif
    }

    public void RegisterCustomEventSwipe(CustomEvent customEvent)
    {
#if !UNITY_EDITOR
        Debug.Log("==== Analytics - Registering swipe: " + customEvent.ToString());

        switch (customEvent)
        {
            case CustomEvent.SwipeLeft:
            case CustomEvent.SwipeRight:
                {
                    Analytics.CustomEvent("Input", new Dictionary<string, object>
                    { { "Level", GameManager.Instance.GetLevelID() },
                        { "Swipe", customEvent }
                    });
                }
                break;
        }
#endif
    }

    public void RegisterCustomEventTriggerEnter(string gameObject)
    {
#if !UNITY_EDITOR
        Debug.Log("==== Analytics - Registering Trigger enter: " + gameObject);

        Analytics.CustomEvent("Trigger Enter", new Dictionary<string, object>
        { { "Level", GameManager.Instance.GetLevelID() },
            { "Name", gameObject }
        });
#endif
    }

    public void RegisterCustomEventTriggerExit(string gameObject)
    {
#if !UNITY_EDITOR
        Debug.Log("==== Analytics - Registering Trigger exit: " + gameObject);

        Analytics.CustomEvent("Trigger Exit", new Dictionary<string, object>
        { { "Name", gameObject },
            { "Level", GameManager.Instance.GetLevelID() }
        });
#endif
    }

}