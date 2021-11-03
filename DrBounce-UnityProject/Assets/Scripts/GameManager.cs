using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance = null;

    /// <summary>
    /// gravity here is just set to whatever the PlayerMovement has it set to at the start of runtime, don't try to update this gravity value
    /// </summary>
    public static float gravity { get; set; }

    public LayerMask bounceableLayers = 9; // Set this to the layer mask of any bounceable terrain/enemies
    private InputMaster controls;

    [HideInInspector]
    public bool paused = false;

    public void SetPaused(bool value)
    {
        paused = value;
    }

    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.DEBUG_Pause.performed += _ => Stop();
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void Stop() 
    {
        Debug.Break();
    }

}
