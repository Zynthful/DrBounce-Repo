using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomisedTip : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    private Image image = null;

    [Header("Tip Settings")]
    [SerializeField]
    private Sprite[] tips = null;
    [SerializeField]
    private float initialDelay = 2.0f;
    [SerializeField]
    private float tipDelay = 8.0f;

    private List<Sprite> unusedTips = new List<Sprite>();

    private void OnEnable()
    {
        image.enabled = false;
        StartCoroutine(TipDelay(initialDelay));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SelectNew()
    {
        if (tips == null)
        {
            Debug.LogError("No tip sprites set.", gameObject);
            return;
        }
        else if (image == null)
        {
            Debug.LogError("No Image set.", gameObject);
            return;
        }
        else if (unusedTips.Count <= 0 || unusedTips == null)
        {
            ResetTips();
            SelectNew();
            return;
        }

        int spriteIndex = Random.Range(0, unusedTips.Count - 1);
        image.enabled = true;
        image.sprite = unusedTips[spriteIndex];
        image.preserveAspect = true;
        unusedTips.RemoveAt(spriteIndex);

        StartCoroutine(TipDelay(tipDelay));
    }

    private void ResetTips()
    {
        unusedTips = new List<Sprite>(tips);
    }

    private IEnumerator TipDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SelectNew();
    }
}