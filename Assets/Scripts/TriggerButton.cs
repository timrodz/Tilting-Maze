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

    public enum EndAction
    {
        None,
        Destroy
    }

    [Header("Visual debugging")]
    [SerializeField] private Color m_GizmosColor = Color.white;

    [Header("Barrier related")]
    [Range(0.25f, 2f)]
    [SerializeField] private float m_AnimationDuration = 1f;
    [SerializeField] private Ease m_AnimationEase;
    [SerializeField] private EndAction m_AnimationEndAction = EndAction.Destroy;
    [SerializeField] private List<MovableBarrier> m_BarrierList = new List<MovableBarrier>(1);

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
        for (int i = 0; i < m_BarrierList.Count; i++)
        {
            if (null == m_BarrierList[i].GameObject())
            {
                continue;
            }

            m_BarrierList[i].SetOriginalPosition(m_BarrierList[i].Transform().position);

            m_BarrierList[i].SetFinalPosition(m_BarrierList[i].GetOriginalPosition() + m_BarrierList[i].GetFinalDirection());

            // Debug.LogFormat("Barrier ({0}), Original Position::{1}--Final Position::{2}", m_BarrierList[i].GameObject().name, m_BarrierList[i].GetOriginalPosition(), m_BarrierList[i].GetFinalPosition());
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

        StartCoroutine(LevelControllerHandler());

        for (int i = 0; i < m_BarrierList.Count; i++)
        {
            StartCoroutine(MoveBarrier(m_BarrierList[i]));
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected override void OnTriggerExit2D(Collider2D other)
    {
        Game_Events.Instance.Event_PlayerTriggerExit();
    }

    private IEnumerator LevelControllerHandler()
    {
        yield return new WaitForSeconds(m_AnimationDuration);

        Game_Events.Instance.Event_TriggerButtonAnimationFinished();
    }

    private IEnumerator MoveBarrier(MovableBarrier barrier)
    {
        Vector3 finalPosition = (!barrier.HasMoved()) ? barrier.GetFinalPosition() : barrier.GetOriginalPosition();

        barrier.Transform().DOLocalMove(finalPosition, m_AnimationDuration).SetEase(m_AnimationEase);

        // Give a bit of delay in case of any glitches
        yield return new WaitForSeconds(m_AnimationDuration);

        Debug.LogFormat("----> Moving barrier ({0}) to position::{1}", barrier.GameObject().name, finalPosition);

        switch (m_AnimationEndAction)
        {
            case EndAction.None:
                {
                    barrier.SetHasMoved(!barrier.HasMoved());

                    if (barrier.ShouldDeleteFromList())
                    {
                        m_BarrierList.Remove(barrier);
                    }
                }
                break;
            case EndAction.Destroy:
                {
                    Destroy(this.gameObject);
                }
                break;
        }

    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = m_GizmosColor;

        // Draw a wire sphere on top of the button
        Gizmos.DrawWireSphere(transform.position + Vector3.back, 0.45f);

        for (int i = 0; i < m_BarrierList.Count; i++)
        {
            Vector3 position = m_BarrierList[i].GetPosition();
            Vector3 scale = m_BarrierList[i].GetScale();

            // Draws an arrow from the center of the barrier to the target position
            DrawArrow.ForGizmo2D(position, m_BarrierList[i].GetFinalDirection(), m_GizmosColor);

            // Draws a cube around the initial and final positions of the barrier
            Gizmos.DrawWireCube(position, scale);
            Gizmos.DrawWireCube(m_BarrierList[i].GetWorldFinalPosition(), scale);
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