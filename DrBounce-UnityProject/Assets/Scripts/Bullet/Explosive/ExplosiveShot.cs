using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosiveShot : BulletMovement
{
    public int comboSize;
    [SerializeField] float explosionSizeMultiplier;
    [SerializeField] int maxComboSize;
    [SerializeField] float explosionDamageMultiplier;
    private GameObject explosionTrigger;
    private bool expanding;

    [SerializeField] [Range(10f, 1000f)] float expansionSpeed;
    private MeshRenderer shotRenderer;
    private MeshCollider shotModelCollider;
    private CheckForBouncing bounceCheck;

    [Header("Explosive Shot Events")]
    public UnityEvent onExplode = null;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();

        expanding = false;

        if (!shotRenderer || !explosionTrigger || !shotModelCollider || !bounceCheck)
        {
            explosionTrigger = GetComponentInChildren<SphereCollider>().gameObject;
            shotRenderer = GetComponent<MeshRenderer>();
            shotModelCollider = GetComponent<MeshCollider>();
            bounceCheck = GetComponent<CheckForBouncing>();
        }

        shotRenderer.enabled = true;
        shotModelCollider.enabled = true;
        explosionTrigger.SetActive(false);
        
        rb.constraints = RigidbodyConstraints.None;
    }

   public void OnCollisionEnter(Collision other)
    {
        if (!other.transform.GetComponentInChildren<BulletMovement>() && other.transform.root != PlayerMovement.player.root && !expanding)
        {
            if (GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << other.gameObject.layer))
            {
                if (bounceCheck.CanBounce(other.gameObject))
                {
                    return;
                }
            }

            explosionTrigger.SetActive(true);
            //if (comboSize > 1 && explosionDamageMultiplier > 0)
            //dam = (int)(dam * comboSize * explosionDamageMultiplier);
            rb.constraints = RigidbodyConstraints.FreezeAll;

            shotRenderer.GetComponent<MeshCollider>().enabled = false;
            shotRenderer.enabled = false; rb.velocity = Vector3.zero;

            StartCoroutine(ExplosionExpansion());

            onExplode?.Invoke();
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        return;
    }

    IEnumerator ExplosionExpansion()
    {
        expanding = true;

        explosionTrigger.GetComponent<ExplosionDamageTrigger>().damage = dam;

        comboSize = Mathf.Clamp(comboSize, 1, maxComboSize);

        explosionTrigger.transform.localScale = Vector3.one * (.1f / explosionTrigger.transform.lossyScale.x);

        float startScale = explosionTrigger.transform.lossyScale.x;

        while(explosionTrigger.transform.localScale.x < (explosionSizeMultiplier * comboSize) * (.1f / startScale))
        {
            explosionTrigger.transform.localScale += Vector3.one * ((comboSize * explosionSizeMultiplier) * (.1f / startScale)) / expansionSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(.25f);
        gameObject.SetActive(false);
    }
}
