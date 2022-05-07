using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager s_Instance = null;

    private static AsyncOperation destinationOperation = null;

    private static string destination = null;
    private static ContinueOptions continueOptions = ContinueOptions.Automatic;
    private static UnloadOptions unloadPrevOptions = UnloadOptions.Automatic;
    private static UnloadOptions unloadLoadScreenOptions = UnloadOptions.Automatic;
    private static float smoothMultiplier = 1.0f;

    private static bool loadingInProgress = false;  // Prevents multiple instances of loading
    private static bool continued = false;          // Prevents multiple instances of continuing
    private static bool loadingDest = false;        // Allows progress bar to update
    private static float loadProgress = 0.0f;

    private static int prevSceneIndex = 0;

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
    public string loadingScreenSceneName = null;

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
            loadProgress = Mathf.MoveTowards(loadProgress, target, smoothMultiplier * Time.unscaledDeltaTime);  // cry about it tom
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
    public void LoadScene(
            string _destination,
            ContinueOptions _continueOptions = ContinueOptions.RequireInput,
            UnloadOptions _unloadPrevOptions = UnloadOptions.Manual,
            UnloadOptions _unloadLoadScreenOptions = UnloadOptions.Manual,
            float _smoothMultiplier = 1.0f)
    {
        // Prevent loading whilst we're already performing a loading operation
        if (loadingInProgress)
            return;

        loadingInProgress = true;

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

    /// <summary>
    /// Unload all loaded scenes EXCEPT the loading screen scene. Then, load the destination scene.
    /// </summary>
    public void UnloadPrevScene()
    {
        Scene[] loadedScenes = SceneManagement.GetLoadedScenes();
        for (int i = 0; i < loadedScenes.Length; i++)
        {
            if (loadedScenes[i] != SceneManager.GetSceneByName(loadingScreenSceneName))
            {
                SceneManager.UnloadSceneAsync(loadedScenes[i], UnloadSceneOptions.None);
            }
        }
        PauseHandler.SetTimeFreeze(false);
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
        OnLoadCompleted();
    }

    private void OnLoadCompleted()
    {
        loadingDest = false;
        continued = false;
        onLoadProgress?.Raise(1.0f);
        onLoadProgressSmoothed?.Raise(1.0f);
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
        loadingInProgress = false;
    }
}
