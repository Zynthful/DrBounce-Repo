using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]

public class LevelData : ScriptableObject
{
    [SerializeField]
    private string levelName = "New Level";

    [SerializeField]
    private string sceneName = "Scene Name";

    [SerializeField]
    private Sprite previewSprite = null;

    public void SetLevelName(string value) { levelName = value; }
    public string GetLevelName() { return levelName; }

    public void SetSceneName(string value) { sceneName = value; }
    public string GetSceneName() { return sceneName; }

    public void SetSprite(Sprite value) { previewSprite = value; }
    public Sprite GetSprite() { return previewSprite; }

}
