using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAnimControls : MonoBehaviour
{
    [SerializeField] Animator animator;
    public bool killAnim;

    void Start()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(killAnim)
        {
            KillAnimator();
        }
    }

    public void KillAnimator()
    {
        Destroy(animator);
    }
}
