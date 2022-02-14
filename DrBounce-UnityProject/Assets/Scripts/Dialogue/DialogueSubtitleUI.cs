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

    public void ShowSubtitle(DialogueData line)
    {
        // Update speaker text
        speakerText.text = $"{line.GetSpeaker().GetName()}:";
        speakerText.color = line.GetSpeaker().GetColor();

        // Update dialogue text
        dialogueText.text = $"{line.GetSubtitle()}";
    }
}
