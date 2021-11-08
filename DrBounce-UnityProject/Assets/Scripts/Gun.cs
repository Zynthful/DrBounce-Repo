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

    [Tooltip("Charge mode of the gun")]
    public Shooting.GunModes chargeShot;    //the effect the gun has when it is charged

    [Tooltip("Bullet that is fired when the gun is charged")]
    public BulletType chargeBullet;   //the bullet the gun will fire when charged

    [Range(0, 100)]
    [Tooltip("healAmount int value between 0 and 100")]
    public int healAmount;    //amount healed when using one charge

    [Header("Damage")]

    [Tooltip("X = amount of charges, Y = damage delt")]
    public Vector2[] damageGraph;   //allows for each charge state to be set in the insector as well as base damage
}
