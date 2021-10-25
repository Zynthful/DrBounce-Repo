using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMagnet : MonoBehaviour
{
    [SerializeField] private GameObject magnet = null;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Reveal()
    {
        magnet.SetActive(true);
    }

    public void Hide()
    {
        magnet.SetActive(false);
    }
}
