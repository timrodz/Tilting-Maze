using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
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
    private Camera m_Camera;
    [HideInInspector][SerializeField] public PlayerController2D m_Player;

    [Header("Rotation variables")]
    [SerializeField] private Ease m_RotationEaseType = Ease.OutQuad;
    [SerializeField] private float m_RotationLength = 0.4f;

    [Header("Dragging")]
    [SerializeField] public bool m_CanRegisterInput = true;
    [SerializeField] public bool m_CanRotate = true;
    [SerializeField] private float m_BaseAngle = 0.0f;
    [SerializeField] bool m_IsDragging = false;
    [SerializeField] bool m_CanDrag = true;
    [SerializeField] bool m_StartedDragging = false;
    [SerializeField] private float m_DragTime = 0.0f;

    [Header("Storytelling")]
    [SerializeField] private SubtitleTextOptions m_SubtitlesToShowAtStart;

    protected virtual void OnEnable()
    {
        // Initialize values on start because this object can be spawned dynamically
        Game_Events.Instance.OnPlayerTriggerButtonEnter += OnPlayerTriggerButtonEnter;
        Game_Events.Instance.OnPlayerTriggerButtonExit += OnPlayerTriggerButtonExit;
        Game_Events.Instance.TriggerButtonAnimationFinished += TriggerButtonAnimationFinished;

        // Lean Touch
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerSet += OnFingerSet;
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerTap += OnFingerTap;
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
    }

    protected virtual void OnDisable()
    {
        // Unhook all events
        if (null != Game_Events.Instance)
        {
            Game_Events.Instance.OnPlayerTriggerButtonEnter -= OnPlayerTriggerButtonEnter;
            Game_Events.Instance.OnPlayerTriggerButtonExit -= OnPlayerTriggerButtonExit;
            Game_Events.Instance.TriggerButtonAnimationFinished -= TriggerButtonAnimationFinished;
        }

        // Lean Touch
        if (null != LeanTouch.Instance)
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerSet -= OnFingerSet;
            LeanTouch.OnFingerUp -= OnFingerUp;
            LeanTouch.OnFingerTap -= OnFingerTap;
            LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        m_Camera = Camera.main;

        if (null == m_Camera)
        {
            m_Camera = FindObjectOfType<Camera>();
        }

        Game_Events.Instance.Event_DisplaySubtitles(m_SubtitlesToShowAtStart);
    }

    void Update()
    {
        if (GameManager.GetState() != GameState.Play || !m_Player || !m_CanRegisterInput)
        {
            return;
        }

        if (!m_StartedDragging)
        {
            return;
        }

        if (!m_IsDragging && m_CanDrag)
        {
            return;
        }

        if (m_Player.IsMoving)
        {
            return;
        }

        if (LeanTouch.Fingers.Count > 0)
        {
            m_DragTime += Time.deltaTime;

            var f = LeanTouch.Fingers[0];

            Vector2 pos = m_Camera.WorldToScreenPoint(transform.position);
            pos = f.ScreenPosition - pos;
            float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
            transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
        }

    }

    /// <summary>
    /// Rotates the level
    /// </summary>
    public IEnumerator Rotate(bool _shouldRotateRight, bool _override = false, float _overrideAngle = 0)
    {
        GameManager.Instance.IncrementMoveCount();

        m_Player.CanMove = false;

        m_Player.ProcessCollisions();

        m_CanRotate = false;

        Vector3 eulerRotation = transform.eulerAngles;

        if (_override)
        {
            eulerRotation.z = _overrideAngle;
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
        transform.DORotate(eulerRotation, m_RotationLength).SetEase(m_RotationEaseType);

        yield return new WaitForSeconds(m_RotationLength);

        m_CanRotate = true;

        yield return new WaitForSeconds(0.05f);

        m_Player.CanMove = true;

        m_BaseAngle = eulerRotation.z;
        m_CanDrag = true;
        m_IsDragging = false;
    }

    public float GetNearestNinetyDegreeAngle(float _currentAngle, float _dragTime, LeanFinger _fingerData)
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

        // If the delta is higher than -50, the user has definitely swiped.

        // With the level's current rotation values, calculate the degree closest to 90 of it.
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

        return _currentAngle;
    }

    public void OnPlayerTriggerButtonEnter(Vector3 _position)
    {
        m_CanRotate = false;
        m_CanRegisterInput = false;
        m_CanDrag = false;
    }

    public void OnPlayerTriggerButtonExit()
    {

    }

    public void TriggerButtonAnimationFinished()
    {
        m_CanRotate = true;
        m_CanRegisterInput = true;
        m_CanDrag = true;
    }

    public void OnFingerDown(LeanFinger _finger)
    {
        if (GameManager.GetState() != GameState.Play || !m_Player || !m_CanRegisterInput)
        {
            return;
        }

        if (!m_CanDrag)
        {
            return;
        }

        if (m_Player.IsMoving)
        {
            return;
        }

        // Dragging states
        Game_Events.Instance.Event_ToggleDragging(true);
        m_IsDragging = true;
        m_CanDrag = false;
        m_StartedDragging = true;

        Vector2 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = _finger.ScreenPosition - pos;
        m_BaseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        m_BaseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
    }

    public void OnFingerSet(LeanFinger _finger)
    {
        // if (GameManager.GetState () != GameState.Play || !m_Player || !m_CanRegisterInput)
        // {
        //     return;
        // }

        // if (!m_StartedDragging)
        // {
        //     return;
        // }

        // if (!m_IsDragging && m_CanDrag)
        // {
        //     return;
        // }

        // if (m_Player.IsMoving)
        // {
        //     return;
        // }

        // Vector2 pos = m_Camera.WorldToScreenPoint (transform.position);
        // pos = finger.ScreenPosition - pos;
        // float ang = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
        // transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);
    }

    public void OnFingerUp(LeanFinger _finger)
    {
        if (GameManager.GetState() != GameState.Play || !m_Player || !m_CanRegisterInput)
        {
            return;
        }

        if (!m_StartedDragging)
        {
            return;
        }

        if (m_Player.IsMoving)
        {
            return;
        }

        Game_Events.Instance.Event_ToggleDragging(false);

        Vector2 pos = m_Camera.WorldToScreenPoint(transform.position);
        pos = _finger.ScreenPosition - pos;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;

        float roundedAngle = GetNearestNinetyDegreeAngle(ang, m_DragTime, _finger);

        Debug.LogFormat(">> [Base: ({0:0.00}) | Current: ({1:0.00}) | New: ({2:0.00})] - [Drag: ({3:0.00}), Swipe delta: ({4})]", m_BaseAngle, ang, roundedAngle, m_DragTime, _finger.SwipeScreenDelta);

        StartCoroutine(Rotate(false, true, roundedAngle));

        m_StartedDragging = false;
        m_DragTime = 0.0f;
    }

    public void OnFingerTap(LeanFinger _finger)
    {
        // Debug.Log ("Finger " + finger.Index + " tapped the screen");
    }

    public void OnFingerSwipe(LeanFinger _finger)
    {
        // Debug.Log ("Finger " + _finger.Index + " swiped the screen");
    }

}