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

    [SerializeField] private float fadeRate = 1;
    [SerializeField] private float sequenceFadeRate;
    private bool doneFadingIn = true;
    private bool waitComplete = false;

    [SerializeField] private bool isBook;
    [SerializeField] private bool isPage;
    [SerializeField] private float waitTime;
    private bool sequence = false;
    public bool allChildrenActive = false;
    private bool holding;
    private float time;

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

        if (isBook && pageNo == 0)
        {
            pages[pageNo].SetActive(true);
        }

        if (isPage && pageNo == 0)
        {
            StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>(), fadeRate));
            doneFadingIn = false;
        }
    }

    private void Update()
    {
        //While the button is being held down
        if(holding == true)
        {
            //Add to the time value
            time += Time.deltaTime;
        }

        //Once a second of holding down has occurred
        if (time >= 1 && isPage)
        {
            holding = false;
            //Activate all panels, prevent any more panels from activating and allow the book to move to the next page
            foreach (GameObject page in pages)
            {
                doneFadingIn = false;
                page.SetActive(true);
                allChildrenActive = true;
            }
        }
        if(sequence == true && doneFadingIn == true)
        {
            print("TestA");
            doneFadingIn = false;
            StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>(), sequenceFadeRate));
            print("TestB");
        }
    }

    public void OnNextPage(InputAction.CallbackContext context)
    {
        //Activates when any mapped button is initially pressed
        if (context.started)
        {
            //sets the holding value to true, allowing the time value in update to be raised - keeping track of how long the player has held the button down for in seconds
            holding = true;
        }

        //activates when said button is let go of
        if (context.canceled)
        {
            holding = false;
            time = 0;
        }

        if (context.performed && doneFadingIn == true && waitComplete == true)
        {
            //If the object running this code is the canvas rather than a single page, and the page currently active either has no panels or all panels have been activated
            if (isBook && (pages[pageNo].transform.childCount == 0 || pages[pageNo].GetComponent<StoryScroll>().allChildrenActive == true))
            {
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
                doneFadingIn = false;

                //If the number of pages total is equal to the number of pages active
                StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>(), fadeRate));
            }
        }
    }

    private IEnumerator FadeIn(Image image, float fadeRate)
    {
        //Sets the alpha value of the image to 0 - being invisible
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.gameObject.SetActive(true);

        //While the alpha of the image is lower than 1 - being fully visible
        while (image.color.a < 1.0f)
        {
            //raise the alpha value at a speed determined by the fadeRate value
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime * fadeRate));
            yield return null;
        }

        //If the alpha reaches 1
        if (image.color.a >= 1)
        {
            //Set doneFadingIn to true, which allows the next panel to start fading in
            doneFadingIn = true;
        }

        //Sets the next target of FadeIn to the next page by adding one to the pageNo value
        pageNo += 1;

        //If the total number of pages is equal to the current page number (plus one)
        if (pages.Count == pageNo)
        {
            //Allow the book canvas to go to the next page
            allChildrenActive = true;
        }

        if (image.gameObject.GetComponent<ComicSequence>() != null && image.gameObject.GetComponent<ComicSequence>().startOfSequence == true)
        {
            sequence = true;
        }
        if (image.gameObject.GetComponent<ComicSequence>() != null && image.gameObject.GetComponent<ComicSequence>().endOfSequence == true)
        {
            sequence = false;
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