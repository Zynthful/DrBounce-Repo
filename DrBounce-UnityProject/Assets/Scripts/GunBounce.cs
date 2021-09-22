using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBounce : MonoBehaviour
{
    [SerializeField] bool returning; 
    [SerializeField] float forceMod;
    [SerializeField] bool canThrow;
    [SerializeField] string[] bounceableTags;
    Vector3 handPosition;
    Camera cam;
    Vector3 originPoint;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        handPosition = transform.localPosition;
        canThrow = true;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = cam.transform.parent;
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canThrow)
        {
            canThrow = false;
            Thrown(transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            ResetScript();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = cam.transform.rotation;
        if (returning)
        {
            returning = false;
            Vector3 dir = (originPoint - transform.position).normalized;
            rb.velocity = new Vector3(dir.x, .3f, dir.z) * forceMod; 
        }
    }

    public void Thrown(Vector3 position)
    {
        returning = false;
        rb.constraints = RigidbodyConstraints.None;
        transform.parent = null;
        originPoint = position;
        Vector3 dir = transform.forward;
        rb.velocity = new Vector3(dir.x, dir.y + .1f, dir.z) * forceMod;
    }

    void ResetScript()
    {
        returning = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = cam.transform.parent;
        transform.localPosition = handPosition;
        transform.rotation = cam.transform.rotation;
        canThrow = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(string tag in bounceableTags)
        {
            if (collision.transform.CompareTag(tag))
            {
                        Debug.Log("Collided");

                rb.velocity = Vector3.zero;
                returning = true;
            }
        }

        if (collision.transform.CompareTag("Player"))
        {
            ResetScript();
        }
    }
}
