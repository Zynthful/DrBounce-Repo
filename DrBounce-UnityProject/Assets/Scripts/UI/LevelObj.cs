using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]

public class LevelObj : ScriptableObject
{
    [SerializeField]
    private string levelName = "New Level";

    [SerializeField]
    private int sceneIndex = 0;

    [SerializeField]
    private Sprite previewSprite = null;

    public void SetName(string value) { levelName = value; }
    public string GetName() { return levelName; }

    public void SetIndex(int value) { sceneIndex = value; }

    public int GetIndex() { return sceneIndex; }

    public void SetSprite(Sprite value) { previewSprite = value; }
    public Sprite GetSprite() { return previewSprite; }

}
