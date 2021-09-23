using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private int chargesLeft = 0; //gets set to 3
    [SerializeField] private int damage = 1;
    [SerializeField] private int damageModifier = 2;
    private int baseDamage = 1;
    private int amountOfBounces = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
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
        if (transform.parent != null && Input.GetMouseButtonDown(0))
        {
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
            Debug.LogWarning("You hit for " + damage);
        }
    }

    private void Bounce() 
    {
        amountOfBounces++;
        chargesLeft = 3;
    }

    private void Reset()
    {
        amountOfBounces = 0;
        chargesLeft = 0;
    }
}
