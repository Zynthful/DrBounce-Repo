using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RebindUI))]
public class RebindUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RebindUI rebindUI = (RebindUI)target;
        
        GUILayout.Space(15);

        GUIStyle headerStyle = new GUIStyle();
        headerStyle.fontSize = 15;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.normal.textColor = Color.white;

        GUIStyle bindingStyle = new GUIStyle();
        bindingStyle.fontSize = 12;
        bindingStyle.normal.textColor = Color.white;

        // Display binding input info in inspector
        GUILayout.Label("Selected Binding :)", headerStyle);
        GUILayout.Label($"Path: {rebindUI.GetInputPath()}", bindingStyle);
        GUILayout.Label($"Group: {rebindUI.GetInputGroup()}", bindingStyle);
    }
}
