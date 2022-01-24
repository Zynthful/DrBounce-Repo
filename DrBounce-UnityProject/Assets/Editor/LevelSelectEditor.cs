using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSelect))]
public class LevelSelectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelSelect levelSelect = (LevelSelect)target;

        if (GUILayout.Button("Regenerate Levels"))
        {
            levelSelect.RegenerateLevels();
        }
    }
}
