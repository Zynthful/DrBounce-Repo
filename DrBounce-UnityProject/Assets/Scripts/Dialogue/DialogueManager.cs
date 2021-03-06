using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_Instance;
    private GameObject uiInstance;

    [SerializeField]
    private GameObject dialogueCanvas;

    private DialogueSubtitleUI subtitleUI;

    private DialogueData lastPlayedDialogue;
    private GameObject lastSourceObject;

    private bool playing = false;

    [SerializeField]
    private BoolSetting enableSubtitlesSetting;

    [Header("Global Cooldown Settings")]
    [SerializeField]
    [Tooltip("The global cooldown in seconds, triggered everytime a new dialogue line is played. No dialogue (except that which overrides) may play during this cooldown period.")]
    private float globalCooldown = 0.0f;

    private bool coolingDown = false;
    private Coroutine globalCooldownCoroutine;

    [SerializeField]
    private float subtitleDisableDelay = 1.2f;
    private Coroutine delayDisableUICoroutine;

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
    }

    private void CheckNullUI()
    {
        if (uiInstance == null || subtitleUI == null)
        {
            InitialiseUI();
        }
    }

    private void InitialiseUI()
    {
        // Don't create our UI if subtitles are disabled
        if (!enableSubtitlesSetting.GetCurrentValue())
            return;

        uiInstance = Instantiate(dialogueCanvas);
        uiInstance.SetActive(false);
        subtitleUI = uiInstance.GetComponent<DialogueSubtitleUI>();
    }

    public void PlayDialogueLine(DialogueData line, GameObject @object)
    {
        playing = true;

        // Stop previous dialogue if it exists
        if (lastPlayedDialogue != null)
            lastPlayedDialogue.GetEvent().Stop(lastSourceObject);

        // Stop disabling subtitle UI if it is about to be disabled
        if (delayDisableUICoroutine != null)
        {
            StopCoroutine(delayDisableUICoroutine);
            delayDisableUICoroutine = null;
        }

        // Play dialogue
        line.GetEvent().Post(@object, (uint)(AkCallbackType.AK_Marker | AkCallbackType.AK_EndOfEvent), Callback);
        lastPlayedDialogue = line;
        lastSourceObject = @object;

        // Show subtitles
        if (enableSubtitlesSetting.GetCurrentValue())
        {
            CheckNullUI();
            subtitleUI.ShowSubtitle(line);
        }

        // Stop any running global cooldowns
        if (globalCooldownCoroutine != null && coolingDown)
            StopCoroutine(globalCooldownCoroutine);

        // Start new global cooldown
        globalCooldownCoroutine = StartCoroutine(GlobalCooldown(globalCooldown));
    }

    
    private void Callback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {
        switch (in_type)
        {
            case AkCallbackType.AK_EndOfEvent:
                OnEndOfEvent();
                break;
            case AkCallbackType.AK_Marker:
                OnMarker((AkMarkerCallbackInfo)in_info);
                break;
        }
    }

    public void OnEndOfEvent()
    {
        delayDisableUICoroutine = StartCoroutine(DelayDisableUI(subtitleDisableDelay));
        playing = false;
    }

    public void OnMarker(AkMarkerCallbackInfo info)
    {
        CheckNullUI();

        // Update text with subtitle read from marker on audio file
        subtitleUI.SetSubtitleText(info.strLabel);

        // Show subtitles if we have them enabled, otherwise hide them
        uiInstance.SetActive(enableSubtitlesSetting.GetCurrentValue());
    }

    public IEnumerator Cooldown(DialogueData data, float duration)
    {
        data.SetCoolingDown(true);
        yield return new WaitForSeconds(duration);
        data.SetCoolingDown(false);
    }

    public IEnumerator GlobalCooldown(float duration)
    {
        coolingDown = true;
        yield return new WaitForSeconds(duration);
        coolingDown = false;
    }

    private IEnumerator DelayDisableUI(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (subtitleUI != null)
            subtitleUI.Disable();
    }

    public DialogueData GetLastPlayed() { return lastPlayedDialogue; }
    public bool GetIsPlaying() { return playing; }
    public bool GetIsCoolingDown() { return coolingDown; }
}