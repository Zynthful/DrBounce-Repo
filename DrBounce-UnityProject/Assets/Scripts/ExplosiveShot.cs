using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveShot : BulletMovement
{
    public int comboSize;
    [SerializeField] float explosionSizeMultiplier;
    [SerializeField] int maxComboSize;
    private SphereCollider explosionTrigger;

    [SerializeField] [Range(10f, 1000f)] float expansionSpeed;
    private MeshRenderer shotRenderer;
    private Rigidbody rb;

    public override void OnObjectSpawn()
    {
        if(shotRenderer == null)
        {
            explosionTrigger = transform.GetComponentInChildren<SphereCollider>();
            shotRenderer = GetComponentInChildren<MeshRenderer>();
            rb = GetComponent<Rigidbody>();
        }
        
        shotRenderer.enabled = true;
        explosionTrigger.enabled = false;
        
        base.OnObjectSpawn();
    }

   public void OnCollisionEnter(Collision other)
    {
        if (!other.transform.GetComponent<BulletMovement>() && other.transform.root != PlayerMovement.player.root)
        {
            explosionTrigger.enabled = true;

            GetComponentInChildren<MeshRenderer>().enabled = false; GetComponent<Rigidbody>().velocity = Vector3.zero;

            StartCoroutine(ExplosionExpansion());
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        return;
    }

    IEnumerator ExplosionExpansion()
    {
        while(explosionTrigger.radius < explosionSizeMultiplier * Mathf.Clamp(comboSize, 1, maxComboSize))
        {
            explosionTrigger.radius += ((comboSize * explosionSizeMultiplier) / expansionSpeed);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
