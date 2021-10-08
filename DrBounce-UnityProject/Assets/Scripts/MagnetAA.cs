using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAA : MonoBehaviour
{

    public InputMaster controls;
    [SerializeField] GunBounce gun;
    [Range(0.0f, 10.0f)] public float aimAssistMaxRange;
    [SerializeField] private float aimAssistForce;
    [SerializeField] GameEventBool assistEvent;
    bool assistActive;
    Rigidbody rb;

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.ThrowGun.started += _ => AssistStarted();
        controls.Player.ThrowGun.canceled += _ => AssistEnded();
    }

    void AssistStarted()
    {
        if (!gun.transform.parent)
        {
            assistActive = true; assistEvent.Raise(true);
        }
    }
    void AssistEnded() 
    {
        if (assistActive)
        {
            assistActive = false; assistEvent.Raise(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!gun)
            gun = PlayerMovement.player.GetComponentInChildren<GunBounce>();
            if(!rb)
                rb = gun.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(assistActive)
            MoveObjects();
    }

    void MoveObjects()
    {
        float currentDistance = Vector3.Distance(transform.position, gun.transform.position);
        if (!gun.transform.parent && currentDistance < aimAssistMaxRange && gun.inFlight)
        {
            float mag = rb.velocity.magnitude;
            rb.velocity = (rb.velocity + ((transform.position - gun.transform.position).normalized * aimAssistForce / currentDistance)).normalized * mag;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.ThrowGun.started -= _ => AssistStarted();
        controls.Player.ThrowGun.canceled -= _ => AssistEnded();
        controls.Disable();
    }
}
