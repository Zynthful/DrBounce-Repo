using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputActionSetting)), CanEditMultipleObjects]
public class InputActionSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InputActionSetting setting = (InputActionSetting)target;
        
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
        GUILayout.Label($"Path: {setting.GetInputBinding().path}", bindingStyle);
        GUILayout.Label($"Group: {setting.GetInputBinding().groups}", bindingStyle);
        string composite = setting.GetInputBinding().isComposite ? "Yes" : "No";
        GUILayout.Label($"Is Composite: {composite}", bindingStyle);
        GUILayout.Label($"Interactions: {setting.GetInputBinding().interactions}", bindingStyle);
        GUILayout.Label($"Processors: {setting.GetInputBinding().processors}", bindingStyle);
        GUILayout.Label($"ID: {setting.GetInputBinding().id}", bindingStyle);
    }
}
