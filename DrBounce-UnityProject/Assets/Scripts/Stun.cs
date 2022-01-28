using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    /*
    To do:
    link up to other scripts:
        ai (so the enemy stops moving and attacking) DONE BABY
        hit detection (when the enemy gets hit by normal shots, gets hit by the thrown gun)
        ui (update a slider)
    */

    private bool hasBeenHit = false;    //checks if the enemy has been hit recently
    [SerializeField] private int shotsNeededtoStun = 5;      //the amount of hits needed for ther enemy to get stunned
    private float stunCounter = 0;    //the current stun value of the enemy, if it is equal to hitsNeededtoStun, then the enemy is stunned

    [SerializeField] private float timeToResetStun = 8.5f;      //the amount of time that can pass before the stun counter is reset

    private bool isStunned = false;     //if the enemy is currently stunned or not
    [SerializeField] private float timeStunnedFor = 3.5f;       //the amount of time the enemy is stunned for

    private float stunTimer = 0;        //to check if the enemy has reached the timeStunnedFor
    [SerializeField] private float stunLoss = 0.1f;     //amount of stun value lost per frame

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            if (stunTimer >= timeStunnedFor)
            {
                stunTimer = stunTimer + Time.deltaTime;
                StunEnded();
            }
        }
        else
        {
            if (hasBeenHit)      //change to else if?
            {
                stunCounter = stunCounter - stunLoss;
                if (stunCounter <= 0) 
                {
                    hasBeenHit = false;
                }
            }
        }
    }

    /// <summary>
    /// Increase the stun counter unless the enemy is already stunned (normal shot)
    /// </summary>
    private void Hit() 
    {
        if (!isStunned)
        {
            hasBeenHit = true;
            stunCounter++;
            if (stunCounter >= shotsNeededtoStun)
            {
                Stunned();
            }
        }
    }

    /// <summary>
    /// Stuns the enemy after being hit (gun throw)
    /// </summary>
    private void BigHit()
    {
        if (!isStunned)
        {
            Stunned();
        }
    }

    /// <summary>
    /// Stuns the enemy, play partical effects and sound here
    /// </summary>
    private void Stunned() 
    {
        hasBeenHit = false;
        stunCounter = shotsNeededtoStun;
        isStunned = true;
        stunTimer = 0;
    }

    /// <summary>
    /// Starts once the enemy is no longer stunned
    /// </summary>
    private void StunEnded() 
    {
        stunCounter = 0;
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
