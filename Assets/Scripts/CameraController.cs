using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header ("Camera Easing")]
    [SerializeField] private Ease m_CameraEase;

    [Header ("Screen Shake")]
    [RangeAttribute (0.3f, 1f)]
    [SerializeField] private float m_ShakeDuration = 0.5f;

    [SerializeField] private Vector3 m_OriginalPosition;

    private Camera m_Camera;

    [Header ("Field Of View tap controls")]
    [SerializeField] private float m_FieldOfViewOffset = 2.5f;
    private float m_FieldOfView;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake ()
    {
        m_Camera = Camera.main;

        if (null == m_Camera)
        {
            m_Camera = FindObjectOfType<Camera> ();
        }

        m_OriginalPosition = transform.position;

        transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 7.5f);

        m_FieldOfView = m_Camera.fieldOfView;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable ()
    {
        GameEvents.Instance.ToggleDragging += OnPlayerToggleDrag;
        GameEvents.Instance.PlayerCollision += Shake;
        GameEvents.Instance.NewLevelLoaded += ZoomIn;
    }

    // Use this for initialization
    void Start ()
    {
        ZoomIn ();
    }

    public void Shake ()
    {
        StartCoroutine (ShakeController ());
    }

    private IEnumerator ShakeController ()
    {
        transform.DOShakePosition (m_ShakeDuration);

        yield return new WaitForSeconds (m_ShakeDuration);

        transform.DOMove (m_OriginalPosition, 0.35f);
    }

    public void ZoomIn ()
    {
        StartCoroutine (ResetPositionController (2));
    }

    private IEnumerator ResetPositionController (float _delay)
    {
        transform.DOMove (m_OriginalPosition, _delay).SetEase (m_CameraEase);

        yield return new WaitForSeconds (_delay);

        GameManager.SetState (GameState.Play);
    }

    public void OnPlayerToggleDrag (bool _state)
    {
        // if (_state)
        // {
        //     m_Camera.DOFieldOfView (m_FieldOfView + m_FieldOfViewOffset, 0.5f);
        // }
        // else
        // {
        //     m_Camera.DOFieldOfView (m_FieldOfView, 0.25f);
        // }
    }

}