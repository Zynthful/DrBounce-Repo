using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(SelectableNavigation))]
public class SelectableNavigationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SelectableNavigation nav = (SelectableNavigation)target;

        if (GUILayout.Button("Find Navigation"))
        {
            nav.FindNavigation();
        }
    }
}