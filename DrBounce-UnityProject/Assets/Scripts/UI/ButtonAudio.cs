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
    protected AK.Wwise.Event onClickSuccess = null;
    [SerializeField]
    protected AK.Wwise.Event onClickFail = null;

    protected override void Awake()
    {
        base.Awake();
        if (button == null)
        {
            button = GetComponent<Button>();
        }
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(Click);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(Click);
    }

    protected virtual void Click()
    {
        onClickSuccess?.Post(gameObject);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable)
        {
            Click();
        }
        else
        {
            onClickFail?.Post(gameObject);
        }
    }
}