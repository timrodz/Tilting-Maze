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

    // -------------------------------------------------------------------------------------------
    // -- All events used by this model
    // Level
    public event LevelCompleteDelegate LevelComplete;
    public event NewLevelLoadedDelegate NewLevelLoaded;

    // Player
    public event PlayerTriggerButtonEnterDelegate PlayerTriggerButtonEnter;
    public event PlayerTriggerButtonExitDelegate PlayerTriggerButtonExit;
    public event TriggerButtonAnimationFinishedDelegate TriggerButtonAnimationFinished;
    public event PlayerCollisionDelegate PlayerCollision;

    // Input
    public event ToggleDraggingDelegate ToggleDragging;
    public event RotateLevelDelegate RotateLevel;

    // UI
    public event DisplaySubtitlesDelegate DisplaySubtitles;

    // -------------------------------------------------------------------------------------------
    // --Delegates
    // Level
    public delegate void LevelCompleteDelegate (int _levelID);
    public delegate void NewLevelLoadedDelegate ();

    // Player
    public delegate void PlayerTriggerButtonEnterDelegate (Vector3 _position);
    public delegate void PlayerTriggerButtonExitDelegate ();
    public delegate void TriggerButtonAnimationFinishedDelegate ();
    public delegate void PlayerCollisionDelegate ();

    // Input
    public delegate void ToggleDraggingDelegate (bool _state);
    public delegate void RotateLevelDelegate(bool _rotateRight);

    // UI
    public delegate void DisplaySubtitlesDelegate (SubtitleTextOptions _options);

    // -------------------------------------------------------------------------------------------
    // Level
    public void Event_LevelComplete (int _levelID)
    {
        LevelComplete (_levelID);
    }

    public void Event_LevelLoaded ()
    {
        NewLevelLoaded ();
    }

    // Player
    public void Event_PlayerTriggerButtonEnter (Vector3 _position)
    {
        PlayerTriggerButtonEnter (_position);
    }

    public void Event_PlayerTriggerExit ()
    {
        PlayerTriggerButtonExit ();
    }

    public void Event_TriggerButtonAnimationFinished ()
    {
        TriggerButtonAnimationFinished ();
    }

    public void Event_PlayerCollision ()
    {
        PlayerCollision ();
    }

    // Input
    public void Event_ToggleDragging (bool _state)
    {
        ToggleDragging (_state);
    }

    public void Event_RotateLevel (bool _rotateRight)
    {
        RotateLevel (_rotateRight);
    }

    // UI
    public void Event_DisplaySubtitles (SubtitleTextOptions _options)
    {
        DisplaySubtitles (_options);
    }

}