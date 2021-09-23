using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObject/Gun", order = 0)]
public class Gun : ScriptableObject
{
    public int baseDamage;
    public int damageModifier;
    public int amountOfChargesGiven;
    public float fireRate = 0.9f;
}
