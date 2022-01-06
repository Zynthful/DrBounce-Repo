using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUG_SwapScene : MonoBehaviour
{
    private int currentSceneIndex;
    public InputMaster controls;

    private void Awake()
    {
        controls = InputManager.inputMaster;
    }

    private void OnEnable()
    {
        controls.Player.DEBUG_PrevLevel.performed += _ => BackLevel();
        controls.Player.DEBUG_NextLevel.performed += _ => NextLevel();
        controls.Player.DEBUG_ReloadScene.performed += _ => ReloadScene();
    }

    private void OnDisable()
    {
        controls.Player.DEBUG_PrevLevel.performed -= _ => BackLevel();
        controls.Player.DEBUG_NextLevel.performed -= _ => NextLevel();
        controls.Player.DEBUG_ReloadScene.performed -= _ => ReloadScene();
    }

    private void ReloadScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void BackLevel()
    {
        if (currentSceneIndex > 0)
        {
            currentSceneIndex--;
        }
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Single);
    }

    void NextLevel()
    {
        if(currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            currentSceneIndex++;
        }
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Single);
    }
}
