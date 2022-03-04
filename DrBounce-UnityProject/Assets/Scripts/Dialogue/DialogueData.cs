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

    [Header("Playback Settings")]

    [SerializeField]
    [Tooltip("The overall chance to play this dialogue line. 1 = always plays, 0 = never plays.")]
    [Range(0.0f, 1.0f)]
    private float chanceToPlay = 1.0f;

    [SerializeField]
    [Tooltip("The duration in seconds that this dialogue line must wait after playing before being able to play again.")]
    private float cooldown = 0.0f;

    [SerializeField]
    [Tooltip("If true, this dialogue will only ever trigger once.")]
    private bool triggerOnce = false;

    [SerializeField]
    [Tooltip("If true, other dialogue lines can interrupt this dialogue line. This overrides whether other dialogue lines can interrupt or not.")]
    private bool canBeInterrupted = true;

    [SerializeField]
    [Tooltip("If true, this dialogue line can interrput other dialogue lines.")]
    private bool canInterrupt = true;

    private bool coolingDown = false;
    private bool triggered = false;

    public AK.Wwise.Event GetEvent() { return @event; }
    public DialogueSpeakerData GetSpeaker() { return speaker; }
    public void SetCoolingDown(bool value) { coolingDown = value; }
    public bool GetCoolingDown() { return coolingDown; }
    public bool GetCanBeInterrupted() { return canBeInterrupted; }
    public bool GetCanInterrupt() { return canInterrupt; }

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
        // Prevent re-triggering if we can only trigger once or we're cooling down
        if ((triggerOnce && triggered) || coolingDown)
            return;

        // Prevent interrupting if we can't interrupt and dialogue is currently being played
        else if (!canInterrupt && DialogueManager.s_Instance.GetIsPlaying())
            return;

        // Prevent interrupting if any currently playing dialogue is uninterruptable
        else if (DialogueManager.s_Instance.GetLastPlayed() != null && !DialogueManager.s_Instance.GetLastPlayed().GetCanBeInterrupted() && DialogueManager.s_Instance.GetIsPlaying())
            return;

        // Check against cooldown and chance to play
        else if (Random.Range(0.0f, 1.0f) <= chanceToPlay)
        {
            triggered = true;
            DialogueManager.s_Instance.PlayDialogueLine(this, @object);
            DialogueManager.s_Instance.StartCoroutine(DialogueManager.s_Instance.Cooldown(this, cooldown));
        }
    }

    /// <summary>
    /// Resets this scriptable object's variables to their initial values.
    /// </summary>
    private void Reset()
    {
        coolingDown = false;
        triggered = false;
    }
}