using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class LevelTemplateUI : MonoBehaviour
{
    private LevelData data = null;

    [Header("Declarations")]
    [SerializeField]
    new private TextMeshProUGUI name = null;
    [SerializeField]
    private TextMeshProUGUI description = null;
    [SerializeField]
    private TextMeshProUGUI pbTime = null;
    [SerializeField]
    private Image previewImage = null;

    [Header("Events")]
    public UnityEvent onLock;
    public UnityEvent onUnlock;

    public void Initialise(LevelData _data)
    {
        data = _data;

        Lock();

        previewImage.preserveAspect = true;

        float minutes = Mathf.Floor(data.GetPBTime() / 60);
        float seconds = Mathf.Round(data.GetPBTime() - (minutes * 60));
        pbTime.text = $"{minutes}:{seconds}";
    }

    private void Lock()
    {
        onLock.Invoke();
        name.text = data.GetLockedName();
        description.text = data.GetLockedDescription();
        previewImage.sprite = data.GetLockedSprite();
    }

    public void Unlock()
    {
        onUnlock.Invoke();
        name.text = data.GetLevelName();
        description.text = data.GetDescription();
        previewImage.sprite = data.GetSprite();
    }
}