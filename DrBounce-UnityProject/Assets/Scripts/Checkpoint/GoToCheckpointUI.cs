using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoToCheckpointUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown dropdown = null;

    private void OnEnable()
    {
        InitialiseDropdown();
    }

    private void InitialiseDropdown()
    {
        if (dropdown == null)
        {
            dropdown = GetComponent<TMP_Dropdown>();
            if (dropdown == null)
            {
                dropdown = GetComponentInChildren<TMP_Dropdown>();
                if (dropdown == null)
                {
                    Debug.LogError($"{name}: A dropdow has not been assigned (or could not be found attached) to this component.", this);
                    return;
                }
            }
        }

        dropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < Checkpoint.GetCheckpoints().Length; i++)
        {
            options.Add(Checkpoint.GetCheckpoints()[i].name);
        }
        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(GoToCheckpoint);
    }

    private void OnDisable()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.RemoveListener(GoToCheckpoint);
        }
    }

    public void GoToCheckpoint(int index)
    {
        Checkpoint.GoToCheckpoint(index);
        PauseHandler.Unpause();
    }
}