using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPack", menuName = "ScriptableObjects/HealthPack", order = 0)]
public class HealthPackObject : ScriptableObject
{
    public int Health = 50;
    public int healModifier = 2;
}