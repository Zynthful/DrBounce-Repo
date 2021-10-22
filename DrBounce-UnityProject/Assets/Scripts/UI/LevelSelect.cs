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

    [SerializeField]
    private SelectableGridNavigation grid = null;

    [SerializeField]
    [Tooltip("This is overriden by the first level button, however this should be set to the Return button by default if there are no levels.")]
    private Button defaultSelectedButton = null;

    private void Awake()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = Instantiate(levelPrefab, transform);
            level.name = $"LVL_{levels[i].GetName()}";

            // for some reason i had to make a variable for this???? why???
            int index = levels[i].GetIndex();
            Button button = level.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => SceneManager.LoadScene(index));

            if (i == 0)
            {
                defaultSelectedButton = button;
            }

            // this doesn't work for some reason
            // level.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene(levels[i].GetIndex()));

            // Add selectable to grid navigation to handle finding selectable navigation
            grid.AddSelectable(button);

            level.GetComponentInChildren<TextMeshProUGUI>().text = levels[i].GetName();
            level.GetComponentInChildren<Image>().sprite = levels[i].GetSprite();
        }
    }

    private void OnEnable()
    {
        defaultSelectedButton.Select();
    }
}
