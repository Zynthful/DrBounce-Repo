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
        line.GetEvent().Post(@object, (uint)AkCallbackType.AK_EndOfEvent, OnEndOfEvent);

        // Show subtitles
        dialogueCanvas.SetActive(true);
        subtitleUI.ShowSubtitle(line);
    }

    private void OnEndOfEvent(object in_cookie, AkCallbackType in_type, object in_info)
    {
        subtitleUI.Disable();
    }
}