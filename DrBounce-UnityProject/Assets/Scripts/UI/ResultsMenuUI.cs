using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultsMenuUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentTimeText = null;
    [SerializeField]
    private TextMeshProUGUI pbText = null;

    private void OnEnable()
    {
        //currentTimeText.text = Timer.LevelEndTime().ToString();

        // get pb from save system?
        //pbText.text = SaveSystem.
    }
}
