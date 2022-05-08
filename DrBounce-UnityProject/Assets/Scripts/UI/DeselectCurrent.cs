using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectCurrent : MonoBehaviour
{
    public void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}