using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelPickup : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject popupPrefab = null;
    [SerializeField]
    private TriggerInvoke trigger = null;

    [Header("Pickup Settings")]
    [SerializeField]
    private Unlocks unlocksOnPickup = null;
    [SerializeField]
    private bool destroyOnPickup = true;
    [SerializeField]
    private float timeBeforeDestroy = .05f;
    [SerializeField]
    private float uiPromptDelay = 1.0f;

    [Header("Events")]
    public UnityEvent onPickup = null;

    private void OnEnable()
    {
        trigger.onDetect.AddListener(Pickup);
    }

    private void OnDisable()
    {
        trigger.onDetect.RemoveListener(Pickup);
    }

    private void Pickup(GameObject obj)
    {
        onPickup.Invoke();
        UnlockTracker.instance.PickupNewUnlocks(unlocksOnPickup.unlocks);
        Player.GetPlayer().StartCoroutine(DelayUIPrompt());

        if (destroyOnPickup)
            Destroy(gameObject, timeBeforeDestroy);

        Destroy(this);
    }

    private IEnumerator DelayUIPrompt()
    {
        yield return new WaitForSeconds(uiPromptDelay);
        ShowUIPrompt();
    }

    private void ShowUIPrompt()
    {
        // Freeze game and show cursor
        PauseHandler.SetTimeFreeze(true);
        PauseHandler.SetCanPause(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Spawn popup
        GameObject popup = Instantiate(popupPrefab);
        popup.name = $"{unlocksOnPickup.name} Pickup Popup";

        // Set popup info
        UnlockPromptUI ui = popup.GetComponent<UnlockPromptUI>();
        ui.videoPlayer.clip = unlocksOnPickup.clip;
        ui.unlockNameText.text = $"{unlocksOnPickup.name} Unlocked!";
    }
}
