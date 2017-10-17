using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles the trigger buttons
/// They derived from the 'Interactable' class
/// 
/// Author: Juan Rodriguez
/// Date: 01/10/2017
/// Version: 1.0
/// </summary>
public class TriggerButton : Interactable
{

    [Header("Visual debugging")]
    [SerializeField] private Color m_GizmosColor = Color.white;

    [Header("Barrier related")]
    // [SerializeField] private AnimationSettings m_AnimationSettings;
    [SerializeField] public EndAction m_TriggerEndAction = EndAction.None;
    [SerializeField] private List<MovableBarrier> m_BarrierList = new List<MovableBarrier>(1);

    private Sequence m_AnimationSequence;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        InitializeBarrierPositions();
    }

    void InitializeBarrierPositions()
    {
        foreach (MovableBarrier barrier in m_BarrierList)
        {
            // Do not initialize positions if the object does not exist
            if (null == barrier.GameObject)
            {
                continue;
            }
            
            barrier.Setup();
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.GetState() != GameState.Play)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        base.OnTriggerEnter2D(other);

        Game_Events.Instance.Event_PlayerTriggerEnter(transform.position);

        AudioManager.PlayEffect(ClipType.Trigger_Button);

        AnimateBarriers();

    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (m_TriggerEndAction != EndAction.Destroy)
        {
            Game_Events.Instance.Event_PlayerTriggerExit();
        }
    }

    private void AnimateBarriers()
    {
        if (null != m_AnimationSequence)
        {
            m_AnimationSequence.Kill();
        }

        m_AnimationSequence = DOTween.Sequence();

        foreach (MovableBarrier barrier in m_BarrierList)
        {
            if (null == barrier.GameObject)
            {
                continue;
            }
            
            Vector3 targetPosition = (!barrier.HasMoved) ? barrier.FinalPosition : barrier.OriginalPosition;
            
            m_AnimationSequence.Append(
                barrier.Transform.DOLocalMove(targetPosition, barrier.AnimationSettings.duration).SetEase(barrier.AnimationSettings.ease).SetDelay(barrier.AnimationSettings.delay).OnComplete(() =>
                {
                    if (m_TriggerEndAction == EndAction.None)
                    {
                        barrier.HasMoved = !barrier.HasMoved;
                    }
                })
            );
        }

        m_AnimationSequence.OnComplete(() =>
        {
            Debug.LogFormat("Button {0} - Animation finished", this.name);

            Game_Events.Instance.Event_TriggerButtonAnimationFinished();

            if (m_TriggerEndAction == EndAction.Destroy)
            {
                Destroy(this.gameObject);
            }
        });
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = m_GizmosColor;

        // Draw a wire sphere on top of the button
        Gizmos.DrawWireSphere(transform.position + Vector3.back, 0.49f);
        
        foreach (MovableBarrier barrier in m_BarrierList)
        {
            if (null == barrier.GameObject)
            {
                continue;
            }
            
            // Draws an arrow from the center of the barrier to the target position
            DrawArrow.ForGizmo2D(barrier.Position, barrier.FinalDirection, m_GizmosColor);

            // Draws a cube around the initial and final positions of the barrier
            Gizmos.DrawWireCube(barrier.Position, barrier.Scale);
            Gizmos.DrawWireCube(barrier.FinalPosition, barrier.Scale);
        }
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        InitializeBarrierPositions();
    }

}