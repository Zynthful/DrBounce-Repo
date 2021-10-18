using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    //[SerializeField] private HealthPackObject healthPack = null;

    public delegate void Activated(int value);
    public static event Activated OnActivated;

    public InputMaster controls;

    private int amountOfBounces;
    private bool healing;

    [Header("Healing Values")]
    [SerializeField] private int minAmountHealed;   //33
    [SerializeField] private int MaxAmountHealed;   //100

    [Header("Quadratic Values")]
    [Tooltip("Caps max healing, heals less")]
    [SerializeField] private float a;

    [Tooltip("Makes each bounce heal more")]
    [SerializeField] private float b;

    [Tooltip("Heal more will less bounces")]
    [SerializeField] private float c;

    // Start is called before the first frame update
    void Start()
    {
        //cc Daniel Neale 2021
        a *= -1;    //flips the a value
        if (MaxAmountHealed > MaxXValue()) 
        {
            MaxAmountHealed = MaxXValue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MaxXValue(); not sure why this is here?
    }


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Shoot.performed += _ => StartCoroutine(Healing());
        controls.Player.Shoot.canceled += _ => StopHealing();
    }

    public void Bounce(int bounceCount)
    {
        amountOfBounces++; //= bounceCount;
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
        if (transform.parent && Health.ReturnHealthNotMax())   //(checks if the health pack has a parent, please come back dad) and if you are not at max health
        {
            //where you heal
            healing = false;
            OnActivated?.Invoke(HealingAmpuntCalc(amountOfBounces));
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

    /// <summary>
    /// Calculates how much health the player will heal based on the charge of the medkit
    /// </summary>
    /// <param name="amountOfBounces"></param>
    /// <returns></returns>
    private int HealingAmpuntCalc(int charges) 
    {

        float healAmount;
        int topOfCurve = MaxXValue();

        if (charges == 0)   //default healing value, no bounces
        {
            healAmount = minAmountHealed;
        }
        else if (charges > topOfCurve)    //stop you going down the healing curve (max heal), several bounces
        {
            healAmount = MaxAmountHealed;
        }
        else    //occures if the above two don't, at least one bounce
        {
            healAmount = Equation(charges);
        }

;       return Mathf.RoundToInt(healAmount);
    }

    /// <summary>
    /// the quadratic equation of the curve
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private int Equation(float x)  //-0.5x^2 + 11.5x + 55
    {
        return Mathf.RoundToInt( Mathf.Pow(x, 2) * a + (amountOfBounces * b) + c);
    }

    /// <summary>
    /// Calculates the max x value of the curve
    /// </summary>
    /// <returns></returns>
    private int MaxXValue()     
    {
        return Mathf.RoundToInt((-b) / (2 * a));
    }

    /// <summary>
    /// Calculates the max y value of the curve
    /// </summary>
    /// <returns></returns>
    private int MaxYValue()
    {
        return Mathf.RoundToInt(Equation(MaxXValue()));
    }

    private void StopHealing() 
    {
        if (healing) 
        {
            healing = false;
        }
    }
}
