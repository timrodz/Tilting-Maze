/// <summary>
/// Handles control using the Controller2D class
/// 
/// Author: Juan Rodriguez
/// Date: 10/09/2017
/// </summary>

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController2D : MovingObjectController2D
{
    protected override void OnPlayerTriggerButtonEnter (Vector3 _position)
    {
        m_HasTriggeredButton = true;
        
        // Need the player not to process collisions when the trigger button is entered
        // Also the player should not move and the vertical velocity should be set to 0

        // This is done because processing collisions happen every frame
        m_CanProcessCollisions = false;
        m_Velocity.y = 0;
        CanMove = false;
        IsMoving = false;

        // I will then proceed to move the player towards the center of the trigger buton
        transform.DOMove (_position, 0.1f).OnComplete (() =>
        {
            // Since there's no way to process other collisions, I raycast one unit below to check if there's a barrier
            RaycastHit2D hit = Physics2D.Raycast (transform.position, DownDirection, 1, m_CollisionMask);

            // If a barrier exists, then animate the collision
            if (hit)
            {
                AnimateCollision ();

                Print.LogFormat ("Hit distance: {0}, Object: {1}", hit.distance, hit.transform.name);
            }
            // If no barrier exists, proceed to process collisions normally
            else
            {
                m_CanProcessCollisions = true;
                m_HasTriggeredButton = false;
            }

        });
    }
}