using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager s_Instance = null;

    AsyncOperation destinationOperation = null;
    private string destination = null;
    private bool finished = false;

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

    public void LoadScene(string _destination, bool requireContinueInput)
    {
        finished = false;
        destination = _destination;

        onLoadLoadingScreenStart?.Raise();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadingScreenSceneName, LoadSceneMode.Additive);     // Load loading screen
        operation.completed += _ =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScreenSceneName));                       // Set loading screen as our active scene
            SceneManager.UnloadSceneAsync(currentSceneIndex, UnloadSceneOptions.None);                              // Unload active scene when we've finished loading the loading screen

            onLoadLevelStart?.Raise();
            destinationOperation = SceneManager.LoadSceneAsync(destination, LoadSceneMode.Additive);                // Load destination scene
            destinationOperation.allowSceneActivation = false;                                                      // Don't activate our destination scene immediately

            // Update progress
            // As allowSceneActivation is false, operation progress caps at 0.9, so we wait until it's reached it before allowing continuing
            while (destinationOperation.progress < 0.89f)
            {
                onLoadProgress?.Raise(destinationOperation.progress);
            }

            // Load level completed
            onLoadProgress?.Raise(1.0f);
            onLoadLevelComplete?.Raise();

            // Automatically continue if we don't require input
            if (!requireContinueInput)
            {
                Continue();
            }
            else
            {
                // Listen for continue input
                InputManager.inputMaster.Menu.Continue.performed += _ => Continue();
                InputManager.inputMaster.Menu.Continue.Enable();
            }
        };
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
