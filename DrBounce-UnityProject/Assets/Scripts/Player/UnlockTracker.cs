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

    // Events to enable/disable unlocks
    [SerializeField]
    public UnityEvent unlockFirstDash = null;
    [SerializeField]
    public UnityEvent unlockSecondDash = null;
    [SerializeField]
    public UnityEvent unlockMagnet = null;
    [SerializeField]
    public UnityEvent unlockSlide = null;
    [SerializeField]
    public UnityEvent unlockNormalShooting = null;
    [SerializeField]
    public UnityEvent disableFirstDash = null;
    [SerializeField]
    public UnityEvent disableSecondDash = null;
    [SerializeField]
    public UnityEvent disableMagnet = null;
    [SerializeField]
    public UnityEvent disableSlide = null;
    [SerializeField]
    public UnityEvent disableNormalShooting = null;

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
                Debug.LogError("Type not setup with an event, @Cole for being dumb");
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
            Debug.Log("Unlocking: " + unlock);
            EnableUnlock(unlock);
        }
        GameManager.s_Instance.currentSettings = newUnlocks;
    }

    public void ReloadUnlocks()
    {
        foreach (UnlockTypes unlock in GameManager.s_Instance.currentSettings)
        {
            EnableUnlock(unlock);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        DisableAllUnlocks();
        if(levelStartSettings != null && !saveValues)
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
        else
            Debug.Log("Running without any unlocks, have you forgotten to setup the scriptableObject?");
    }
}
