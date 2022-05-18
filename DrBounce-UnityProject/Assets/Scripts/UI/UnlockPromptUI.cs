using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class UnlockPromptUI : MonoBehaviour
{
    public TextMeshProUGUI unlockNameText = null;
    public VideoPlayer videoPlayer = null;

    public void Close()
    {
        PauseHandler.SetTimeFreeze(false);
        PauseHandler.SetCanPause(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(gameObject);
    }
}