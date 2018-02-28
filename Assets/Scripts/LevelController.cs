using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Controls the level (New implementation)
/// 
/// Author: Juan Rodriguez
/// Date: 10/09/2017
/// Version: 1.0
/// </summary>
public class LevelController : MonoBehaviour
{
    [HideInInspector][SerializeField] public PlayerController2D m_Player;

    [SerializeField] public bool m_CanRotate = true;
    [SerializeField] public bool m_RegisterInput = true;
    [SerializeField] private Ease m_RotationEaseType = Ease.OutQuad;
    [SerializeField] private float m_RotationLength = 0.4f;

    [Space][Header ("Rotation modes")]
    public bool m_InvertRotation = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake ()
    {
        Game_Events.Instance.OnPlayerTriggerButtonEnter += OnPlayerTriggerButtonEnter;
        Game_Events.Instance.OnPlayerTriggerButtonExit += OnPlayerTriggerButtonExit;
        Game_Events.Instance.TriggerButtonAnimationFinished += TriggerButtonAnimationFinished;

        m_Camera = Camera.main;

        if (null == m_Camera)
        {
            m_Camera = FindObjectOfType<Camera> ();
        }
    }

    void Update ()
    {
        //         // Don't do anything if the game's curently paused
        //         if (GameManager.GetState() != GameState.Play || !m_Player || !m_RegisterInput)
        //         {
        //             return;
        //         }

        //         if (
        //             // Not currently rotating
        //             m_CanRotate
        //             // Player can move
        //             &&
        //             (m_Player.CanMove)
        //             // Not currently colliding with anything
        //             &&
        //             (m_Player.m_CollisionInfo.above || m_Player.m_CollisionInfo.right || m_Player.m_CollisionInfo.below || m_Player.m_CollisionInfo.left)
        //         )
        //         {
        // #if UNITY_STANDALONE || UNITY_EDITOR
        //             if (Input.GetKey(KeyCode.E) || MobileInputController.Instance.SwipeRight)
        //             {
        //                 StartCoroutine(Rotate((m_InvertRotation) ? false : true));
        //             }
        //             else if (Input.GetKey(KeyCode.Q) || MobileInputController.Instance.SwipeLeft)
        //             {
        //                 StartCoroutine(Rotate((m_InvertRotation) ? true : false));
        //             }
        // #elif UNITY_IOS || UNITY_ANDROID
        //             if (MobileInputController.Instance.SwipeRight)
        //             {
        //                 StartCoroutine(Rotate((m_InvertRotation) ? false : true));
        //             }
        //             else if (MobileInputController.Instance.SwipeLeft)
        //             {
        //                 StartCoroutine(Rotate((m_InvertRotation) ? true : false));
        //             }
        // #endif
        //         }

    }

    /// <summary>
    /// Rotates the level
    /// </summary>
    public IEnumerator Rotate (bool _shouldRotateRight, bool _override = false, float _overrideAngle = 0)
    {
        GameManager.Instance.IncrementMoveCount ();

        m_Player.CanMove = false;

        m_Player.ProcessCollisions ();

        m_CanRotate = false;

        Vector3 eulerRotation = transform.eulerAngles;

        if (_override)
        {
            eulerRotation.z = _overrideAngle;
            Debug.LogFormat ("Overriding, new angle is: {0}", _overrideAngle);
        }
        else
        {
            if (_shouldRotateRight)
            {
                eulerRotation.z -= 90;
                // AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeRight);
            }
            else
            {
                eulerRotation.z += 90;
                // AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeLeft);
            }
        }

        // Rotate the transform
        transform.DORotate (eulerRotation, m_RotationLength).SetEase (m_RotationEaseType);

        yield return new WaitForSeconds (m_RotationLength);

        m_CanRotate = true;

        yield return new WaitForSeconds (0.05f);

        m_Player.CanMove = true;

        // yield return new WaitForSeconds (0.15f);

        m_BaseAngle = eulerRotation.z;
        candrag = true;
        drag = false;
    }

