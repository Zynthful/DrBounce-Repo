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
    private DecalManager decalM;

    [HideInInspector]
    public bool paused = false;

    [Header("Open URL on Quit")]
    [Space(15)]

    [SerializeField] bool openUrlOnQuit;
    [SerializeField][Tooltip("The url to be opened when the game is quit")] string urlToOpen;

    public void SetPaused(bool value)
    {
        paused = value;
    }

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(GameManager)) as GameManager;
        }

        if(s_Instance == null)
        {
            s_Instance = this;
        }
        else if(s_Instance != this)
        {
            Destroy(gameObject);
        }

        controls = InputManager.inputMaster;
        decalM = DecalManager.Instance;

        // Cap fps to 120
        Application.targetFrameRate = 120;

        DontDestroyOnLoad(s_Instance);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        controls.Debug.DEBUG_Pause.performed += _ => Stop();
    }

    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Debug.DEBUG_Pause.performed -= _ => Stop();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        decalM.ClearDecals();
    }

    public void Stop() 
    {
        Debug.Break();
    }

#if !UNITY_EDITOR
    private void OnApplicationQuit()
    {
        if (openUrlOnQuit)
        {
            Application.OpenURL(urlToOpen);
        }
    }
#endif
}
