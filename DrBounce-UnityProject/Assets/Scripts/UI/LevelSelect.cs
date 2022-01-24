using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[ExecuteInEditMode]
public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Level scriptable objects")]
    private LevelData[] levels = null;

    private List<GameObject> levelObjects = new List<GameObject>();
    
    [SerializeField]
    [Tooltip("The prefab which is instantiated into the grid")]
    private GameObject levelPrefab = null;

    [SerializeField]
    private SelectableGridNavigation grid = null;

    [SerializeField]
    [Tooltip("This is overriden by the first level button, however this should be set to the Return button by default if there are no levels.")]
    private Button defaultSelectedButton = null;

    private Button initialSelectedButton = null;

    [Header("Events")]
    [SerializeField]
    private UnityEvent<string> onSelectLevelToLoad = null;

    private void OnValidate()
    {
        /*
        #if UNITY_EDITOR
        RegenerateLevels();
        #endif
        */
    }

    private void Awake()
    {
        #if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            RegenerateLevels();
        }
        #else
        RegenerateLevels();
        #endif
    }


    /// <summary>
    /// Clears all current levels, then generates them again.
    /// </summary>
    public void RegenerateLevels()
    {
        ClearLevels();
        GenerateLevels();
        initialSelectedButton = defaultSelectedButton;
    }

    /// <summary>
    /// Clears all level objects and destroys all children of this object.
    /// </summary>
    private void ClearLevels()
    {
        /*
        for (int i = 0; i < levelObjects.Count; i++)
        {
            #if UNITY_EDITOR
            DestroyImmediate(levelObjects[i]);
            #else
            Destroy(levelObjects[i];
            #endif
        }
        */

        GameObject[] children = new GameObject[transform.childCount];
        int i = 0;

        // Find all child objects before destroying them
        foreach (Transform child in transform)
        {
            children[i] = child.gameObject;
            i++;
        }

        // Destroy the child.
        // https://youtu.be/EQ8jy7jQ3yY
        foreach (GameObject obj in children)
        {
            #if UNITY_EDITOR
            // Use relative Destroy method based on whether we're in playmode or not
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Destroy(obj.gameObject);
            }
            else
            {
                DestroyImmediate(obj.gameObject);
            }
            #else
            Destroy(obj);
            #endif
        }

        levelObjects.Clear();
        grid.ClearInteractables();
    }

    /// <summary>
    /// Generates level objects from the level data. When the level's button is clicked, it selects the level.
    /// </summary>
    private void GenerateLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = Instantiate(levelPrefab, transform);
            levelObjects.Add(level);
            level.name = $"{levels[i].GetLevelName()}";

            // for some reason i had to make a variable for this???? why???
            string sceneName = levels[i].GetSceneName();
            Button button = level.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => SelectLevel(sceneName));

            if (i == 0)
            {
                initialSelectedButton = button;
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
