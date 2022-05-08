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
            if(doneFadingIn == true)
            {
                doneFadingIn = false;
                StartCoroutine(FadeOut(pages[pageNo].GetComponent<Image>()));
            }

        }
    }

    private IEnumerator FadeOut(Image image)
    {
        if (pages.Count == pageNo + 1)
        {
            levelsData.LoadLevel(0);
            yield break;
        }

        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime * fadeRate));
            yield return null;
        }
        if (image.color.a <= 0.0f)
        {
            image.gameObject.SetActive(false);
            pageNo += 1;
            StartCoroutine(FadeIn(pages[pageNo].GetComponent<Image>()));
        }
    }

    private IEnumerator FadeIn(Image image)
    {
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
    }
}