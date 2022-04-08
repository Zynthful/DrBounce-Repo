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
    public string GetName() { return name; }
    public void SetName(string value) { name = value; }

    [SerializeField]
    [Tooltip("Used for displaying the color of the speaker's name in subtitles.")]
    private Color color;
    public Color GetColor() { return color; }
    public void SetColor(Color value) { color = value; }

    private GameObject sourceObject = null;     // Used for playing dialogue from this object
    public GameObject GetSourceObject() { return sourceObject; }
    public void SetSourceObject(GameObject value) { sourceObject = value; }
}