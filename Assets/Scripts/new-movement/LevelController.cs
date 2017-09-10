using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

#if UNITY_STANDALONE || UNITY_EDITOR
using XboxCtrlrInput;
#endif

/// <summary>
/// Controls the level (New implementation)
/// 
/// Author: Juan Rodriguez
/// Date: 10/09/2017
/// </summary>
public class LevelController : MonoBehaviour
{
    [SerializeField] private bool INVERT_MOVEMENT = false;

    [SerializeField] private PlayerController2D m_Player;

    [SerializeField] public bool m_CanRotate = true;
    [SerializeField] public bool m_RegisterInput = true;
    [SerializeField] private Ease m_RotationEaseType = Ease.OutQuad;
    [SerializeField] private float m_RotationLength = 0.4f;

    private int m_AxisZ = 0;

    void Start()
    {
        m_Player = FindObjectOfType<PlayerController2D>();
    }

    void Update()
    {
        if (
            // Not currently rotating
            m_CanRotate
			// Player can move
			&& (m_Player.m_CanMove)
            // Not currently colliding with anything
            &&
            (m_Player.m_CollisionInfo.above || m_Player.m_CollisionInfo.right || m_Player.m_CollisionInfo.below || m_Player.m_CollisionInfo.left)
        )
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            if (!INVERT_MOVEMENT)
            {
                if (Input.GetKey(KeyCode.E) || XCI.GetButtonDown(XboxButton.RightBumper) || (XCI.GetAxisRaw(XboxAxis.RightTrigger) != 0))
                {
                    StartCoroutine(Rotate(true));
                }
                else if (Input.GetKey(KeyCode.Q) || XCI.GetButtonDown(XboxButton.LeftBumper) || (XCI.GetAxisRaw(XboxAxis.LeftTrigger) != 0))
                {
                    StartCoroutine(Rotate(false));
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Q) || XCI.GetButtonDown(XboxButton.LeftBumper) || (XCI.GetAxisRaw(XboxAxis.LeftTrigger) != 0))
                {
                    StartCoroutine(Rotate(true));
                }
                else if (Input.GetKey(KeyCode.E) || XCI.GetButtonDown(XboxButton.RightBumper) || (XCI.GetAxisRaw(XboxAxis.RightTrigger) != 0))
                {
                    StartCoroutine(Rotate(false));
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
            if (MobileInputController.Instance.SwipeRight)
            {
                StartCoroutine(Rotate(true));
            }
            else if (MobileInputController.Instance.SwipeLeft)
            {
                StartCoroutine(Rotate(false));
            }
#endif
        }

    }

    /// <summary>
    /// Rotates the level
    /// </summary>
    public IEnumerator Rotate(bool _shouldRotateRight)
    {
        m_Player.m_CanMove = false;

        m_CanRotate = false;

        Vector3 eulerRotation = transform.eulerAngles;

        if (_shouldRotateRight)
        {
            eulerRotation.z -= 90;
            AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeRight);
        }
        else
        {
            eulerRotation.z += 90;
            AnalyticsManager.Instance.RegisterCustomEventSwipe(eCustomEvent.SwipeLeft);
        }

        // Rotate the transform
        transform.DORotate(eulerRotation, m_RotationLength).SetEase(m_RotationEaseType);

        float wait = m_RotationLength * 0.2f;

        yield return new WaitForSeconds(wait);

        yield return new WaitForSeconds(m_RotationLength - wait);

        m_CanRotate = true;

        yield return new WaitForSeconds(0.05f);

        m_Player.m_CanMove = true;
    }

}