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

    private bool coolingDown = false;
    private bool triggered = false;

    public AK.Wwise.Event GetEvent() { return @event; }
    public DialogueSpeakerData GetSpeaker() { return speaker; }
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
        if ((triggerOnce && triggered) || coolingDown)
            return;

        // Check against cooldown and chance to play
        else if (Random.Range(0.0f, 1.0f) <= chanceToPlay)
        {
            Debug.Log("playing!");
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