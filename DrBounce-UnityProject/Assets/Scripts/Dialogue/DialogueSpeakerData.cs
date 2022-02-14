using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker", menuName = "ScriptableObjects/Dialogue/Speaker")]
public class DialogueSpeakerData : ScriptableObject
{
    [Header("Speaker Data")]
    [SerializeField]
    [Tooltip("Used for displaying the speaker's name in subtitles.")]
    new private string name;
    [SerializeField]
    [Tooltip("Used for displaying the color of the speaker's name in subtitles.")]
    private Color color;

    public string GetName() { return name; }
    public Color GetColor() { return color; }
}