using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsMenuUI : MonoBehaviour
{
    [SerializeField]
    private LevelsData levelsData;

    [SerializeField]
    private TextMeshProUGUI currentTimeText = null;
    [SerializeField]
    private TextMeshProUGUI pbText = null;

    [SerializeField]
    private GameObject resultsObj;
    [SerializeField]
    private GameObject afterContinueObj;
    [SerializeField]
    private Selectable afterContinueDefaultSelected;

    private void OnEnable()
    {
        GameSaveData data = SaveSystem.LoadGameData();
        if (data != null)
        {
            currentTimeText.text = TimeConversion.ToTime(data.lastLevelTimes[levelsData.GetCurrentLevelIndex()]);
            pbText.text = TimeConversion.ToTime(data.levelPBTimes[levelsData.GetCurrentLevelIndex()]);
        }
    }

    public void ShowButtons()
    {
        StartCoroutine(DelayShowButtons());
    }

    private IEnumerator DelayShowButtons()
    {
        resultsObj.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        afterContinueObj.SetActive(true);
        afterContinueDefaultSelected.Select();
    }
}