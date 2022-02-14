using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Line", menuName = "ScriptableObjects/Dialogue/Dialogue Line")]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue Data")]
    [SerializeField]
    private AK.Wwise.Event @event = null;
    [SerializeField]
    private DialogueSpeakerData speaker = null;
    [SerializeField]
    private string subtitle = null;

    public AK.Wwise.Event GetEvent() { return @event; }
    public DialogueSpeakerData GetSpeaker() { return speaker; }
    public string GetSubtitle() { return subtitle; }

    public void Play(GameObject @object)
    {
        DialogueManager.s_Instance.PlayDialogueLine(this, @object);
    }
}