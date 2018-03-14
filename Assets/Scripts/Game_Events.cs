using UnityEngine;

public class Game_Events : MonoBehaviour
{

    private static Game_Events _instance;

    public static Game_Events Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Game_Events>();

                //None in this scene - add it
                if (_instance == null)
                {
                    var go = new GameObject();
                    go.name = "Event Manager";
                    _instance = go.AddComponent<Game_Events>();
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

    // Delegates
    public delegate void OnLevelCompleteDelegate(int _levelID);
    public delegate void OnPlayerTriggerButtonEnterDelegate(Vector3 _position);
    public delegate void OnPlayerTriggerButtonExitDelegate();
    public delegate void TriggerButtonAnimationFinishedDelegate();
    public delegate void ToggleDraggingDelegate(bool _state);

    public void Event_LevelComplete(int _levelID)
    {
        OnLevelComplete(_levelID);
    }

    public void Event_PlayerTriggerEnter(Vector3 _position)
    {
        OnPlayerTriggerButtonEnter(_position);
    }

    public void Event_PlayerTriggerExit()
    {
        OnPlayerTriggerButtonExit();
    }
    
    public void Event_TriggerButtonAnimationFinished()
    {
        TriggerButtonAnimationFinished();
    }
    
    public void Event_ToggleDragging(bool _state)
    {
        ToggleDragging(_state);
        // Debug.LogFormat("Dragging: {0}", _state);
    }

}