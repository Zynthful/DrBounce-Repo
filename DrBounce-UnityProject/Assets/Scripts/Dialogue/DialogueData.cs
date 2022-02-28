using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Line", menuName = "ScriptableObjects/Dialogue/Dialogue Line")]
public class DialogueData : ScriptableObject
{
    [Header("Dialogue Data")]
    [SerializeField]
    [Tooltip("The Wwise event to be posted when this dialogue line is played.")]
    private AK.Wwise.Event @event = null;
    [SerializeField]
    [Tooltip("The Dialogue Speaker Data who speaks this dialogue line.")]
    private DialogueSpeakerData speaker = null;
    [SerializeField]
    private string subtitle = null; // todo: rework subtitle to use marker callbacks from Wwise

    [Header("Randomisation Settings")]
    [SerializeField]
    [Tooltip("The overall chance to play this dialogue line. 1 = always plays, 0 = never plays.")]
    [Range(0.0f, 1.0f)]
    private float chanceToPlay = 1.0f;

    [Header("Cooldown Settings")]
    [SerializeField]
    [Tooltip("The duration in seconds that this dialogue line must wait after playing before being able to play again.")]
    private float cooldown = 0.0f;

    private bool coolingDown = false;

    public AK.Wwise.Event GetEvent() { return @event; }
    public DialogueSpeakerData GetSpeaker() { return speaker; }
    public string GetSubtitle() { return subtitle; }
    public void SetCoolingDown(bool value) { coolingDown = value; }
    public bool GetCoolingDown() { return coolingDown; }

    private void OnEnable()
    {
        Reset();
    }

    private void OnDisable()
    {
        Reset();
    }

    public void Play(GameObject @object)
    {
        // Check against cooldown and chance to play
        if (!coolingDown && Random.Range(0.0f, 1.0f) <= chanceToPlay)
        {
            DialogueManager.s_Instance.PlayDialogueLine(this, @object);
            DialogueManager.s_Instance.StartCoroutine(DialogueManager.s_Instance.Cooldown(this, cooldown));
        }
    }

    /// <summary>
    /// Resets cooldown.
    /// </summary>
    private void Reset()
    {
        coolingDown = false;
    }
}