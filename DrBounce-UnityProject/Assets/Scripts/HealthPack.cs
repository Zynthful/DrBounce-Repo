using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private HealthPackObject healthPack = null;

    public delegate void Activated(int value);
    public static event Activated OnActivated;

    public InputMaster controls;

    private int amountOfBounces;
    private bool healing;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => StartCoroutine(Healing());
        controls.Player.Shoot.canceled += _ => StopHealing();
    }

    public void Bounce(int bounceCount)
    {
        amountOfBounces = bounceCount;
        //feedback for when the when the health pack is catched
    }

    public void Dropped()
    {
        if (!transform.parent)  //if the health pack is dropped and has no parent
        {
            amountOfBounces = 0;
            //feedback for when the health pack losses its charge
        }
    }

    private void Heal() 
    {
        if (transform.parent && Health.ReturnHealthNotMax())   //checks if the health pack has a parent, please come back dad and if you are not at max health
        {
            //where you heal
            healing = false;
            OnActivated?.Invoke(HealingEquation(amountOfBounces));
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Healing() 
    {
        healing = true;
        yield return new WaitForSeconds(0.2f);
        if (healing) 
        {
            Heal();
        }
    }

    private int HealingEquation(int amountOfBounces) 
    {

        float healAmount;

        if (amountOfBounces == 0)   //default healing value
        {
            healAmount = 33;
        }
        else if (amountOfBounces > 5)    //stop you going down the healing curve
        {
            healAmount = 100;
        }
        else
        {
            float a = 0.5f;
            float b = 11.5f;
            float c = 55f;

            healAmount = Mathf.Pow(amountOfBounces, 2) * -a;
            healAmount += (amountOfBounces * b);
            healAmount += c;
        }

        return Mathf.RoundToInt(healAmount);
    }

    private void StopHealing() 
    {
        if (healing) 
        {
            healing = false;
        }
        
    }
}
