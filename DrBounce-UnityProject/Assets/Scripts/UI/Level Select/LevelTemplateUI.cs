using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTemplateUI : MonoBehaviour
{
    [Header("Declarations")]
    [SerializeField]
    new private TextMeshProUGUI name = null;
    [SerializeField]
    private TextMeshProUGUI description = null;
    [SerializeField]
    private TextMeshProUGUI pbTime = null;
    [SerializeField]
    private Image previewImage = null;

    public void SetName(string value) { name.text = value; }
    public void SetDescription(string value) { description.text = value; }
    public void SetPBTime(float value)
    {
        float minutes = Mathf.Floor(value / 60);
        float seconds = Mathf.Round(value - (minutes * 60));
        pbTime.text = $"{minutes}:{seconds}";
    }
    public void SetPreviewSprite(Sprite value)
    {
        previewImage.sprite = value;
        previewImage.preserveAspect = true;
    }
}
