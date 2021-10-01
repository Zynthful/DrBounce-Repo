using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private HealthPackObject healthPack = null;

    private bool hasBounced = false;

    private bool isPickupable = false;
        private bool activated = false;

    public delegate void Entered();
    public static event Entered OnEntered;

    public delegate void Activated(int value);
    public static event Activated OnActivated;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPickupable && activated)
        {
            OnActivated?.Invoke((int)healthPack.Health);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject?.tag == "Player")
        {
            isPickupable = true;
            OnEntered?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject?.tag == "Player")
        {
            isPickupable = false;
        }
    }

    private void CanHeal()
    {
        activated = true;
    }

    private void OnEnable()
    {
        Health.ReportHealth += CanHeal;
    }

    private void OnDisable()
    {
        Health.ReportHealth -= CanHeal;
    }
}
