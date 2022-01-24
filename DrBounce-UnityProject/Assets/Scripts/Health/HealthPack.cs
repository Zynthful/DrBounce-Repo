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

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        controls.Player.Shoot.performed += _ => StartCoroutine(Healing());
        controls.Player.Shoot.canceled += _ => StopHealing();
    }

    private void OnDisable()
    {
        controls.Player.Shoot.performed -= _ => StartCoroutine(Healing());
        controls.Player.Shoot.canceled -= _ => StopHealing();
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
        if (transform.parent && GetComponentInParent<Health>().GetIsAtFullHealth())   // checks if the health pack has a parent (please come back dad) and if you are not at max health
        {
            //where you heal
            healing = false;
            OnActivated?.Invoke(HealingAmountCalc(amountOfBounces));
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Healing(float delay = 0.2f) 
    {
        healing = true;
        yield return new WaitForSeconds(delay);
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
    private int HealingAmountCalc(int charges) 
    {
        foreach (Vector2 amount in healingGraph)  //loops through the vector 2 (graph)
        {
            if (amount.x == charges)
            {
                return Mathf.RoundToInt(amount.y);
            }
        }
        if (charges >= healingGraph.Length) //in case you over the max
        {
            return Mathf.RoundToInt(healingGraph[healingGraph.Length - 1].y);
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
