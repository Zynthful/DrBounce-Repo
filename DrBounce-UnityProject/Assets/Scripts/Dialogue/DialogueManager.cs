using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_Instance = null;
    private GameObject uiInstance = null;

    [SerializeField]
    private GameObject dialogueCanvas = null;

    private DialogueSubtitleUI subtitleUI = null;

    private DialogueData lastPlayedDialogue = null;

    private bool playing = false;

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
        uiInstance = Instantiate(dialogueCanvas);
        uiInstance.SetActive(false);
        subtitleUI = uiInstance.GetComponent<DialogueSubtitleUI>();
    }

    public void PlayDialogueLine(DialogueData line, GameObject @object)
    {
        playing = true;

        CheckNullUI();

        line.GetEvent().Post(@object, (uint)(AkCallbackType.AK_Marker | AkCallbackType.AK_EndOfEvent), Callback);
        lastPlayedDialogue = line;

        subtitleUI.ShowSubtitle(line);
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
        subtitleUI.Disable();
        playing = false;
    }

    public void OnMarker(AkMarkerCallbackInfo info)
    {
        CheckNullUI();

        // Show subtitles
        uiInstance.SetActive(true);

        // Update text with subtitle read from marker on audio file
        subtitleUI.SetSubtitleText(info.strLabel);
    }

    public IEnumerator Cooldown(DialogueData data, float duration)
    {
        data.SetCoolingDown(true);
        yield return new WaitForSeconds(duration);
        data.SetCoolingDown(false);
    }

    public DialogueData GetLastPlayed() { return lastPlayedDialogue; }
    public bool GetIsPlaying() { return playing; }
}