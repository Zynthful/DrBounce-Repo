using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager s_Instance = null;

    AsyncOperation destinationOperation = null;

    private string destination = null;
    private ContinueOptions continueOptions = ContinueOptions.Automatic;
    private UnloadOptions unloadPrevOptions = UnloadOptions.Automatic;
    private UnloadOptions unloadLoadScreenOptions = UnloadOptions.Automatic;
    private float smoothMultiplier = 1.0f;

    private bool continued = false;
    private bool loadingDest = false;
    private float loadProgress = 0.0f;

    private int prevSceneIndex = 0;

    public enum ContinueOptions
    {
        Automatic,
        Manual,
        RequireInput,
    }

    public enum UnloadOptions
    {
        Automatic,
        Manual,
    }

    [Header("Declarations")]
    [SerializeField]
    private string loadingScreenSceneName = null;

    [Header("Loading Events")]
    public GameEvent onLoadLoadingScreenStart = null;
    public GameEvent onLoadLevelStart = null;
    public GameEvent onLoadLevelComplete = null;
    public GameEventFloat onLoadProgress = null;
    public GameEventFloat onLoadProgressSmoothed = null;   // A smoothed version of the load progress, useful for progress bars
    public GameEvent onContinue = null;
    public GameEvent onUnloadLoadingScreenComplete = null;
    public GameEvent onDestinationSceneActivated = null;


    private void Awake()
    {
        if (Application.isPlaying)
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(LoadingScreenManager)) as LoadingScreenManager;
            }
            if (s_Instance == null)
            {
                s_Instance = this;
            }
            else if (s_Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (loadingDest)
        {
            float target = destinationOperation.progress / 0.9f;
            loadProgress = Mathf.MoveTowards(loadProgress, target, smoothMultiplier * Time.deltaTime);  // cry about it tom
            onLoadProgressSmoothed?.Raise(loadProgress);

            if (loadProgress >= 1)
            {
                OnLoadCompleted();
            }
        }
    }

    /// <summary>
    /// Loads a scene via a loading screen.
    /// </summary>
    /// <param name="_destination">The destination scene name to load.</param>
    /// <param name="_continueOptions">Additional parameter to determine when to continue.</param>
    /// <param name="_unloadPrevOptions">Additional parameter to determine when to unload the previous (starting) scene.</param>
    /// <param name="_unloadLoadScreenOptions">Additional parameter to determine when to unload the loading screen scene.</param>
    /// <param name="_smoothMultiplier">Additional parameter that affects smoothing of load progress (useful for progress bars).</param>
    public void LoadScene(string _destination, ContinueOptions _continueOptions = ContinueOptions.Automatic, UnloadOptions _unloadPrevOptions = UnloadOptions.Automatic, UnloadOptions _unloadLoadScreenOptions = UnloadOptions.Automatic, float _smoothMultiplier = 1.0f)
    {
        continued = false;

        destination = _destination;
        continueOptions = _continueOptions;
        unloadPrevOptions = _unloadPrevOptions;
        unloadLoadScreenOptions = _unloadLoadScreenOptions;
        smoothMultiplier = _smoothMultiplier;
        prevSceneIndex = SceneManager.GetActiveScene().buildIndex;

        onLoadLoadingScreenStart?.Raise();
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadingScreenSceneName, LoadSceneMode.Additive);     // Load loading screen

        operation.completed += _ =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScreenSceneName));                       // Set loading screen as our active scene

            switch (unloadPrevOptions)
            {
                case UnloadOptions.Automatic:
                    UnloadPrevScene();
                    break;
                case UnloadOptions.Manual:
                    break;
                default:
                    UnloadPrevScene();
                    break;
            }
        };
    }

    public void UnloadPrevScene()
    {
        SceneManager.UnloadSceneAsync(prevSceneIndex, UnloadSceneOptions.None);
        LoadDestination();
    }

    public void LoadDestination()
    {
        loadingDest = true;
        onLoadLevelStart?.Raise();
        destinationOperation = SceneManager.LoadSceneAsync(destination, LoadSceneMode.Additive);                // Load destination scene
        destinationOperation.allowSceneActivation = false;                                                      // Don't activate our destination scene immediately

        // Update progress
        // As allowSceneActivation is false, operation progress caps at 0.9, so we wait until it's reached it before allowing continuing
        while (destinationOperation.progress < 0.89f)
        {
            onLoadProgress?.Raise(destinationOperation.progress);
        }
    }

    private void OnLoadCompleted()
    {
        loadingDest = false;
        onLoadProgress?.Raise(1.0f);
        onLoadLevelComplete?.Raise();

        switch (continueOptions)
        {
            case ContinueOptions.Automatic:
                Continue();
                break;
            case ContinueOptions.RequireInput:
                InputManager.inputMaster.Menu.Continue.performed += _ => Continue();
                InputManager.inputMaster.Menu.Continue.Enable();
                break;
            default:
                Continue();
                break;
        }
    }

    private void Continue()
    {
        if (!continued)  // had to add this in because Continue kept getting called despite stopping listening for input??
        {
            continued = true;

            onContinue?.Raise();

            // Stop listening for continue input
            InputManager.inputMaster.Menu.Continue.performed -= _ => Continue();
            InputManager.inputMaster.Menu.Continue.Disable();

            // Activate loaded scene
            destinationOperation.allowSceneActivation = true;

            // Wait for loaded scene to be active before unloading loading screen
            destinationOperation.completed += _ =>
            {
                onDestinationSceneActivated?.Raise();
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(destination));

                switch (unloadLoadScreenOptions)
                {
                    case UnloadOptions.Automatic:
                        UnloadLoadScreen();
                        break;
                    case UnloadOptions.Manual:
                        break;
                    default:
                        UnloadLoadScreen();
                        break;
                }
            };
        }
    }

    public void UnloadLoadScreen()
    {
        // Unload loading screen
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(loadingScreenSceneName, UnloadSceneOptions.None);

        unloadOp.completed += _ =>
        {
            onUnloadLoadingScreenComplete?.Raise();
        };

        destinationOperation = null;
    }
}
