using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(SelectableGridNavigation))]
public class SelectableGridNavigationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectableGridNavigation nav = (SelectableGridNavigation)target;

        if (GUILayout.Button("Find Navigation"))
        {
            nav.FindNavigation();
        }
    }
}