using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_Instance = null;

    [SerializeField]
    private GameObject dialogueCanvas = null;

    private DialogueSubtitleUI subtitleUI = null;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = FindObjectOfType(typeof(DialogueManager)) as DialogueManager;
        }

        if (s_Instance == null)
        {
            s_Instance = this;
        }

        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }

        dialogueCanvas = Instantiate(dialogueCanvas);
        dialogueCanvas.SetActive(false);
        subtitleUI = dialogueCanvas.GetComponent<DialogueSubtitleUI>();
    }

    public void PlayDialogueLine(DialogueData line, GameObject @object)
    {
        line.GetEvent().Post(@object);

        // Show subtitles
        dialogueCanvas.SetActive(true);
        subtitleUI.ShowSubtitle(line);
    }
}