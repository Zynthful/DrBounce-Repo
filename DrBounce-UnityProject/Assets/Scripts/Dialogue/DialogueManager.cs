using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager s_Instance = null;

    [SerializeField]
    private GameObject dialogueCanvas = null;

    private DialogueSubtitleUI subtitleUI = null;

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

        dialogueCanvas = Instantiate(dialogueCanvas);
        dialogueCanvas.SetActive(false);
        subtitleUI = dialogueCanvas.GetComponent<DialogueSubtitleUI>();
    }

    public void PlayDialogueLine(DialogueData line, GameObject @object)
    {
        line.GetEvent().Post(@object, (uint)(AkCallbackType.AK_Marker | AkCallbackType.AK_EndOfEvent), Callback);

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
    }

    public void OnMarker(AkMarkerCallbackInfo info)
    {
        // Show subtitles
        dialogueCanvas.SetActive(true);

        // Update text with subtitle read from marker on audio file
        subtitleUI.SetSubtitleText(info.strLabel);
    }

    public IEnumerator Cooldown(DialogueData data, float duration)
    {
        data.SetCoolingDown(true);
        yield return new WaitForSeconds(duration);
        data.SetCoolingDown(false);
    }
}