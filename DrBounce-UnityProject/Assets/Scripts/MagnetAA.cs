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
    [SerializeField] GameEventBool assistEvent;
    bool assistActive;
    Rigidbody rb;

    [Header("Vibrations")]
    public VibrationManager vibrationManager;

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
            Debug.Log("mag on");
            assistActive = true; assistEvent.Raise(true);
            vibrationManager.ActiveMagnetAssist();

        }
    }
    void AssistEnded() 
    {
        if (assistActive)
        {
            Debug.Log("mag off");
            vibrationManager.StopMagnet();
            assistActive = false; assistEvent.Raise(false);
            
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
        float currentDistance = Vector3.Distance(transform.position, gun.transform.position);
        if (!gun.transform.parent && currentDistance < aimAssistMaxRange)
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
