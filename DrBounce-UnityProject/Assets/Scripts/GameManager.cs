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

    public UnlockTracker.UnlockTypes[] currentSettings;

    [HideInInspector]
    public bool triggerThatThing = false;

    public static Player player = null;

    [SerializeField]
    private LevelsData levelsData = null;

    [HideInInspector]
    public bool paused = false;

    [Header("Open URL on Quit")]
    [Space(15)]

    [SerializeField] bool openUrlOnQuit;
    [SerializeField][Tooltip("The url to be opened when the game is quit")] string urlToOpen;

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

        Time.fixedDeltaTime = 0.02f;

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
        //Time.timeScale = 1.0f;
    }

    public static void SetCursorEnabled(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Stop() 
    {
        Debug.Break();
    }

    public void NewgameStart()
    {
        SaveSystem.DeleteGameData();
        LoadingScreenManager.s_Instance.LoadScene("ComicBookIntro");
    }

    public void Continue()
    {
        GameSaveData data = SaveSystem.LoadGameData();
        Debug.Log("Level Unlocked: " + data.levelUnlocked);
        levelsData.LoadLevel(data.levelUnlocked);
    }

    public void SetTriggerThatThing()
    {
        triggerThatThing = true;
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
