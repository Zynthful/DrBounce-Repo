using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSTracker : MonoBehaviour
{
    int fps = 0;

    public static FPSTracker s_Instance = null;

    [SerializeField] private TMP_Text m_TextComponent;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("OutputTime", 0.5f, 0.5f);  //1s delay, repeat every 1s
    }

    // Update is called once per frame
    void Update()
    {
        //m_TextComponent.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);
    }

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(FPSTracker)) as FPSTracker;
        }
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


    void OutputTime()
    {
        m_TextComponent.text = "FPS: " + (int)(1f / Time.unscaledDeltaTime);
    }
}
