using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : Interactable
{
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
        GameManager.CompleteLevel();
    }
}