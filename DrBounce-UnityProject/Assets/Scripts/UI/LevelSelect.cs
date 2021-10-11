using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Level scriptable objects")]
    private LevelObj[] levels = null;
    
    [SerializeField]
    [Tooltip("The prefab which is instantiated into the grid")]
    private GameObject levelPrefab = null;

    private void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = Instantiate(levelPrefab, transform);
            level.name = $"LVL_{levels[i].GetName()}";

            // for some reason i had to make a variable for this???? why???
            int index = levels[i].GetIndex();
            level.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene(index));

            // this doesn't work for some reason
            // level.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene(levels[i].GetIndex()));

            level.GetComponentInChildren<TextMeshProUGUI>().text = levels[i].GetName();
            level.GetComponentInChildren<Image>().sprite = levels[i].GetSprite();
        }
    }

    private void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
