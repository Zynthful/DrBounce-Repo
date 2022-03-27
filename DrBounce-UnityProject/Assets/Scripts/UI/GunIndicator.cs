using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunIndicator : MonoBehaviour
{

    [SerializeField] private Transform gun = null;
    [SerializeField] private GameObject indicatorObj = null;
    [SerializeField] private Transform cam = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camForward = cam.forward;
        Vector3 gunPos = gun.position;

        float cDg = Vector3.Dot(camForward.normalized, gunPos.normalized);
        Debug.Log(cDg);

        //doesnt work rn:(
        //float indicatorOpacity = Mathf.Clamp(1f + (cDg - -1f) * (-1f - 1f) / (1f - -1f), 0f, 1f);

        float rot = Vector3.SignedAngle(gunPos - cam.position, camForward, Vector3.up);
        //Debug.Log(rot);
        indicatorObj.transform.localRotation = Quaternion.Euler(0f, 0f, rot);

    }
}
