using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienJump : MonoBehaviour
{
    [SerializeField] private Animator anim = null;  //🐌
    private float jumpTime = 4.0f;
    private bool celebrating = false;

    private void Start()
    {
        //anim.GetComponent<Animator>();  //bad line 🐌🐌
        anim.SetFloat("offset", Random.Range(0.0f, 2.0f));
        StartCoroutine(WaitTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Celebrate();
        }

    }

    private void Celebrate() 
    {
        celebrating = true;
        anim.SetTrigger("isCelebrating");
    }

    private void OnEnable()
    {
        IngGameController.OnServedCombo += Celebrate;
    }

    private void OnDisable()
    {
        IngGameController.OnServedCombo -= Celebrate;
    }

    private void Jump() 
    {
        if (!celebrating)
        {
            anim.SetTrigger($"Cheer{Random.Range(1, 4)}");
            //anim.SetTrigger("Cheer1");
        }
    }

    private IEnumerator WaitTime() 
    {
        jumpTime = Random.Range(4.0f, 5.0f);
        float waitTime = Random.Range(0.5f, 4.0f);
        yield return new WaitForSeconds(waitTime);
        InvokeRepeating("Jump", 3, jumpTime);
    }

    public void CelebrationEnd() 
    {
        celebrating = false;
        //Debug.Log("it worked : D 🐌");
    }
}
