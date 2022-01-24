using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]

public class LevelData : ScriptableObject
{
    // TODO: look into dragging in scene object as reference instead of relying on scene name or index
    // unfortunately the below code doesn't work for that ;_;
    /*
    [SerializeField]
    private Scene scene = new Scene();
    */

    [SerializeField]
    [Tooltip("This name is reflected in-game, e.g., 'BouncePad'.")]
    private string levelName = "New Level";

    [SerializeField]
    [Tooltip("This should be the exact name of the scene asset, e.g., 'BouncePad_SCN'.")]
    private string sceneName = "Scene Name";

    [SerializeField]
    [Tooltip("This sprite is reflected in-game.")]
    private Sprite previewSprite = null;

    public void SetLevelName(string value) { levelName = value; }
    public string GetLevelName() { return levelName; }

    public void SetSceneName(string value) { sceneName = value; }
    public string GetSceneName() { return sceneName; }

    public void SetSprite(Sprite value) { previewSprite = value; }
    public Sprite GetSprite() { return previewSprite; }
}
