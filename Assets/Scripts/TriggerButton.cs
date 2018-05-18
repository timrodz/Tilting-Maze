using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    [Header ("Visual debugging")]
    [SerializeField] private Color m_GizmosColor = Color.white;

    [Header ("Barrier settings")]
    [SerializeField] private EndAction m_TriggerEndAction = EndAction.None;
    [SerializeField] private bool m_AppendSequence = true;
    [SerializeField] private bool m_OverrideBarrierAnimationSettings = false;
    [SerializeField] private AnimationSettings m_AnimationSettings;

    [Header ("Barrier List")]
    [SerializeField] private List<MovableBarrier> m_BarrierList = new List<MovableBarrier> (1);

    private Sequence m_AnimationSequence;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start ()
    {
        InitializeBarrierPositions ();
    }

    void InitializeBarrierPositions ()
    {
        foreach (MovableBarrier barrier in m_BarrierList)
        {
            // Do not initialize positions if the object does not exist
            if (null == barrier.GameObject)
            {
                continue;
            }

            barrier.Setup ();
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected override void OnTriggerEnter2D (Collider2D other)
    {
        if ((int)transform.localPosition.y != 0)
        {
            return;
        }

        if (GameManager.Instance.State != GameState.Play)
        {
            return;
        }

        if (!other.CompareTag ("Player"))
        {
            return;
        }

        base.OnTriggerEnter2D (other);

        GameEvents.Instance.Event_PlayerTriggerButtonEnter (transform.position);

        AudioManager.PlayEffect (ClipType.Trigger_Button);

        AnimateBarriers ();
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    protected override void OnTriggerExit2D (Collider2D other)
    {
        if ((int)transform.localPosition.y != 0)
        {
            return;
        }

        if (m_TriggerEndAction != EndAction.Destroy)
        {
            GameEvents.Instance.Event_PlayerTriggerExit ();
        }
    }

    public void AnimateBarriers ()
    {
        if (null != m_AnimationSequence)
        {
            m_AnimationSequence.Kill ();
        }

        m_AnimationSequence = DOTween.Sequence ();

        foreach (MovableBarrier barrier in m_BarrierList)
        {
            if (null == barrier.GameObject)
            {
                continue;
            }

            Vector3 targetPosition = (!barrier.HasMoved) ? barrier.FinalPosition : barrier.OriginalPosition;

            // TODO: Find out if the tween inside the sequences can be added in a Tweet.
            if (m_AppendSequence)
            {
                m_AnimationSequence.Append (
                    barrier.Transform.DOLocalMove (targetPosition, m_OverrideBarrierAnimationSettings ? m_AnimationSettings.duration : barrier.AnimationSettings.duration)
                    .SetEase (m_OverrideBarrierAnimationSettings ? m_AnimationSettings.ease : barrier.AnimationSettings.ease)
                    .SetDelay (m_OverrideBarrierAnimationSettings ? m_AnimationSettings.delay : barrier.AnimationSettings.delay)
                    .OnComplete (() =>
                    {
                        if (m_TriggerEndAction == EndAction.None)
                        {
                            barrier.HasMoved = !barrier.HasMoved;
                        }
                    })
                );
            }
            else
            {
                m_AnimationSequence.Join (
                    barrier.Transform.DOLocalMove (targetPosition, m_OverrideBarrierAnimationSettings ? m_AnimationSettings.duration : barrier.AnimationSettings.duration)
                    .SetEase (m_OverrideBarrierAnimationSettings ? m_AnimationSettings.ease : barrier.AnimationSettings.ease)
                    .SetDelay (m_OverrideBarrierAnimationSettings ? m_AnimationSettings.delay : barrier.AnimationSettings.delay)
                    .OnComplete (() =>
                    {
                        if (m_TriggerEndAction == EndAction.None)
                        {
                            barrier.HasMoved = !barrier.HasMoved;
                        }
                    })
                );
            }

        }

        m_AnimationSequence.OnComplete (() =>
        {
            Print.LogFormat ("Button {0} - Animation finished", this.name);

            GameEvents.Instance.Event_TriggerButtonAnimationFinished ();

            if (m_TriggerEndAction == EndAction.Destroy)
            {
                Destroy (this.gameObject);
            }
        });
    }

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos ()
    {
        // Aiming to make the gizmos rotate with the level's rotation - Not important
        // Matrix4x4 matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        // Gizmos.matrix = matrix;

        Gizmos.color = m_GizmosColor;

        // Draw a wire sphere on top of the button
        Gizmos.DrawWireSphere (transform.position + Vector3.back, 0.49f);

        foreach (MovableBarrier barrier in m_BarrierList)
        {
            if (null == barrier.GameObject)
            {
                continue;
            }

            // Draws an arrow from the center of the barrier to the target position
            DrawArrow.ForGizmo2D (barrier.Position, barrier.FinalDirection, m_GizmosColor);

            // Matrix4x4 rotationMatrix = Matrix4x4.TRS(barrier.Position, transform.parent.rotation, transform.lossyScale);
            // Gizmos.matrix = rotationMatrix;

            // Draws a cube around the initial and final positions of the barrier
            Gizmos.DrawWireCube (barrier.Position, barrier.Scale);
            Gizmos.DrawWireCube (barrier.FinalPosition, barrier.Scale);
        }
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    void OnValidate ()
    {
        InitializeBarrierPositions ();
    }

}

#if UNITY_EDITOR
[CustomEditor (typeof (TriggerButton))]
public class TriggerButtonEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        TriggerButton tb = (TriggerButton) target;

        if (DrawDefaultInspector ()) { }

        if (GUILayout.Button ("Activate"))
        {
            tb.AnimateBarriers ();
        }
    }
}

#endif