/// <summary>
/// Controls the level (New implementation)
/// 
/// Date: 10/09/2017
/// </summary>

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private Camera m_Camera;
    [HideInInspector][SerializeField] public PlayerController2D m_Player;
    [HideInInspector][SerializeField] private MovingObjectController2D[] m_Obstacles;

    [Header ("Rotation variables")]
    [SerializeField] private Ease m_RotationEaseType = Ease.OutQuad;
    [SerializeField] private float m_RotationLength = 0.4f;

    [Header ("Dragging")]
    [SerializeField] public bool m_CanRegisterInput = true;
    [SerializeField] public bool m_CanRotate = true;
    [SerializeField] private float m_BaseAngle = 0.0f;
    [SerializeField] bool m_IsDragging = false;
    [SerializeField] bool m_CanDrag = true;
    [SerializeField] bool m_StartedDragging = false;
    [SerializeField] private float m_DragTime = 0.0f;

    [Header ("Storytelling")]
    [SerializeField] private SubtitleTextOptions m_SubtitlesToShowAtStart;

    protected virtual void OnEnable ()
    {
        // Initialize values on start because this object can be spawned dynamically
        GameEvents.Instance.PlayerTriggerButtonEnter += OnPlayerTriggerButtonEnter;
        GameEvents.Instance.PlayerTriggerButtonExit += OnPlayerTriggerButtonExit;
        GameEvents.Instance.TriggerButtonAnimationFinished += TriggerButtonAnimationFinished;
        GameEvents.Instance.RotateLevel += AccessibilityRotateRequest;

        // Lean Touch
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerSet += OnFingerSet;
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerTap += OnFingerTap;
        LeanTouch.OnFingerSwipe += OnFingerSwipe;
    }

    protected virtual void OnDisable ()
    {
        // // Unhook all events
        // if (null != Game_Events.Instance)
        // {
        //     Game_Events.Instance.OnPlayerTriggerButtonEnter -= OnPlayerTriggerButtonEnter;
        //     Game_Events.Instance.OnPlayerTriggerButtonExit -= OnPlayerTriggerButtonExit;
        //     Game_Events.Instance.TriggerButtonAnimationFinished -= TriggerButtonAnimationFinished;
        // }

        // // Lean Touch
        // if (null != LeanTouch.Instance)
        // {
        //     LeanTouch.OnFingerDown -= OnFingerDown;
        //     LeanTouch.OnFingerSet -= OnFingerSet;
        //     LeanTouch.OnFingerUp -= OnFingerUp;
        //     LeanTouch.OnFingerTap -= OnFingerTap;
        //     LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        // }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start ()
    {
        m_Camera = Camera.main;

        if (null == m_Camera)
        {
            m_Camera = FindObjectOfType<Camera> ();
        }

        GameEvents.Instance.Event_DisplaySubtitles (m_SubtitlesToShowAtStart);

        m_Obstacles = FindObjectsOfType<MovingObjectController2D> ();
    }

    void Update ()
    {
        if (Utils.ACCESSIBILITY_ON)
        {
            return;
        }

        if (GameManager.Instance.State != GameState.Play || !m_Player || !m_CanRegisterInput)
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

            Vector2 pos = m_Camera.WorldToScreenPoint (transform.position);
            pos = f.ScreenPosition - pos;
            float ang = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;
            transform.rotation = Quaternion.AngleAxis (ang, Vector3.forward);
        }
    }

    /// <summary>
    /// Coroutine that rotates the Transform
    /// </summary>
    /// <param name="_forwardAngle">The forward rotation angle</param>
    /// <returns>IEnumerator</returns>
    public IEnumerator Rotate (float _forwardAngle = 0)
    {
        GameManager.Instance.IncrementMoveCount ();

        m_Player.CanMove = false;

        m_Player.ProcessCollisions ();

        foreach (MovingObjectController2D obstacle in m_Obstacles)
        {
            obstacle.CanMove = false;
            obstacle.ProcessCollisions ();
        }

        m_CanRotate = false;

        Vector3 eulerRotation = transform.eulerAngles;

        eulerRotation.z = _forwardAngle;

        // Rotate the transform
        transform.DORotate (eulerRotation, m_RotationLength).SetEase (m_RotationEaseType);

        yield return new WaitForSeconds (m_RotationLength);

        m_CanRotate = true;

        yield return new WaitForSeconds (0.05f);

        ResetStates ();
    }

    /// <summary>
    /// Gets the next angle based on this Transform's current eulerAngles
    /// </summary>
    /// <param name="_baseAngle">Base transform angles (Set up when the player starts dragging)</param>
    /// <param name="_currentAngle">Current Transform angles</param>
    /// <param name="_dragTime">Total dragging time</param>
    /// <param name="_fingerData">Gesture information</param>
    /// <returns>Float</returns>
    private float GetNextRotationAngle (float _baseAngle, float _currentAngle, float _dragTime, LeanFinger _fingerData)
    {
        bool isNegative = false;
        int angleMultiplier = 0;

        // With the level's current rotation values, calculate the degree closest to 90 of it.
        // The following are cartesian units with an offset of 45 degrees each.
        if ((_currentAngle > -45 || _currentAngle > 315) && _currentAngle <= 45)
        {
            angleMultiplier = 0;
        }
        if (_currentAngle > 45 && _currentAngle <= 135)
        {
            angleMultiplier = 1;
        }
        else if (_currentAngle > 135 && _currentAngle <= 225)
        {
            angleMultiplier = 2;
        }
        else if (_currentAngle > 225 && _currentAngle <= 315)
        {
            angleMultiplier = 3;
        }
        else if (_currentAngle < -360 || _currentAngle > 360)
        {
            isNegative = true;
            angleMultiplier = 0;
        }
        else if (_currentAngle >= -315 && _currentAngle < -225)
        {
            isNegative = true;
            angleMultiplier = -3;
        }
        else if (_currentAngle >= -225 && _currentAngle < -135)
        {
            isNegative = true;
            angleMultiplier = -2;
        }
        else if (_currentAngle >= -135 && _currentAngle < -45)
        {
            isNegative = true;
            angleMultiplier = -1;
        }

        // Very small drag time, rotate anyways
        if (_dragTime < 0.2f)
        {
            Print.LogFormat ("Current Multiplier: {0}, Angle: {1}", angleMultiplier, _currentAngle);

            int direction = (int) Mathf.Sign (_fingerData.SwipeScreenDelta.x);
            SmoothAngleMultiplier (ref angleMultiplier, _currentAngle, _dragTime, direction, isNegative);
        }
        else
        {
            Print.LogFormat ("Outcome: {0}", angleMultiplier);
        }

        _currentAngle = 90 * angleMultiplier;

        return _currentAngle;
    }

    /// <summary>
    /// Attempts to smooth the next angle depending on how fast the player swipes
    /// This method is called from GetNearestNinetyDegreeAngle
    /// 
    /// TODO: Test on iOS devices
    /// </summary>
    /// <param name="_multiplier">Angle multiplier - Passed by reference</param>
    /// <param name="_angle">Current angle of the trasnform</param>
    /// <param name="_dragTime">Total dragging time</param>
    /// <param name="_direction">Direction of the swipe</param>
    /// <param name="_isMultiplierNegative">Whether or not the multiplier was originally negative</param>
    /// <returns>Int</returns>
    private void SmoothAngleMultiplier (ref int _multiplier, float _angle, float _dragTime, int _direction, bool _isMultiplierNegative)
    {
        float calculatedAngle = 90 * (_multiplier == 0 ? (_direction > 0 ? 1 : -1) : _multiplier);
        float x = Mathf.Abs (calculatedAngle) - Mathf.Abs (_angle);

        string res = ">>>>";

        bool check = false;
        bool angleCheck = false;
        if (_direction > 0)
        {
            if (_multiplier > 0)
            {
                check = x > 45 || _angle > calculatedAngle;
                res += string.Format ("[Mult>=0/Check:{0}/", check);
            }
            else
            {
                _isMultiplierNegative = false;

                angleCheck = (_angle >= 0) ? (_angle<calculatedAngle) : (_angle> calculatedAngle);

                check = x < 45;

                if (!angleCheck)
                {
                    check &= (x > 0);
                }

                res += string.Format ("[Check:{0}/Multiplier:{1}||", check, _multiplier);

                if (angleCheck)
                {
                    _multiplier++;
                    check = false;
                    res += "_angle > calculatedAngle/";
                }
            }
        }
        else if (_direction < 0)
        {
            if (_multiplier < 0)
            {
                check = x > 45 || _angle < calculatedAngle;
                res += string.Format ("[Mult<=0/Check:{0}/", check);
            }
            else
            {
                _isMultiplierNegative = false;

                angleCheck = (_angle >= 0) ? (_angle<calculatedAngle) : (_angle> calculatedAngle);

                check = x < 45;

                res += string.Format ("[Check:{0}/Multiplier:{1}||", check, _multiplier);

                if (angleCheck)
                {
                    _multiplier--;
                    check = false;
                    res += "_angle > calculatedAngle/";
                }
            }
        }

        if (check)
        {
            _multiplier = _multiplier + (1 * _direction);
        }

        res += string.Format ("Multiplier:{0}/Direction:{1}/IsNegative:{2}]", _multiplier, _direction, _isMultiplierNegative);
        Print.Log (res);
    }

    private void ResetStates ()
    {
        Print.Log ("Reset States");
        m_Player.CanMove = true;

        foreach (MovingObjectController2D obstacle in m_Obstacles)
        {
            obstacle.CanMove = true;
        }

        m_CanDrag = true;
        m_IsDragging = false;
    }

    public void OnPlayerTriggerButtonEnter (Vector3 _position)
    {
        m_CanRotate = false;
        m_CanRegisterInput = false;
        m_CanDrag = false;
    }

    public void OnPlayerTriggerButtonExit () { }

    public void TriggerButtonAnimationFinished ()
    {
        m_CanRotate = true;
        m_CanRegisterInput = true;
        m_CanDrag = true;
    }

    public void OnFingerDown (LeanFinger _finger)
    {
        if (Utils.ACCESSIBILITY_ON)
        {
            return;
        }

        if (GameManager.Instance.State != GameState.Play || !m_Player || !m_CanRegisterInput)
        {
            return;
        }

        if (!m_CanDrag || m_Player.IsMoving)
        {
            return;
        }

        // Dragging states
        GameEvents.Instance.Event_ToggleDragging (true);
        m_IsDragging = true;
        m_CanDrag = false;
        m_StartedDragging = true;

        // Capture the initial rotation values
        Vector2 pos = m_Camera.WorldToScreenPoint (transform.position);
        pos = _finger.ScreenPosition - pos;
        m_BaseAngle = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg;
        m_BaseAngle -= Mathf.Atan2 (transform.right.y, transform.right.x) * Mathf.Rad2Deg;
    }
    public void OnFingerSet (LeanFinger _finger) { }

    public void OnFingerUp (LeanFinger _finger)
    {
        if (Utils.ACCESSIBILITY_ON)
        {
            return;
        }

        if (GameManager.Instance.State != GameState.Play || !m_Player || !m_CanRegisterInput)
        {
            return;
        }

        if (!m_StartedDragging || m_Player.IsMoving)
        {
            return;
        }

        GameEvents.Instance.Event_ToggleDragging (false);

        if (_finger.SwipeScreenDelta.magnitude > 0.0f)
        {
            Vector2 pos = m_Camera.WorldToScreenPoint (transform.position);
            pos = _finger.ScreenPosition - pos;
            float ang = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg - m_BaseAngle;

            float newAngle = GetNextRotationAngle (m_BaseAngle, ang, m_DragTime, _finger);

            StartCoroutine (Rotate (newAngle));

            Print.LogFormat (">> [Base: ({0:0.00}) | Current: ({1:0.00}) | New: ({2:0.00})] - [Drag: ({3:0.00}), Swipe delta: ({4})]", m_BaseAngle, ang, newAngle, m_DragTime, _finger.SwipeScreenDelta);
        }
        else
        {
            ResetStates ();
        }

        m_StartedDragging = false;
        m_DragTime = 0.0f;
    }

    public void OnFingerTap (LeanFinger _finger) { }

    public void OnFingerSwipe (LeanFinger _finger) { }

    public void AccessibilityRotateRequest (bool _rotateRight)
    {
        if (GameManager.Instance.State != GameState.Play || !m_Player || !m_CanRegisterInput)
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

        GameEvents.Instance.Event_ToggleDragging (true);

        float angle = transform.eulerAngles.z;

        StartCoroutine (Rotate (_rotateRight ? (angle - 90) : (angle + 90)));
    }

}