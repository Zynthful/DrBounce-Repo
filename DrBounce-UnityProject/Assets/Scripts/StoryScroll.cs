using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryScroll : MonoBehaviour
{
    private List<GameObject> pages = new List<GameObject>();
    public InputMaster controls;
    private int pageNo = 0;
    [SerializeField]
    private Scene sceneToLoad;
    // Start is called before the first frame update

    void Awake()
    {
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

    private void OnEnable()
    {
        controls = new InputMaster();
        controls.Menu.Continue.performed += _ => Continue();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Continue()
    {
        print("Input!");
        pages[pageNo].SetActive(false);
        pageNo += 1;
        if (pages[pageNo] == null)
        {
            SceneManager.SetActiveScene(sceneToLoad);
        }
        else
        {
            pages[pageNo].SetActive(true);
        }
    }
}