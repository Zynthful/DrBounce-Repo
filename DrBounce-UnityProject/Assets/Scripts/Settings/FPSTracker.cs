using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSTracker : MonoBehaviour
{
    private int fps = 0;

    private int[] fpsList = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int currentItemInList = 0;
    private int fpsAverage = 0;


    public static FPSTracker s_Instance = null;

    [SerializeField] private TMP_Text textcolour;
    [SerializeField] private TMP_Text fpsCounter;
    [SerializeField] private TMP_Text fpsAverageCounter;

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
        //print(fpsList.Length);

        fps = (int)(1f / Time.unscaledDeltaTime);

        fpsList[currentItemInList] = fps;

        currentItemInList++;
        if (currentItemInList >= fpsList.Length)
        {
            currentItemInList = 0;
        }

        AverageCalc();

        if (fps >= 120)
        {
            ChangeColour(bestFPSColor, "Golden");
        }
        if (fps >= 60)
        {
            ChangeColour(goodFPSColor, "Green");
        }
        else if (fps >= 30)
        {
            ChangeColour(okFPSColor, "Amber");
        }
        else 
        {
            ChangeColour(badFPSColor, "Red");
        }
        fpsCounter.text = "FPS: " + fps;
        fpsAverageCounter.text = "Average FPS: " + fpsAverage;
    }

    private void ChangeColour(Color colour, string text) 
    {
        textcolour.color = colour;
        textcolour.text = text;
    } 

    private void AverageCalc() 
    {
        fpsAverage = 0;

        foreach (int fps in fpsList) 
        {
            fpsAverage = fpsAverage + fps;
        }

        fpsAverage = fpsAverage / fpsList.Length;
    }

}
