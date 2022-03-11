using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSTracker : MonoBehaviour
{
    int fps = 0;

    public static FPSTracker s_Instance = null;

    [SerializeField] private TMP_Text colour;
    [SerializeField] private TMP_Text fpsCounter;

    [SerializeField] private Color badFPSColor = new Color();
    [SerializeField] private Color okFPSColor = new Color();
    [SerializeField] private Color goodFPSColor = new Color();
    [SerializeField] private Color bestFPSColor = new Color();

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
        fps = (int)(1f / Time.unscaledDeltaTime);

        if (fps >= 120)
        {
            colour.color = bestFPSColor;
            colour.text = "Golden";

            fpsCounter.text = "FPS: " + fps;
        }
        if (fps >= 60)
        {
            colour.color = goodFPSColor;
            colour.text = "Green";

            fpsCounter.text = "FPS: " + fps;
        }
        else if (fps >= 30)
        {
            colour.color = okFPSColor;
            colour.text = "Amber";

            fpsCounter.text = "FPS: " + fps;
        }
        else 
        {
            colour.color = badFPSColor;
            colour.text = "Red";

            fpsCounter.text = "FPS: " + fps;
        }
    }
}
