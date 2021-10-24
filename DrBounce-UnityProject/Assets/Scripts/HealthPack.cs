using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    //[SerializeField] private HealthPackObject healthPack = null;

    [SerializeField] private Vector2[] healingGraph;

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
        foreach (Vector2 amount in healingGraph) 
        {
            if (amount.x == charges)
            {
                return Mathf.RoundToInt(amount.y);
            }
        }
        return 0;
    }

    private void StopHealing() 
    {
        if (healing) 
        {
            healing = false;
        }
    }
}
