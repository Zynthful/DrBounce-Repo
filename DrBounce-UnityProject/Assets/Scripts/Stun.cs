using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    /*
    To do:
    link up to other scripts:
        ai (so the enemy stops moving and attacking)
        hit detection (when the enemy gets hit by normal shots, throw the gun)
        ui (update a slider)
    meter slowly goes down
    */

    private bool hasBeenHit = false;    //checks if the enemy has been hit recently
    [SerializeField] private int hitsNeededtoStun = 5;      //the amount of hits needed for ther enemy to get stunned
    private int stunCounter = 0;    //the current stun value of the enemy, if it is equal to hitsNeededtoStun, then the enemy is stunned

    [SerializeField] private float timeToResetStun = 8.5f;      //the amount of time that can pass before the stun counter is reset

    private bool isStunned = false;     //if the enemy is currently stunned or not
    [SerializeField] private float timeStunnedFor = 3.5f;       //the amount of time the enemy is stunned for

    private float stunTimer = 0;        //to check if the enemy has reached the timeStunnedFor
    private float hitTimer = 0;        //to check if the enemy has reached the timeToResetStun

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            if (stunTimer >= timeStunnedFor)
            {
                StunEnded();
            }
        }
        else
        {
            if (hitTimer >= timeToResetStun && hasBeenHit)      //change to else if?
            {
                hasBeenHit = false;
                hitTimer = 0;
                stunCounter = 0;
            }
        }
    }

    /// <summary>
    /// Increase the stun counter unless the enemy is already stunned
    /// </summary>
    private void Hit() 
    {
        if (!isStunned)
        {
            hasBeenHit = true;
            hitTimer = 0;
            stunCounter++;
            if (stunCounter >= hitsNeededtoStun)
            {
                Stunned();
            }
        }
    }

    private void BigHit() 
    {
        if (!isStunned)
        {
            hasBeenHit = true;
            hitTimer = 0;
            stunCounter = hitsNeededtoStun;
            if (stunCounter >= hitsNeededtoStun)
            {
                Stunned();
            }
        }
    }

    /// <summary>
    /// Stuns the enemy, play partical effects and sound here
    /// </summary>
    private void Stunned() 
    {
        isStunned = true;
        stunTimer = 0;
        hasBeenHit = false;
        stunCounter = 0;
    }

    /// <summary>
    /// Starts once the enemy is no longer stunned
    /// </summary>
    private void StunEnded() 
    {
        isStunned = false;
    }

    /// <summary>
    /// Used to check if the enemy is currently stunned
    /// </summary>
    /// <returns></returns>
    public bool IsStunned()
    {
        return isStunned;
    }
}
