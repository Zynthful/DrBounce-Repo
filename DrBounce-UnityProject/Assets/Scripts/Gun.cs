using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObject/Gun", order = 0)]
public class Gun : ScriptableObject
{
    public int baseDamage;    //default damage of the gun 
    public float damageModifier;    //multiplied with the amount of bounces 
    public int amountOfChargesGiven;    //amount of charges given when you rebound the gun
    public float fireRate;    //time between shots
}
