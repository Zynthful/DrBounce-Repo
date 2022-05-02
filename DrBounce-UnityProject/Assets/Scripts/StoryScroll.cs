using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StoryScroll : MonoBehaviour
{
    private List<GameObject> pages = new List<GameObject>();
    public InputMaster controls;
    private int pageNo = 0;
    private Scene sceneToLoad;
    // Start is called before the first frame update

    void Awake()
    {
        controls = new InputMaster();
        controls = InputManager.inputMaster;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pages.Add(gameObject.transform.GetChild(i).gameObject);
            if (i >= 1)
            {
                pages[i].SetActive(false);
            }
        }
    }

    public void OnNextPage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            print("Input!");
            pages[pageNo].SetActive(false);
            pageNo += 1;
            if (pages[pageNo] == null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                pages[pageNo].SetActive(true);
            }
        }
    }
}