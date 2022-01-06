using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
public class MagnetAA : MonoBehaviour
{

    public InputMaster controls;
    [SerializeField] GunThrowing gun;
    [Range(0.0f, 50f)] public float aimAssistMaxRange;
    [SerializeField] private float aimAssistForce;
    bool assistActive;
    Rigidbody rb;

    [Header("Events")]
    [SerializeField]
    private GameEventBool onIsActive = null;

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        controls.Player.ThrowGun.started += _ => AssistStarted();
        controls.Player.ThrowGun.canceled += _ => AssistEnded();
    }

    private void OnDisable()
    {
        controls.Player.ThrowGun.started -= _ => AssistStarted();
        controls.Player.ThrowGun.canceled -= _ => AssistEnded();
    }

    void AssistStarted()
    {
        if (!gun.transform.parent)
        {
            assistActive = true;
            onIsActive?.Raise(true);
        }
    }
    void AssistEnded()
    {
        if (assistActive)
        {
            assistActive = false;
            onIsActive?.Raise(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!gun)
            gun = PlayerMovement.player.GetComponentInChildren<GunThrowing>();
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
        gun.AffectPhysics(0f, 0f);
        float currentDistance = Vector3.Distance(transform.position, gun.transform.position);
        if (!gun.transform.parent && currentDistance < aimAssistMaxRange)
        {
            float mag = rb.velocity.magnitude;
            if (rb.velocity.magnitude <= .2f)
            {
                mag = 2;
            }
            mag += Time.deltaTime * aimAssistForce;
            rb.velocity = ((transform.position - gun.transform.position).normalized * aimAssistForce / currentDistance).normalized * mag;
        }
    }
}
