using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonAudio : SelectableAudio, IPointerClickHandler
{
    protected Button button = null;

    [Header("Button AkEvents")]
    [SerializeField]
    protected AK.Wwise.Event onClick = null;

    [Header("Button AkSwitches")]
    [SerializeField]
    protected AK.Wwise.Switch buttonType = null;
    [SerializeField]
    protected AK.Wwise.Switch clickSuccess = null;
    [SerializeField]
    protected AK.Wwise.Switch clickFail = null;

    protected override void Awake()
    {
        base.Awake();
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        buttonType.SetValue(button.gameObject);
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(ClickSuccess);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(ClickSuccess);
    }

    protected virtual void ClickSuccess()
    {
        clickSuccess?.SetValue(button.gameObject);
        onClick?.Post(button.gameObject);
    }

    protected virtual void ClickFail()
    {
        clickFail?.SetValue(button.gameObject);
        onClick?.Post(button.gameObject);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            ClickFail();
        }
    }
}