using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("Camera Easing")]
    [SerializeField] private Ease m_CameraEase;

    [Header("Screen Shake")]
    [RangeAttribute(0.3f, 1f)]
    [SerializeField] private float m_ShakeDuration = 0.5f;

    [SerializeField] private Vector3 m_OriginalPosition;

    private Camera m_Camera;

    [Header("Field Of View tap controls")]
    [SerializeField] private float m_FieldOfViewOffset = 2.5f;
    private float m_FieldOfView;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Check if there is another instance of the same type and destroy it
        if (Instance != null & Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        m_Camera = Camera.main;

        if (null == m_Camera)
        {
            m_Camera = FindObjectOfType<Camera>();
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        Game_Events.Instance.ToggleDragging += OnPlayerToggleDrag;
    }

    private void OnDisable()
    {
        if (null != Game_Events.Instance)
        {
            Game_Events.Instance.ToggleDragging -= OnPlayerToggleDrag;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_OriginalPosition = transform.position;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 7.5f);

        ResetPosition();

        m_FieldOfView = m_Camera.fieldOfView;
    }

    public static void Shake()
    {
        if (null == CameraController.Instance)
        {
            return;
        }

        CameraController.Instance.StartCoroutine(CameraController.Instance.ShakeController());
    }

    private IEnumerator ShakeController()
    {
        transform.DOShakePosition(m_ShakeDuration);

        yield return new WaitForSeconds(m_ShakeDuration);

        transform.DOMove(m_OriginalPosition, 0.35f);
    }

    public static void ResetPosition()
    {
        if (null == CameraController.Instance)
        {
            return;
        }

        CameraController.Instance.StartCoroutine(CameraController.Instance.ResetPositionController());
    }

    private IEnumerator ResetPositionController()
    {
        transform.DOMove(m_OriginalPosition, 2).SetEase(m_CameraEase);

        yield return new WaitForSeconds(2);

        GameManager.SetState(GameState.Play);
    }

    public void OnPlayerToggleDrag(bool _state)
    {
        // if (_state)
        // {
        //     m_Camera.DOFieldOfView(m_FieldOfView + m_FieldOfViewOffset, 0.5f);
        // }
        // else
        // {
        //     m_Camera.DOFieldOfView(m_FieldOfView, 0.25f);
        // }
    }

}