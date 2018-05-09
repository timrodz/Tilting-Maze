/// <summary>
/// Credits: mstevenson
/// URL: https://gist.github.com/mstevenson/4325117
/// </summary>

using System.Collections;
using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour
where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType (typeof (T)) as T[];

                // Check for any other instances
                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }

                if (objs.Length > 1)
                {
                    Print.LogError ("There is more than one " + typeof (T).Name + " in the scene.");
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject ();
                    obj.name = typeof (T).Name;
                    // obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T> ();
                }

            }

            return _instance;
        }
    }

    public virtual void Awake ()
    {
        if (_instance == null)
        {
            _instance = this as T;
            // DontDestroyOnLoad(this);
            DontDestroyOnLoad (this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
    }

}

public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
where T : Component
{
    public static T Instance { get; private set; }

    public virtual void Awake ()
    {
        if (Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad (this);
        }
        else
        {
            Destroy (gameObject);
        }
    }
}