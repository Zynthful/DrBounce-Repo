using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogueSubtitleUI : MonoBehaviour
{
    [Header("Text Object References")]
    [SerializeField]
    private TextMeshProUGUI speakerText = null;
    [SerializeField]
    private TextMeshProUGUI dialogueText = null;

    [Header("Events")]
    public UnityEvent onShowSubtitle = null;
    public UnityEvent onDisableSubtitle = null;

    public void ShowSubtitle(DialogueData line)
    {
        onShowSubtitle?.Invoke();

        // Update speaker text
        speakerText.text = $"{line.GetSpeaker().GetName()}:";
        speakerText.color = line.GetSpeaker().GetColor();
    }

    public void SetSubtitleText(string text)
    {
        dialogueText.text = text;
    }

    public void Disable()
    {
        onDisableSubtitle?.Invoke();

        this.gameObject.SetActive(false);
    }
}