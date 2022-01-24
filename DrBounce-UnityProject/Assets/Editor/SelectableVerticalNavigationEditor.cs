using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(SelectableVerticalNavigation))]
public class SelectableVerticalNavigationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectableVerticalNavigation nav = (SelectableVerticalNavigation)target;

        if (GUILayout.Button("Find Navigation"))
        {
            nav.FindNavigation();
        }
    }
}