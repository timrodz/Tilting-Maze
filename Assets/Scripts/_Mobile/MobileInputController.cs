using UnityEngine;

public class MobileInputController
{
    private static MobileInputController _instance;
    public static MobileInputController Instance
    {
        get
        {
            if (_instance == null)
            {
                Instance = new MobileInputController();
            }

            return _instance;
        }

        protected set
        {
            _instance = value;
        }
    }

    public float deadzone = 100;

    private bool tap;
    private bool isDragging;
    private bool swipeLeft, swipeRight, swipeUp, swipeDown;

    private Vector2 startTouch = Vector2.zero;
    private Vector2 swipeDelta = Vector2.zero;

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // -------------------------------------------------------------------------------------------
        // Reset every frame
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        if (Input.GetMouseButtonDown(0))
        {

            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;

        }
        else if (Input.GetMouseButtonUp(0))
        {

            isDragging = false;
            ResetSwipeState();

        }

        // -------------------------------------------------------------------------------------------
        // Touching the screen
        if (Input.touches.Length > 0)
        {
            // Check the first touch and store its values as soon
            // as it's registered
            if (Input.touches[0].phase == TouchPhase.Began)
            {

                isDragging = true;
                tap = true;
                startTouch = Input.touches[0].position;

            }
            // Check if the touch has ended or has been canceled
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {

                isDragging = false;
                ResetSwipeState();

            }

        }

        // -------------------------------------------------------------------------------------------
        // Calculate the movement difference (delta)
        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (Input.touches.Length > 0)
            {

                swipeDelta = Input.touches[0].position - startTouch;

            }
            else if (Input.GetMouseButton(0))
            {

                swipeDelta = (Vector2) Input.mousePosition - startTouch;

            }

        }

        // -------------------------------------------------------------------------------------------
        // Determine if deadzone has been crossed
        if (swipeDelta.magnitude > deadzone)
        {
            // Which direction are we swiping
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            // X-axis is bigger (Left or right)
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {

                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }

            }
            // Y-axis is bigger (Up or down)
            else
            {
                if (y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }

            }

            ResetSwipeState();
        }

    }

    private void ResetSwipeState()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }

}