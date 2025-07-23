using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyManager))]
public class KeyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        KeyManager KeyManager = (KeyManager)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("����� Ű ���� ����"))
        {
            KeyManager.InitializeUserBindingsFromInput();
        }

        if (GUILayout.Button("Jump ���ε� Ȯ��"))
        {
            Debug.Log(KeyManager.GetCurrentKeyDisplay("Jump"));
        }

        if (GUILayout.Button("Jump ���ε� ����"))
        {
            KeyManager.StartListeningForKey("Jump");
        }
    }
}
