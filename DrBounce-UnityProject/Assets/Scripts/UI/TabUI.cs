using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TabUI : MonoBehaviour
{
    private int selectedTabIndex = 0;

    [SerializeField]
    private Tab[] tabs = null;

    [System.Serializable]
    private struct Tab
    {
        [Tooltip("The button which, when clicked, selects the tab.")]
        public UnityEngine.UI.Button button;
        [Tooltip("The GameObjects which are enabled when selecting the tab.")]
        public GameObject[] gameObjects;

        public UnityEvent onSelect;

        public void SetActive(bool active)
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.SetActive(active);
            }
        }
    }

    private void Awake()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            int temp = i;   // needed as using i for the below delegate doesn't work (since it's a pointer or something). i spent 30mins debugging this.. help...
            tabs[i].button.onClick.AddListener(() => SelectTab(temp));

            // Disable all tabs
            tabs[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        // Listen for input
        InputManager.inputMaster.Menu.Next.performed += OnNextPerformed;
        InputManager.inputMaster.Menu.Previous.performed += OnPreviousPerformed;

        // Select (or re-select) the starting tab (or our last selected one if we're re-selecting)
        SelectTab(selectedTabIndex);
    }

    private void OnDisable()
    {
        // Stop listening for input
        InputManager.inputMaster.Menu.Next.performed -= OnNextPerformed;
        InputManager.inputMaster.Menu.Previous.performed -= OnPreviousPerformed;
    }

    private void OnNextPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Next();
        }
    }

    private void OnPreviousPerformed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Previous();
        }
    }

    private void SelectTab(int index)
    {
        tabs[selectedTabIndex].SetActive(false);    // Disable last tab objects
        tabs[index].SetActive(true);                // Enable new tab objects
        tabs[index].onSelect.Invoke();
        selectedTabIndex = index;
    }

    private void Next()
    {
        if (selectedTabIndex == tabs.Length - 1)
        {
            SelectTab(0);   // select first tab if we've reached the end
        }
        else
        {
            SelectTab(selectedTabIndex + 1);
        }
    }

    private void Previous()
    {
        if (selectedTabIndex == 0)
        {
            SelectTab(tabs.Length - 1);     // select last tab if we've reached the start
        }
        else
        {
            SelectTab(selectedTabIndex - 1);
        }
    }
}
