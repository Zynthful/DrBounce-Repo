using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMagnet : MonoBehaviour
{
    [SerializeField] private GameObject magnet = null;
    [SerializeField] private Animator magnetAnim = null;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Reveal()
    {
        magnetAnim.SetTrigger("Reveal");

        //magnet.SetActive(true);
    }

    public void Hide()
    {
        magnetAnim.SetTrigger("Drop");

        //magnet.SetActive(false);
    }
}
