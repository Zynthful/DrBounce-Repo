using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnlockTracker : MonoBehaviour
{
    [System.Serializable]
    public enum UnlockTypes
    {
        FirstDash,
        SecondDash,
        Magnet,
        Slide,
        NormalShooting,
    }

    [SerializeField]
    private Unlocks levelStartSettings;

    public static UnlockTracker instance;

    public bool saveValues;
    public UnlockTypes[] saveUnlocks;

    [HideInInspector]
    public UnlockTypes? lastUnlock = null;      // Last unlock acquired
    [HideInInspector]
    public bool usedUnlock = false;     // False when acquired a new unlock, True after using the newly acquired unlock

    [Header("Enable Unlock Events")]
    public UnityEvent unlockFirstDash = null;
    public UnityEvent unlockSecondDash = null;
    public UnityEvent unlockMagnet = null;
    public UnityEvent unlockSlide = null;
    public UnityEvent unlockNormalShooting = null;

    [Header("Disable Unlock Events")]
    public UnityEvent disableFirstDash = null;
    public UnityEvent disableSecondDash = null;
    public UnityEvent disableMagnet = null;
    public UnityEvent disableSlide = null;
    public UnityEvent disableNormalShooting = null;

    [Header("Use Unlock Events")]
    public UnityEvent usedUnlockFirstTime = null;

    void EnableUnlock(UnlockTypes type)
    {
        switch(type)
        {
            case UnlockTypes.FirstDash:
                unlockFirstDash?.Invoke();
                break;

            case UnlockTypes.SecondDash:
                unlockSecondDash?.Invoke();
                break;

            case UnlockTypes.Magnet:
                unlockMagnet?.Invoke();
                break;

            case UnlockTypes.Slide:
                unlockSlide?.Invoke();
                break;

            case UnlockTypes.NormalShooting:
                unlockNormalShooting?.Invoke();
                break;

            default:
                //Debug.LogError("Type not setup with an event, @Cole for being dumb");
                break;
        }
    }

    void DisableAllUnlocks()
    {
        disableSecondDash?.Invoke();
        disableFirstDash?.Invoke();
        disableMagnet?.Invoke();
        disableSlide?.Invoke();
        disableNormalShooting?.Invoke();
    }

    public void NewUnlocks(UnlockTypes[] newUnlocks)
    {
        foreach(UnlockTypes unlock in newUnlocks)
        {
            //Debug.Log("Unlocking: " + unlock);
            EnableUnlock(unlock);
        }
        GameManager.s_Instance.currentSettings = newUnlocks;
    }

    public void PickupNewUnlocks(UnlockTypes[] newUnlocks)
    {
        lastUnlock = newUnlocks[newUnlocks.Length - 1];
        usedUnlock = false;
        NewUnlocks(newUnlocks);
    }

    public void ReloadUnlocks()
    {
        foreach (UnlockTypes unlock in GameManager.s_Instance.currentSettings)
        {
            EnableUnlock(unlock);
        }
    }

    public void UsedUnlockFirstTime()
    {
        usedUnlock = true;
        usedUnlockFirstTime.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        DisableAllUnlocks();
        if (levelStartSettings != null && !saveValues)
        {
            NewUnlocks(levelStartSettings.unlocks);
            GameManager.s_Instance.currentSettings = levelStartSettings.unlocks;
        }
        else if (saveValues)
        {
            NewUnlocks(saveUnlocks);
            GameManager.s_Instance.currentSettings = saveUnlocks;
            saveValues = false;
        }
        else { }
            //Debug.Log("Running without any unlocks, have you forgotten to setup the scriptableObject?");
    }
}
