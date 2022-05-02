using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Bullet", menuName = "ScriptableObjects/Bullet", order = 1)]
public class BulletType : ScriptableObject
{
    public Vector3 size;
    public float speed;
    public int damage;
    public float lifespan;
    public AK.Wwise.Switch typeSwitch;

    public BulletType(float siz, float spe, int dam, float life)
    {
        size = new Vector3(siz, siz, siz);
        speed = spe;
        damage = dam;
        lifespan = life;
    }
}