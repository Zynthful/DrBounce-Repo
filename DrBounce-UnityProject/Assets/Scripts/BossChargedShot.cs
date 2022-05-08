using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossChargedShot : BulletMovement
{
    public int comboSize;
    [SerializeField] float explosionSizeMultiplier;
    [SerializeField] int maxComboSize;
    [SerializeField] float explosionDamageMultiplier;
    private GameObject explosionTrigger;
    private bool expanding;

    [SerializeField] [Range(.01f, 50f)] float expansionSpeed;
    [SerializeField] [Range(.01f, 50f)] float shrinkSpeed;
    private MeshRenderer shotRenderer;
    private MeshCollider shotModelCollider;
    private CheckForBouncing bounceCheck;
    private Vector3 explosionTriggerBaseScale = Vector3.zero;
    [SerializeField] private float explosionLifespan = .25f;

    [Header("Explosive Shot Events")]
    public UnityEvent onExplode = null;
    public UnityEvent onShrank = null;

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();

        expanding = false;

        comboSize = 1;

        if (!shotRenderer || !explosionTrigger || !shotModelCollider || !bounceCheck)
        {
            explosionTrigger = GetComponentInChildren<SphereCollider>().gameObject;
            shotRenderer = GetComponent<MeshRenderer>();
            shotModelCollider = GetComponent<MeshCollider>();
            bounceCheck = GetComponent<CheckForBouncing>();
        }

        shotRenderer.enabled = true;
        shotModelCollider.enabled = true;

        if(explosionTriggerBaseScale == Vector3.zero)
            explosionTriggerBaseScale = explosionTrigger.transform.localScale;

        explosionTrigger.transform.localScale = explosionTriggerBaseScale;
        explosionTrigger.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.transform.GetComponentInChildren<BulletMovement>() && other.gameObject.GetComponent<Enemy>() == null && !expanding)
        {
            if (GameManager.s_Instance.bounceableLayers == (GameManager.s_Instance.bounceableLayers | 1 << other.gameObject.layer))
            {
                if (bounceCheck.CanBounce(other.gameObject))
                {
                    return;
                }
            }

            // Check for any ignored layers, and if we haven't found one, we've hit something not ignored
            bool foundIgnoredLayer = false;
            for (int i = 0; i < layersToIgnore.Length; i++)
            {
                // Check if colliding object's layer matches ignored layer (h e l p)
                if ((layersToIgnore[i].value & (1 << other.gameObject.layer)) > 0)
                {
                    foundIgnoredLayer = true;
                    break;
                }
            }
            if (!foundIgnoredLayer)
            {
                explosionTrigger.SetActive(true);
                //if (comboSize > 1 && explosionDamageMultiplier > 0)
                //dam = (int)(dam * comboSize * explosionDamageMultiplier);
                rb.constraints = RigidbodyConstraints.FreezeAll;

                shotRenderer.GetComponent<MeshCollider>().enabled = false;
                shotRenderer.enabled = false; rb.velocity = Vector3.zero;

                onExplode?.Invoke();

                StartCoroutine(ExplosionExpansion());
            }
        }
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
        yield return new WaitForSeconds(explosionLifespan);
        while (explosionTrigger.transform.localScale.x > 0)
        {
            explosionTrigger.transform.localScale -= Vector3.one * ((comboSize * explosionSizeMultiplier) * (.1f / startScale)) / shrinkSpeed;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        onShrank?.Invoke();
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
