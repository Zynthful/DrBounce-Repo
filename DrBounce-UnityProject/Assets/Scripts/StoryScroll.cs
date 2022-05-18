using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StoryScroll : MonoBehaviour
{
    [SerializeField]
    private LevelsData levelsData = null;

    private List<GameObject> pages = new List<GameObject>();
    public InputMaster controls;
    private int pageNo = 0;

    [SerializeField]
    private float fadeRate = 1;
    private bool doneFadingIn = true;
    private bool waitComplete = false;

    [SerializeField] private bool isBook;
    [SerializeField] private bool isPage;
    [SerializeField] private float waitTime;
    public bool allChildrenActive = false;

    void Awake()
    {
        controls = new InputMaster();
        controls = InputManager.inputMaster;

        //Creates a list of all the pages/panels childed and sets all of them to inactive - apart from the first.
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pages.Add(gameObject.transform.GetChild(i).gameObject);
            pages[i].SetActive(false);
        }

        //A small buffer once the scene has loaded so the player doesn't click and skip the first page or panel immediately

    }

    private void Start()
    {
        if (waitTime != 0)
        {
            StartCoroutine(WaitOnSceneLoad());
        }

        else
        {
            waitComplete = true;
        }

        if(isBook && pageNo == 0)
        {
            pages[pageNo].SetActive(true);
        }

        if (isPage && pageNo == 0)
        {
            StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>()));
        }
    }

    public void OnNextPage(InputAction.CallbackContext context)
    {
        print("testing 1 - " + gameObject.name);
        if (context.performed && doneFadingIn == true && waitComplete == true)
        {
            print("testing 2 - " + gameObject.name);
            //If the object running this code is the canvas rather than a single page, and the page currently active either has no panels or all panels have been activated
            if (isBook && (pages[pageNo].transform.childCount == 0 || pages[pageNo].GetComponent<StoryScroll>().allChildrenActive == true))
            {
                print("testing 6 - " + gameObject.name);
                if (pages.Count == pageNo + 1)
                {
                    levelsData.LoadLevel(0);
                }

                //Disable the current page and enable the next one
                pages[pageNo].SetActive(false);
                pageNo += 1;
                pages[pageNo].SetActive(true);
                //StartCoroutine(FadeOut(pages[pageNo].GetComponent<Image>()));
            }

            //If the object running this is a page within the book canvas
            if (isPage)
            {
                print("testing 3 - " + gameObject.name);
                doneFadingIn = false;

                //If the number of pages total is equal to the number of pages active
                StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>()));

                print("testing 4 - " + gameObject.name);
            }
        }
    }

    private IEnumerator FadeIn(Image image)
    {
        print("testing 5 - " + gameObject.name);

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.gameObject.SetActive(true);
        while (image.color.a < 1.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime * fadeRate));
            yield return null;
        }
        if (image.color.a >= 1)
        {
            doneFadingIn = true;
        }
        pageNo += 1;
        if (pages.Count == pageNo)
        {
            //Allow the book canvas to go to the next page
            allChildrenActive = true;
        }
    }

    private IEnumerator WaitOnSceneLoad()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        waitComplete = true;
    }

    //private IEnumerator FadeOut(Image image)
    //{
    //    if (pages.Count == pageNo + 1)
    //    {
    //        levelsData.LoadLevel(0);
    //        yield break;
    //    }

    //    while (image.color.a > 0.0f)
    //    {
    //        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime * fadeRate));
    //        yield return null;
    //    }
    //    if (image.color.a <= 0.0f)
    //    {
    //        image.gameObject.SetActive(false);
    //        pageNo += 1;

    //        StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>()));
    //    }
    //}
}