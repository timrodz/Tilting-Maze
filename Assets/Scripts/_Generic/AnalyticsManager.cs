using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
using UnityEngine.Analytics;
#endif

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
                Instance = new AnalyticsManager ();
            }

            return _instance;
        }

        protected set
        {
            _instance = value;
        }
    }

    public void RegisterCustomEventLevelComplete (int moveCount, float levelTime)
    {
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
        Print.Log ("==== Analytics - Registering level complete - Level ID: " + GameManager.Instance.LevelID.ToString () + " - Moves: " + moveCount.ToString () + " - time: " + levelTime.ToString ());

        Analytics.CustomEvent ("Level complete", new Dictionary<string, object>
        { { "Level", GameManager.Instance.LevelID },
            { "Moves", moveCount },
            { "Time", levelTime }
        });
#endif
    }

    public void RegisterCustomEventSwipe (CustomEvent customEvent)
    {
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
        Print.Log ("==== Analytics - Registering swipe: " + customEvent.ToString ());

        switch (customEvent)
        {
            case CustomEvent.SwipeLeft:
            case CustomEvent.SwipeRight:
                {
                    Analytics.CustomEvent ("Input", new Dictionary<string, object>
                    { { "Level", GameManager.Instance.LevelID },
                        { "Swipe", customEvent }
                    });
                }
                break;
        }
#endif
    }

    public void RegisterCustomEventTriggerEnter (string gameObject)
    {
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
        Print.Log ("==== Analytics - Registering Trigger enter: " + gameObject);

        Analytics.CustomEvent ("Trigger Enter", new Dictionary<string, object>
        { { "Level", GameManager.Instance.LevelID },
            { "Name", gameObject }
        });
#endif
    }

    public void RegisterCustomEventTriggerExit (string gameObject)
    {
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
        Print.Log ("==== Analytics - Registering Trigger exit: " + gameObject);

        Analytics.CustomEvent ("Trigger Exit", new Dictionary<string, object>
        { { "Name", gameObject },
            { "Level", GameManager.Instance.LevelID }
        });
#endif
    }

}