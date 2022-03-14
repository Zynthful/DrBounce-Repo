using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager s_Instance = null;

    AsyncOperation destinationOperation = null;

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

    public void LoadScene(string destination, bool requireContinueInput)
    {
        // Load loading screen and unload active scene
        onLoadLoadingScreenStart?.Raise();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        AsyncOperation operation = SceneManager.LoadSceneAsync(loadingScreenSceneName, LoadSceneMode.Additive);     // Load loading screen
        operation.completed += _ =>
        {
            SceneManager.UnloadSceneAsync(currentSceneIndex, UnloadSceneOptions.None);      // Unload active scene when we've finished loading the loading screen

            // Load destination scene
            onLoadLevelStart?.Raise();
            destinationOperation = SceneManager.LoadSceneAsync(destination, LoadSceneMode.Additive);
            destinationOperation.allowSceneActivation = false;

            // Update progress
            while (destinationOperation.progress < 0.89f)   // As allowSceneActivation is false, operation progress caps at 0.9, so we wait until it's reached it before allowing continuing
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
                InputManager.inputMaster.Menu.Continue.performed += _ => Continue();
                InputManager.inputMaster.Menu.Continue.Enable();
            }
        };
    }

    public void Continue()
    {
        InputManager.inputMaster.Menu.Continue.performed -= _ => Continue();
        InputManager.inputMaster.Menu.Continue.Disable();
        SceneManager.UnloadSceneAsync(loadingScreenSceneName, UnloadSceneOptions.None);
        destinationOperation.allowSceneActivation = true;
        destinationOperation = null;
    }
}
