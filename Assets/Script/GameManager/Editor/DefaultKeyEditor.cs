using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DefaultKeyBindings))]
public class DefaultKeyBindingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DefaultKeyBindings keyBindings = (DefaultKeyBindings)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Load From InputActionAsset Path"))
        {
            keyBindings.LoadFromInputActions();
        }
    }
}
