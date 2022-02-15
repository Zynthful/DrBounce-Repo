using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Selectable))]
public class SelectableAudio : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    protected Selectable selectable;

    [Header("Selectable AkEvents")]
    [SerializeField]
    protected AK.Wwise.Event onSelect = null;
    [SerializeField]
    protected AK.Wwise.Event onPointerEnter = null;

    [Header("Selectable AkSwitches")]
    [SerializeField]
    protected AK.Wwise.Switch selectableType = null;

    protected virtual void Awake()
    {
        if (selectable == null)
        {
            selectable = GetComponent<Selectable>();
        }

        selectableType.SetValue(selectable.gameObject);
    }

    // Called when the cursor enters the rect area of this selectable UI object.
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (selectable != null && selectable.IsInteractable())
        {
            onPointerEnter?.Post(gameObject);
        }
    }

    // Called when the selectable UI object is selected.
    public virtual void OnSelect(BaseEventData eventData)
    {
        onSelect?.Post(gameObject);
    }
}