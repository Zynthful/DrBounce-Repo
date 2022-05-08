using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

[ExecuteInEditMode]
public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Level scriptable object")]
    private LevelsData levelsData = null;

    private int selectedLevel = 0;

    private List<LevelTemplateUI> levelObjects = new List<LevelTemplateUI>();
    
    [SerializeField]
    [Tooltip("The prefab which is instantiated into the grid")]
    private GameObject levelPrefab = null;

    [SerializeField]
    private Button[] startButtons = null;

    [SerializeField]
    private BoolSetting unlockAllLevelsSetting = null;

    private void OnEnable()
    {
        #if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            RegenerateLevels();
            InputManager.inputMaster.Menu.Next.performed += OnNextPerformed;
            InputManager.inputMaster.Menu.Previous.performed += OnPreviousPerformed;
        }
#else
        RegenerateLevels();
        InputManager.inputMaster.Menu.Next.performed += OnNextPerformed;
        InputManager.inputMaster.Menu.Previous.performed += OnPreviousPerformed;
#endif
    }

    private void OnDisable()
    {
        InputManager.inputMaster.Menu.Next.performed -= OnNextPerformed;
        InputManager.inputMaster.Menu.Previous.performed -= OnPreviousPerformed;
    }

    private void OnNextPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            NextLevel();
        }
    }

    private void OnPreviousPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            PreviousLevel();
        }
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
    /// Generates level objects from the level data, using our save system to unlock certain levels
    /// </summary>
    private void GenerateLevels()
    {
        // Generate our locked levels:
        for (int i = 0; i < levelsData.levels.Count; i++)
        {
            GameObject level = Instantiate(levelPrefab, transform);
            level.name = $"{levelsData.levels[i].GetLevelName()}";

            LevelTemplateUI ui = level.GetComponent<LevelTemplateUI>();
            ui.Initialise(levelsData.levels[i]);

            levelObjects.Add(ui);
            level.SetActive(false);
        }

        // Then unlock them:
        UnlockLevel(0);         // Always unlock first level
        
        // Unlock all levels if we're using the debug setting to do so
        if (unlockAllLevelsSetting.GetCurrentValue())
        {
            for (int i = 1; i < levelsData.levels.Count; i++)
            {
                UnlockLevel(i);
            }
        }
        // Unlock levels according to our game save, if one exists
        else
        {
            GameSaveData data = SaveSystem.LoadGameData();
            if (data != null)
            {
                for (int i = 1; i < data.levelUnlocked + 1; i++)
                {
                    UnlockLevel(i);
                }
            }
        }

        SelectLevel(0);
    }

    private void UnlockLevel(int index)
    {
        levelObjects[index].Unlock();
    }

    public void SelectLevel(int levelIndex)
    {
        levelObjects[selectedLevel].gameObject.SetActive(false);
        levelObjects[levelIndex].gameObject.SetActive(true);

        selectedLevel = levelIndex;

        // Disable/enable our start buttons if our selected level is locked/unlocked
        for (int i = 0; i < startButtons.Length; i++)
        {
            if (unlockAllLevelsSetting.GetCurrentValue())
            {
                startButtons[i].interactable = true;
            }
            else
            {
                startButtons[i].interactable = SaveSystem.IsLevelUnlocked(levelIndex);
            }
        }

        // Select our first start button, if it's interactable
        if (startButtons[0].interactable)
        {
            startButtons[0].Select();
        }
    }

    public void NextLevel()
    {
        if (selectedLevel == levelsData.levels.Count - 1)
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
            SelectLevel(levelsData.levels.Count - 1);
        }
        else
        {
            SelectLevel(selectedLevel - 1);
        }
    }

    public void StartLevel()
    {
        SaveSystem.DeleteLevelData();
        levelsData.LoadLevel(selectedLevel);
    }
}