using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienJump : MonoBehaviour
{
    [SerializeField] private Animator anim = null;  //🐌
    private bool celebrating = false;

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
        if(!celebrating)
        {
            celebrating = true;
            anim.SetTrigger("isCelebrating");
        }
    }

    public void CelebrationEnd() 
    {
        celebrating = false;
        //Debug.Log("it worked : D 🐌");
    }
}
