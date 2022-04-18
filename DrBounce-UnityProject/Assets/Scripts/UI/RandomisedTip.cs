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
    private float tipDelay = 8.0f;

    private List<Sprite> unusedTips = null;

    private void OnEnable()
    {
        SelectNew();
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
        else if (unusedTips.Count <= 0)
        {
            ResetTips();
            SelectNew();
            return;
        }

        int spriteIndex = Random.Range(0, unusedTips.Count - 1);
        image.sprite = unusedTips[spriteIndex];
        unusedTips.RemoveAt(spriteIndex);

        StartCoroutine(TipDelay());
    }

    private void ResetTips()
    {
        unusedTips.Clear();
        tips.CopyTo(unusedTips.ToArray(), 0);
    }

    private IEnumerator TipDelay()
    {
        yield return new WaitForSecondsRealtime(tipDelay);
        SelectNew();
    }
}
