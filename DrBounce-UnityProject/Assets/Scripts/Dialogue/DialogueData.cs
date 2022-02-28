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

    [Header("Randomisation Settings")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float chanceToPlay = 1.0f;

    [Header("Cooldown Settings")]
    [SerializeField]
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

    private void Reset()
    {
        coolingDown = false;
    }
}