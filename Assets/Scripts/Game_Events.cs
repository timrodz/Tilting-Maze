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
    public event OnPlayerTriggerButtonEnterDelegate OnPlayerTriggerButtonEnter;
    public event TriggerButtonAnimationFinishedDelegate TriggerButtonAnimationFinished;
    public event OnPlayerTriggerButtonExitDelegate OnPlayerTriggerButtonExit;

    // Delegates
    public delegate void OnPlayerTriggerButtonEnterDelegate(Vector3 _position);
    public delegate void TriggerButtonAnimationFinishedDelegate();
    public delegate void OnPlayerTriggerButtonExitDelegate();

    public void Event_PlayerTriggerEnter(Vector3 _position)
    {
        OnPlayerTriggerButtonEnter(_position);
    }

    public void Event_TriggerButtonAnimationFinished()
    {
        TriggerButtonAnimationFinished();
    }

    public void Event_PlayerTriggerExit()
    {
        OnPlayerTriggerButtonExit();
    }

}