using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Level scriptable objects")]
    private LevelData[] levels = null;
    
    [SerializeField]
    [Tooltip("The prefab which is instantiated into the grid")]
    private GameObject levelPrefab = null;

    [SerializeField]
    private SelectableGridNavigation grid = null;

    [SerializeField]
    [Tooltip("This is overriden by the first level button, however this should be set to the Return button by default if there are no levels.")]
    private Button defaultSelectedButton = null;

    [Header("Events")]
    [SerializeField]
    private UnityEvent<string> onSelectLevelToLoad = null;

    private void Awake()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            // this is cringe because we're instantiating the levels at runtime
            // todo: either remove this or make an editor script for this
            GameObject level = Instantiate(levelPrefab, transform);
            level.name = $"LVL_{levels[i].GetLevelName()}";

            // for some reason i had to make a variable for this???? why???
            string sceneName = levels[i].GetSceneName();
            Button button = level.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => SelectLevel(sceneName));

            if (i == 0)
            {
                defaultSelectedButton = button;
            }

            // this doesn't work for some reason
            // level.GetComponentInChildren<Button>().onClick.AddListener(() => SceneManager.LoadScene(levels[i].GetIndex()));

            // Add selectable to grid navigation to handle finding selectable navigation
            grid.AddSelectable(button);

            level.GetComponentInChildren<TextMeshProUGUI>().text = levels[i].GetLevelName();
            level.GetComponentInChildren<Image>().sprite = levels[i].GetSprite();
        }
    }

    public void SelectLevel(string level)
    {
        onSelectLevelToLoad.Invoke(level);
    }

    public void SelectDefaultButton()
    {
        defaultSelectedButton.Select();
    }
}
