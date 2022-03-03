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

    private int selectedLevel = 0;

    private List<GameObject> levelObjects = new List<GameObject>();
    
    [SerializeField]
    [Tooltip("The prefab which is instantiated into the grid")]
    private GameObject levelPrefab = null;

    [SerializeField]
    [Tooltip("This is overriden by the first level button, however this should be set to the Return button by default if there are no levels.")]
    private Button defaultSelectedButton = null;

    [Header("Events")]
    public UnityEvent<string> onSelectLevelToLoad = null;

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
    }

    /// <summary>
    /// Clears all level objects and destroys all children of this object.
    /// </summary>
    private void ClearLevels()
    {
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
    }

    /// <summary>
    /// Generates level objects from the level data.
    /// </summary>
    private void GenerateLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            GameObject level = Instantiate(levelPrefab, transform);
            levelObjects.Add(level);
            level.name = $"{levels[i].GetLevelName()}";

            LevelTemplateUI ui = level.GetComponent<LevelTemplateUI>();
            ui.SetName(levels[i].GetLevelName());
            ui.SetDescription(levels[i].GetDescription());
            ui.SetPreviewSprite(levels[i].GetSprite());

            level.SetActive(false);

            //todo: set pb time from save system
        }

        SelectLevel(0);
    }

    public void SelectLevel(int levelIndex)
    {
        levelObjects[selectedLevel].SetActive(false);
        levelObjects[levelIndex].SetActive(true);

        selectedLevel = levelIndex;

        onSelectLevelToLoad.Invoke(levels[levelIndex].GetSceneName());
    }

    public void SelectDefaultButton()
    {
        defaultSelectedButton.Select();
    }

    public void NextLevel()
    {
        if (selectedLevel == levels.Length - 1)
        {
            SelectLevel(0);
        }
        else
        {
            SelectLevel(selectedLevel + 1);
        }
    }

    public void PreviousLevel()
    {
        if (selectedLevel == 0)
        {
            SelectLevel(levels.Length - 1);
        }
        else
        {
            SelectLevel(selectedLevel - 1);
        }
    }
}
