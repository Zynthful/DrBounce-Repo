using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HideMagnet : MonoBehaviour
{
    private Animator magnetAnim = null;

    private void Awake()
    {
        magnetAnim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Reveal()
    {
        magnetAnim.SetTrigger("Reveal");
    }

    public void Hide()
    {
        magnetAnim.SetTrigger("Drop");
    }
}
