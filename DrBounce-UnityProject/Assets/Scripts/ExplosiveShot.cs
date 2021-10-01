using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShot : BulletMovement
{
    public int comboSize;
    private SphereCollider explosionTrigger;

    // Start is called before the first frame update
    override void Start()
    {
        objectPool = ObjectPooler.Instance;
        explosionTrigger = transform.GetComponentInChildren<SphereCollider>();
        explosionTrigger.enabled = false;
    }

   public override void OnCollisionEnter(Collider other)
    {
        if (!other.GetComponent<BulletMovement>() && !PlayerMovement.player.root)
        {
            explosionTrigger.enabled = true;

            StartCoroutine(ExplosionExpansion);

            gameObject.SetActive(false);
        }
    }

    IEnumerator ExplosionExpansion()
    {
        while(explosionTrigger.)
        {

        }
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
