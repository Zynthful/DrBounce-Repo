using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int baseDamage = 1;    //default damage of the gun 
    [SerializeField] private int damageModifier = 2;    //multiplied with the amount of bounces 
    private int damage = 1;     //current damage value

    [Header("Charges")]
    private int amountOfBounces = 0;    //amount of times the gun has been bounced successfully
    [SerializeField] private int amountOfChargesGiven = 3;   //amount of charges given when you rebound the gun
    private int chargesLeft = 0;    //current amount of charges left in the gun (reset if dropped)

    [Header("Fire Rate")]
    [SerializeField] private float fireRate = 0.9f;    //time between shots
    private float timeSinceLastShot = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();

        timeSinceLastShot += Time.deltaTime;
    }

    private void OnEnable()
    {
        GunBounce.OnPickUp += Bounce;
        GunBounce.OnFloorCollision += Reset;
    }

    private void OnDisable()
    {
        GunBounce.OnPickUp -= Bounce;
        GunBounce.OnFloorCollision += Reset;
    }

    private void Shoot() 
    {
        if (transform.parent != null && Input.GetMouseButtonDown(0) && timeSinceLastShot > fireRate)    //checks the object has a parent and is not already shooting
        {
            timeSinceLastShot = 0;

            if (chargesLeft > 0)
            {
                damage = amountOfBounces * damageModifier;
                chargesLeft--;
            }
            else 
            {
                damage = baseDamage;
            }

            Debug.LogWarning("BANG!!!");
            Debug.LogWarning("You shot for " + damage);
        }
    }

    private void Bounce() 
    {
        amountOfBounces++;
        chargesLeft = amountOfChargesGiven;
    }

    private void Reset()
    {
        amountOfBounces = 0;
        chargesLeft = 0;
    }
}