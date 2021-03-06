using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.Events;

public class Stun : MonoBehaviour
{
    /*
    To do:
    make stun only work with gun

    make ui bar only apear when only should go if it is empty
    stun timer go down every half second
    */

    [SerializeField]
    private MMHealthBar stunBar;

    private bool hasBeenHit = false;    //checks if the enemy has been hit recently
    [SerializeField] private int shotsNeededtoStun = 4;      //the amount of hits needed for ther enemy to get stunned
    private float stunCounter = 0;    //the current stun value of the enemy, if it is equal to hitsNeededtoStun, then the enemy is stunned

    private bool isStunned = false;     //if the enemy is currently stunned or not
    [SerializeField] private float timeStunnedFor = 6f;       //the amount of time the enemy is stunned for

    private float stunTimer = 0;        //to check if the enemy has reached the timeStunnedFor
    [SerializeField] private float stunLoss = 0.005f;     //amount of stun value lost per frame

    private int maxStun = 0;
    private int minStun = 0;

    [Space]
    [Header("Events")]
    public UnityEvent onStun = null;
    public UnityEvent onStunEnd = null;
    public UnityEvent<float> onStunProgress = null;     // passes stun progress as a percentage between 0-1
    public UnityEvent onStunIncrease = null;            // called when hit

    private void Start()
    {
        maxStun = shotsNeededtoStun;
    }

    private void FixedUpdate()
    {
        if (!isStunned && hasBeenHit)
        {
            UpdateStunBar(true);
        }
        if (isStunned) 
        {
            UpdateStunBar(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.s_Instance.paused)
        {
            return;
        }

        if (isStunned)
        {
            //print("stun timer");

            stunTimer = stunTimer + Time.deltaTime;

            if (stunTimer >= timeStunnedFor)
            {
                StunEnded();
            }
        }
        else
        {
            if (hasBeenHit)      //change to else if?
            {
                //print("stun counter lowering");

                stunCounter = stunCounter - stunLoss;
                if (stunCounter < 0) 
                {
                    StoppingBeingHit();
                }
            }
        }
    }

    private void StoppingBeingHit() 
    {
        hasBeenHit = false;
        UpdateStunBar(false);
    }

    /// <summary>
    /// Increase the stun counter unless the enemy is already stunned (normal shot)
    /// </summary>
    public void Hit() 
    {
        //print("hit");

        if (!isStunned)
        {
            //print("hit success");

            hasBeenHit = true;
            stunCounter++;
            UpdateStunBar(true);
            if (stunCounter >= shotsNeededtoStun)
            {
                Stunned();
            }
            else
            {
                onStunIncrease?.Invoke();
            }
        }
    }

    /// <summary>
    /// Stuns the enemy after being hit (gun throw)
    /// </summary>
    public void BigHit()
    {
        //print("big hit");

        if (!isStunned)
        {
            //print("big hit success");
            stunCounter = shotsNeededtoStun;
            hasBeenHit = true;
            UpdateStunBar(true);

            Stunned();
        }
    }

    /// <summary>
    /// Stuns the enemy, play partical effects and sound here
    /// </summary>
    private void Stunned() 
    {
        //print("Stunned");
        onStun?.Invoke();

        hasBeenHit = false;
        stunCounter = shotsNeededtoStun;
        UpdateStunBar(true);
        isStunned = true;
        stunTimer = 0;
    }

    /// <summary>
    /// Starts once the enemy is no longer stunned
    /// </summary>
    private void StunEnded() 
    {
        //print("not Stunned");
        stunCounter = 0;
        UpdateStunBar(false);
        onStunEnd?.Invoke();
        stunTimer = 0;
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

    protected virtual void UpdateStunBar(bool showBar)
    {
        if (stunBar != null)
        {
            stunBar.UpdateBar(stunCounter, minStun, maxStun, showBar);
            onStunProgress?.Invoke(stunCounter / (float)maxStun);
        }
    }
}
