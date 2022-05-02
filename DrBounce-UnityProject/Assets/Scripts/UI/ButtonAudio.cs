using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAudio : SelectableAudio, IPointerClickHandler
{
    protected Button button = null;

    [Header("Button AkEvents")]
    [SerializeField]
    protected AK.Wwise.Event onClickSuccess = null;
    [SerializeField]
    protected AK.Wwise.Event onClickFail = null;

    [Header("Button AkSwitches")]
    [SerializeField]
    protected AK.Wwise.Switch buttonType = null;

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
        onClickSuccess?.Post(button.gameObject);
    }

    protected virtual void ClickFail()
    {
        onClickFail?.Post(button.gameObject);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            ClickFail();
        }
    }
}