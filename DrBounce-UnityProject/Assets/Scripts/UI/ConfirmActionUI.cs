using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmActionUI : MonoBehaviour
{
    [SerializeField]
    private Confirmation[] confirmations = null;

    [System.Serializable]
    private struct Confirmation
    {
        public string actionName;
        public UnityEvent onConfirm;
        public GameObject[] gameObjects;
        public UnityEngine.UI.Button confirmButton;
        public UnityEngine.UI.Button cancelButton;

        public void SetActive(bool active)
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(active);
            }
        }
    }

    public void Prompt(string actionName)
    {
        for (int i = 0; i < confirmations.Length; i++)
        {
            if (confirmations[i].actionName == actionName)
            {
                Prompt(i);
            }
        }
    }

    public void Prompt(int actionIndex)
    {
        // Enable menu
        confirmations[actionIndex].SetActive(true);

        // Listen to confirm and cancel buttons
        confirmations[actionIndex].confirmButton.onClick.AddListener(() => Confirm(actionIndex));
        confirmations[actionIndex].cancelButton.onClick.AddListener(() => Confirm(actionIndex));

    }

    private void Confirm(int actionIndex)
    {
        confirmations[actionIndex].onConfirm.Invoke();
        confirmations[actionIndex].SetActive(false);
    }

    private void Cancel(int actionIndex)
    {
        confirmations[actionIndex].SetActive(false);
        
        // Select origin button
    }
}
