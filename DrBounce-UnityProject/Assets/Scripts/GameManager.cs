using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance = null;
    public static LayerMask bounceableLayers = 8; // Set this to the layer mask of any bounceable terrain/enemies

    [HideInInspector]
    public bool paused = false;

    public void SetPaused(bool value)
    {
        paused = value;
    }

    private void Awake()
    {
        // Cap fps to 120
        Application.targetFrameRate = 120;

        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
        }

        if (s_Instance == null)
        {
            var obj = new GameObject("GameManager");
            s_Instance = obj.AddComponent<GameManager>();
        }

        DontDestroyOnLoad(s_Instance);
    }
}
