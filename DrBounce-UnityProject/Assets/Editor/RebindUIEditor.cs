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
        GUILayout.Label($"Path: {rebindUI.GetInputBinding().path}", bindingStyle);
        GUILayout.Label($"Group: {rebindUI.GetInputBinding().groups}", bindingStyle);
        string composite = rebindUI.GetInputBinding().isComposite ? "Yes" : "No";
        GUILayout.Label($"Is Composite: {composite}", bindingStyle);
        GUILayout.Label($"Interactions: {rebindUI.GetInputBinding().interactions}", bindingStyle);
        GUILayout.Label($"Processors: {rebindUI.GetInputBinding().processors}", bindingStyle);
        GUILayout.Label($"ID: {rebindUI.GetInputBinding().id}", bindingStyle);
    }
}
