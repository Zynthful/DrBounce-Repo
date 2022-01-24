using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun", order = 0)]
public class Gun : ScriptableObject
{
    [Header("Normal Gun Shots")]

    [Range(0.0f, 10f)]
    [Tooltip("fireRate float value between 0 and 10")]
    public float fireRate;    //time between shots, in seconds

    [Range(0, 1000)]
    [Tooltip("normalRange int value between 0 and 1000")]
    public float normalRange;     //the range of uncharged shots

    [Tooltip("can you hold to shoot")]
    public bool canRepeatShoot;

    [Header("Charge Gun Shots")]

    [Tooltip("do you use all charges upon using one")]
    public bool useAllChargesOnUse;

    [Tooltip("Charge mode of the gun")]
    public Shooting.GunModes chargeShot;    //the effect the gun has when it is charged

    [Tooltip("Bullet that is fired when the gun is charged")]
    public BulletType chargeBullet;   //the bullet the gun will fire when charged

    [Header("Damage")]

    [Tooltip("X = amount of charges, Y = damage delt")]
    public Vector2[] damageGraph;   //allows for each charge state to be set in the insector as well as base damage

    [Header("Healing")]

    [Tooltip("X = amount of charges, Y = health healed")]
    public Vector2[] healGraph;   //allows for each charge state to be set in the insector as well as base healing

    [Header("Max Shot Settings")]

    [Range(0, 10)]
    [Tooltip("Duration in seconds that the shoot button must be held in order to fully charge a max charge shot.")]
    public float holdTimeToFullCharge; //1.0f;

    [Range(0, 1)]
    [Tooltip("Percentage that the max shot must be charged before cancelling it does NOT trigger a regular shot.")]
    public float chargeCancelThreshold; //0.5f

    [Range(0, 1)]
    [Tooltip("Percentage that the shoot control must be held for before beginning a max shot charge")]
    public float chargeBeginThreshold; //0.25f

    [Range(0, 100)]
    [Tooltip("Minimum charges to begin charging a max shot charge")]
    public int minChargeToMaxShot; //1
}
