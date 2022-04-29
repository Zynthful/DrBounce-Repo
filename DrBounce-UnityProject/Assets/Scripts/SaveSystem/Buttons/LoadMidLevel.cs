using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMidLevel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The button that is used to load a checkpoint. This will be made uninteractable/interactable depending on if a checkpoint can be reloaded or not.")]
    private Button button = null;

    [SerializeField]
    [Tooltip("Used for updating the button navigation. This should be the parent of the button used for loading the checkpoint.")]
    private SelectableVerticalNavigation nav = null;

    [SerializeField]
    private LevelsData levelsData = null;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("LoadMidLevel: No button set or no button has been detected on this object.", gameObject);
                enabled = false;    // Disable this component if the button is not valid
                return;
            }
            else if (nav == null)
            {
                nav = button.GetComponentInParent<SelectableVerticalNavigation>();
                if (nav == null)
                {
                    Debug.LogError("LoadMidLevel: SelectableVerticalNavigation has not been set or detected.", gameObject);
                    enabled = false;
                }
            }
        }
    }

    private void OnEnable()
    {
        CheckpointHit.onHit += _ => SetInteractable(true);
        SetInteractable(SaveSystem.LevelSaveExists(levelsData.GetCurrentLevelIndex()));
    }

    private void OnDisable()
    {
        CheckpointHit.onHit -= _ => SetInteractable(true);
    }

    void SetInteractable(bool value)
    {
        button.interactable = value;
        nav.FindNavigation();           // Update button navigation
    }

    public void OnClickedLoad()
    {
        LevelSaveData data = SaveSystem.LoadInLevel();

        if(levelsData.GetCurrentLevelIndex() != data.level)
        {
            Debug.LogError("Load level failed, not on the correct level, this button should be disabled");
            Debug.LogError("Current Level " + SceneManager.GetActiveScene().buildIndex + ", desired level: " + data.level);
            return;
        }
        else
        {
            Checkpoint.ReloadFromCheckpoint();
        }
    }
}
