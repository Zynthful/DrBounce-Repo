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
    private float smoothMultiplier = 1.0f;

    private bool finished = false;
    private bool loadingDest = false;
    private float loadProgress = 0.0f;

    public enum ContinueOptions
    {
        Automatic,
        RequireInput,
    }

    [Header("Declarations")]
    [SerializeField]
    private string loadingScreenSceneName = null;

    [Header("Loading Events")]
    [SerializeField]
    private GameEvent onLoadLoadingScreenStart = null;
    [SerializeField]
    private GameEvent onLoadLevelStart = null;
    [SerializeField]
    private GameEvent onLoadLevelComplete = null;
    [SerializeField]
    private GameEventFloat onLoadProgress = null;
    [SerializeField]
    private GameEventFloat onLoadProgressSmoothed = null;   // A smoothed version of the load progress, useful for progress bars
    [SerializeField]
    private GameEvent onContinue = null;
    [SerializeField]
    private GameEvent onUnloadLoadingScreenComplete = null;


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

    public void LoadScene(string _destination, ContinueOptions _continueOptions = ContinueOptions.Automatic, float _smoothMultiplier = 1.0f, float delay = 0.0f)
    {
        finished = false;

        destination = _destination;
        continueOptions = _continueOptions;
        smoothMultiplier = _smoothMultiplier;

        onLoadLoadingScreenStart?.Raise();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadingScreenSceneName, LoadSceneMode.Additive);     // Load loading screen
        operation.completed += _ =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScreenSceneName));                       // Set loading screen as our active scene
            SceneManager.UnloadSceneAsync(currentSceneIndex, UnloadSceneOptions.None);                              // Unload active scene when we've finished loading the loading screen

            StartCoroutine(DelayLoad(delay));
        };
    }

    private IEnumerator DelayLoad(float duration)
    {
        yield return new WaitForSeconds(duration);

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
        if (!finished)  // had to add this in because Continue kept getting called despite stopping listening for input??
        {
            onContinue?.Raise();

            // Stop listening for continue input
            InputManager.inputMaster.Menu.Continue.performed -= _ => Continue();
            InputManager.inputMaster.Menu.Continue.Disable();

            // Activate loaded scene
            destinationOperation.allowSceneActivation = true;

            // Wait for loaded scene to be active before unloading loading screen
            destinationOperation.completed += _ =>
            {
                destinationOperation = null;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(destination));

                // Unload loading screen
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(loadingScreenSceneName, UnloadSceneOptions.None);
                unloadOp.completed += _ =>
                {
                    onUnloadLoadingScreenComplete?.Raise();
                };
                finished = true;
            };
        }
    }
}
