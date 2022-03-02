using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 0)]
public class Gun : ScriptableObject
{
    [Header("Normal Gun Shots")]

    [Range(0.0f, 10f)]
    [Tooltip("fireRate float value between 0 and 10")]
    public float fireRate = 0.21f;    //time between shots, in seconds

    [Range(0, 1000)]
    [Tooltip("normalRange int value between 0 and 1000")]
    public float normalRange = 100f;     //the range of uncharged shots

    [Tooltip("can you hold to shoot")]
    public bool canRepeatShoot = true;

    [Header("Charge Gun Shots")]

    [Tooltip("How many charges can you get max in this level")]
    public int maxCharges = 3;

    [Tooltip("do you use all charges upon using one")]
    public bool useAllChargesOnUse = false;

    [Tooltip("Charge mode of the gun")]
    public Shooting.GunModes chargeShot = Shooting.GunModes.Explosives;    //the effect the gun has when it is charged

    [Tooltip("Bullet that is fired when the gun is charged")]
    public BulletType chargeBullet = null;   //the bullet the gun will fire when charged

    [Header("Damage")]

    [Tooltip("X = amount of charges, Y = damage delt")]
    public List<Vector2> damageGraph = new List<Vector2> {new Vector2(0, 5), new Vector2(1, 100)};   //allows for each charge state to be set in the insector as well as base damage

    [Header("Healing")]

    [Tooltip("X = amount of charges, Y = health healed")]
    public List<Vector2> healGraph = new List<Vector2> {new Vector2(0, 10), new Vector2(0, 30)};   //allows for each charge state to be set in the insector as well as base healing

    [Header("Max Shot Settings")]

    [Range(0, 10)]
    [Tooltip("Duration in seconds that the shoot button must be held in order to fully charge a max charge shot.")]
    public float holdTimeToFullCharge = 1.0f;

    [Range(0, 1)]
    [Tooltip("Percentage that the max shot must be charged before cancelling it does NOT trigger a regular shot.")]
    public float chargeCancelThreshold = 0.5f;

    [Range(0, 1)]
    [Tooltip("Percentage that the shoot control must be held for before beginning a max shot charge")]
    public float chargeBeginThreshold = 0.25f;

    [Range(0, 100)]
    [Tooltip("Minimum charges to begin charging a max shot charge")]
    public int minChargeToMaxShot = 1;
}
