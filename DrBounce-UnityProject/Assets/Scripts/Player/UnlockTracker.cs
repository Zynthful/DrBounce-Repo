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
    }

    [SerializeField]
    private Unlocks levelStartSettings;

    public static UnlockTracker instance;


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
    public UnityEvent disableFirstDash = null;
    [SerializeField]
    public UnityEvent disableSecondDash = null;
    [SerializeField]
    public UnityEvent disableMagnet = null;
    [SerializeField]
    public UnityEvent disableSlide = null;

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
    }

    public void NewUnlocks(UnlockTypes[] newUnlocks)
    {
        foreach(UnlockTypes unlock in newUnlocks)
        {
            EnableUnlock(unlock);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        DisableAllUnlocks();
        if(levelStartSettings != null)
        {
            NewUnlocks(levelStartSettings.unlocks);
        }
        else
            Debug.Log("Running without any unlocks, have you forgotten to setup the scriptableObject?");
    }
}
