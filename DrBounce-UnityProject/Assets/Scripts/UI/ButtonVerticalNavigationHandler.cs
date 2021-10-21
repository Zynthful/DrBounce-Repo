using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVerticalNavigationHandler : MonoBehaviour
{
    [SerializeField]
    private Button[] buttons = null;

    private void OnEnable()
    {
        FindVerticalButtonNavigation(RemoveUninteractableButtons(buttons));
    }

    private Button[] RemoveUninteractableButtons(Button[] buttons)
    {
        List<Button> tempButtons = new List<Button>(buttons);

        for (int i = 0; i < tempButtons.Count; i++)
        {
            if (!tempButtons[i].interactable)
            {
                tempButtons.RemoveAt(i);
                i--;
            }
        }

        return tempButtons.ToArray();
    }

    private void FindVerticalButtonNavigation(Button[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation nav = new Navigation();
            nav.mode = Navigation.Mode.Explicit;

            nav.selectOnUp = i == 0 ? buttons[buttons.Length - 1] : buttons[i - 1];
            nav.selectOnDown = i == buttons.Length - 1 ? buttons[0] : buttons[i + 1];

            buttons[i].navigation = nav;
        }
    }
}
