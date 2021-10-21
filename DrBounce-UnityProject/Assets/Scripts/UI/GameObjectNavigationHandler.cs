using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameObjectNavigationHandler : MonoBehaviour
{
    public void Select(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
    }
}