    public void RoundToNearestNinenty (float _currentAngle)
    {
        // if (MobileInputController.Instance.SwipeRight || MobileInputController.Instance.SwipeLeft)
        // {
        //     if (MobileInputController.Instance.SwipeRight)
        //     {
        //         StartCoroutine(Rotate(true));
        //     }
        //     else if (MobileInputController.Instance.SwipeLeft)
        //     {
        //         StartCoroutine(Rotate(false));
        //     }

        //     Debug.LogFormat("Rounding to nearest ninety but swiping right/left");

        //     return;
        // }

        Debug.LogFormat ("Rounding to nearest ninety - Current angle: {0}", _currentAngle);

        // RIGHT SIDE ANGLES
        if (_currentAngle > -45 && _currentAngle <= 45)
        {
            _currentAngle = 0;
        }
        if (_currentAngle > 45 && _currentAngle <= 135)
        {
            _currentAngle = 90;
        }
        else if (_currentAngle > 135 && _currentAngle <= 225)
        {
            _currentAngle = 180;
        }
        else if (_currentAngle > 225 && _currentAngle <= 315)
        {
            _currentAngle = 270;
        }
        else if (_currentAngle < -360 || _currentAngle > 360)
        {
            _currentAngle = 0;
        }
        else if (_currentAngle >= -360 && _currentAngle < -270)
        {
            _currentAngle = -270;
        }
        else if (_currentAngle >= -270 && _currentAngle < -180)
        {
            _currentAngle = -180;
        }
        else if (_currentAngle >= -180 && _currentAngle < -135)
        {
            _currentAngle = -180;
        }
        else if (_currentAngle >= -135 && _currentAngle < -45)
        {
            _currentAngle = -90;
        }

        // LEFT SIDE ANGLES
        StartCoroutine (Rotate (false, true, _currentAngle));
    }

    private float m_BaseAngle = 0.0f;
    [SerializeField] bool drag;
    [SerializeField] bool candrag;
    [SerializeField] bool startdrag;

    private Camera m_Camera;

    void OnMouseDown ()
    {
        if (!candrag)
        {
            return;
        }

        if (m_Player.IsMoving)
        {
            return;
        }

        Debug.LogFormat (">>>>>>>>>>>>>>>>>>>>> MOUSE DOWN");
        Vector3 pos = m_Camera.WorldToScreenPoint (transform.position);
        pos = Input.mousePosition - pos;
        m_BaseAngle = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg;
        Debug.LogFormat ("Initial Base Angle: {0} - Position: {1}", m_BaseAngle, pos);
        m_BaseAngle -= Mathf.Atan2 (transform.right.y, transform.right.x) * Mathf.Rad2Deg;
        Debug.LogFormat ("Initial Base Angle: {0}", m_BaseAngle);

        Game_Events.Instance.Event_ToggleDragging (true);

        drag = true;
        candrag = false;
        startdrag = true;
    }

    void OnMouseUp ()
    {
        if (!startdrag)
        {
            return;
        }

        if (m_Player.IsMoving)
        {
            return;
        }

        Debug.LogFormat (">>>>>>>>>>>>>>>>>>>>> MOUSE UP");
        Game_Events.Instance.Event_ToggleDragging (false);

        Vector3 pos = m_Camera.WorldToScreenPoint (transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;

        RoundToNearestNinenty (ang);
        startdrag = false;
    }

    void OnMouseDrag ()
    {
        if (!startdrag)
        {
            return;
        }

        if (!drag && candrag)
        {
            return;
        }

        if (m_Player.IsMoving)
        {
            return;
        }
        
        Vector3 pos = m_Camera.WorldToScreenPoint (transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
        transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);
    }

    public void OnPlayerTriggerButtonEnter (Vector3 _position)
    {
        m_CanRotate = false;
        m_RegisterInput = false;
    }

    public void OnPlayerTriggerButtonExit ()
    {

    }

    public void TriggerButtonAnimationFinished ()
    {
        m_CanRotate = true;
        m_RegisterInput = true;
    }

}