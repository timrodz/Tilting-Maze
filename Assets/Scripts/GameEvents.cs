using UnityEngine;

public class GameEvents : MonoBehaviour
{

    private static GameEvents _instance;

    public static GameEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameEvents> ();

                //None in this scene - add it
                if (_instance == null)
                {
                    var go = new GameObject ();
                    go.name = "Event Manager";
                    _instance = go.AddComponent<GameEvents> ();
                }
            }
            return _instance;
        }
    }

    // All events used by this model
    public event OnLevelCompleteDelegate OnLevelComplete;
    public event OnPlayerTriggerButtonEnterDelegate OnPlayerTriggerButtonEnter;
    public event OnPlayerTriggerButtonExitDelegate OnPlayerTriggerButtonExit;
    public event TriggerButtonAnimationFinishedDelegate TriggerButtonAnimationFinished;
    public event ToggleDraggingDelegate ToggleDragging;
    public event DisplaySubtitlesDelegate DisplaySubtitles;

    // Delegates
    public delegate void OnLevelCompleteDelegate (int _levelID);
    public delegate void OnPlayerTriggerButtonEnterDelegate (Vector3 _position);
    public delegate void OnPlayerTriggerButtonExitDelegate ();
    public delegate void TriggerButtonAnimationFinishedDelegate ();
    public delegate void ToggleDraggingDelegate (bool _state);
    public delegate void DisplaySubtitlesDelegate (SubtitleTextOptions _options);

    public void Event_LevelComplete (int _levelID)
    {
        OnLevelComplete (_levelID);
    }

    public void Event_PlayerTriggerEnter (Vector3 _position)
    {
        OnPlayerTriggerButtonEnter (_position);
    }

    public void Event_PlayerTriggerExit ()
    {
        OnPlayerTriggerButtonExit ();
    }

    public void Event_TriggerButtonAnimationFinished ()
    {
        TriggerButtonAnimationFinished ();
    }

    public void Event_ToggleDragging (bool _state)
    {
        ToggleDragging (_state);
    }

    public void Event_DisplaySubtitles (SubtitleTextOptions _options)
    {
        DisplaySubtitles (_options);
    }

}