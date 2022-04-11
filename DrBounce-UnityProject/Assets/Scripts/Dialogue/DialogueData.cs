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
    [Tooltip("The specific dialogue lines that this dialogue line can be interrupted by.")]
    private List<DialogueData> interruptLines = null;

    [SerializeField]
    [Tooltip("If true, ALL other dialogue lines can interrupt this dialogue line. This overrides whether other dialogue lines can interrupt or not.")]
    private bool canBeInterruptedByAll = true;

    [SerializeField]
    [Tooltip("If true, this dialogue line can interrupt other dialogue lines.")]
    private bool canInterrupt = true;

    [SerializeField]
    [Tooltip("If true, this dialogue line will override the dialogue global cooldown.")]
    private bool overrideGlobalCooldown = false;

    private bool coolingDown = false;
    private bool triggered = false;

    public AK.Wwise.Event GetEvent() { return @event; }
    public DialogueSpeakerData GetSpeaker() { return speaker; }
    public void SetCoolingDown(bool value) { coolingDown = value; }
    public bool GetCoolingDown() { return coolingDown; }
    public List<DialogueData> GetInterruptLines() { return interruptLines; }
    public bool GetCanBeInterruptedByAll() { return canBeInterruptedByAll; }
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
        // Check against chance to play
        if (Random.Range(0.0f, 1.0f) > chanceToPlay)
            return;

        // Prevent re-triggering if we can only trigger once or we're cooling down
        else if ((triggerOnce && triggered) || coolingDown)
            return;

        // Prevent playback if the global dialogue cooldown is in effect and we can't override it
        else if (DialogueManager.s_Instance.GetIsCoolingDown() && !overrideGlobalCooldown)
            return;

        // If dialogue is already being played, we need to check if we can interrupt
        else if (DialogueManager.s_Instance.GetIsPlaying())
        {
            if (canInterrupt && (DialogueManager.s_Instance.GetLastPlayed().GetCanBeInterruptedByAll() || DialogueManager.s_Instance.GetLastPlayed().GetInterruptLines().Contains(this)))
            {
                // Interrupt
                ConfirmPlay(@object);
            }
            else
            {
                return;
            }
        }
        else
        {
            ConfirmPlay(@object);
        }
    }

    private void ConfirmPlay(GameObject @object)
    {
        triggered = true;
        DialogueManager.s_Instance.PlayDialogueLine(this, @object);
        DialogueManager.s_Instance.StartCoroutine(DialogueManager.s_Instance.Cooldown(this, cooldown));
    }

    /// <summary>
    /// Attempts to play this dialogue on the source object.
    /// </summary>
    public void PlayFromSource()
    {
        if (speaker != null)
        {
            if (speaker.GetSourceObject() != null)
            {
                Play(speaker.GetSourceObject());
            }
            else
            {
                Debug.LogError($"uh oh, you're trying to play this dialogue line {@event.Name} on speaker {speaker.GetName()}'s source object, but their source object is null! (ask jamie)");
            }
        }
        else
        {
            Debug.LogError($"uh oh, you're trying to play this dialogue line {@event.Name} but the speaker is null! you need to assign a speaker to this dialogue line. (ask jamie)");
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